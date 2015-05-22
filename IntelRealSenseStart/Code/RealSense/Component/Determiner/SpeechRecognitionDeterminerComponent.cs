using System;
using System.Linq;
using IntelRealSenseStart.Code.RealSense.Component.Determiner.Builder;
using IntelRealSenseStart.Code.RealSense.Config.RealSense;
using IntelRealSenseStart.Code.RealSense.Data.Properties;
using IntelRealSenseStart.Code.RealSense.Event;
using IntelRealSenseStart.Code.RealSense.Exception;
using IntelRealSenseStart.Code.RealSense.Factory;
using IntelRealSenseStart.Code.RealSense.Helper;
using IntelRealSenseStart.Code.RealSense.Manager;
using IntelRealSenseStart.Code.RealSense.Provider;

namespace IntelRealSenseStart.Code.RealSense.Component.Determiner
{
    public class SpeechRecognitionDeterminerComponent : DeterminerComponent
    {
        public event SpeechEventListener Speech;

        private readonly GrammarBuilder grammarBuilder;
        private readonly RealSenseFactory factory;
        private readonly NativeSense nativeSense;
        private readonly RealSensePropertiesManager propertiesManager;

        private readonly RealSenseConfiguration configuration;

        private readonly PXCMSpeechRecognition.Handler recognitionHandler;

        private PXCMAudioSource audioSource;
        private PXCMSpeechRecognition speechRecognition;

        private bool recognitionStarted;

        private SpeechRecognitionDeterminerComponent(GrammarBuilder grammarBuilder, RealSenseFactory factory, 
            NativeSense nativeSense, RealSensePropertiesManager propertiesManager, RealSenseConfiguration configuration)
        {
            this.grammarBuilder = grammarBuilder;
            this.factory = factory;
            this.nativeSense = nativeSense;
            this.propertiesManager = propertiesManager;
            this.configuration = configuration;

            recognitionHandler = CreateRecognitionHandler();
        }

        private PXCMSpeechRecognition.Handler CreateRecognitionHandler()
        {
            return new PXCMSpeechRecognition.Handler {onRecognition = OnRecognition};
        }

        public bool ShouldBeStarted
        {
            get { return configuration.SpeechRecognitionEnabled; }
        }

        public void EnableFeatures()
        {
        }

        public void Configure()
        {
            audioSource = factory.Native.AudioSource(nativeSense.Session);
            
            var audioProperties = propertiesManager.GetProperties().Audio;
            var deviceProperties = GetSelectedDevice(audioProperties);
            var profileProperties = GetSelectedProfile(audioProperties);

            audioSource.SetDevice(deviceProperties.DeviceInfo);
            CreateSpeechRecognition();
            speechRecognition.SetProfile(profileProperties.Profile);
            grammarBuilder.UseSpeechRecognition(speechRecognition);

            UpdateConfiguration(configuration.SpeechRecognition);
        }

        private AudioInputDeviceProperties GetSelectedDevice(AudioProperties audioProperties)
        {
            try
            {
                return audioProperties.InputDevices.First(configuration.Base.Audio.InputDeviceSelectorFunction);
            }
            catch(InvalidOperationException)
            {
                throw new RealSenseInitializationException(String.Format("No profile for the specified selector is attached"));
            }
        }

        private SpeechRecognitionProfileProperties GetSelectedProfile(AudioProperties audioProperties)
        {
            try
            {
                return audioProperties.SpeechRecognitionModules.SelectMany(module => module.Profiles)
                    .First(configuration.Base.Audio.SpeechRecognitionProfileSelectorFunction);
            }
            catch (InvalidOperationException)
            {
                throw new RealSenseInitializationException(String.Format("No profile for the specified selector could be found"));
            }
        }

        private void CreateSpeechRecognition()
        {
            if (nativeSense.Session.CreateImpl(out speechRecognition) < pxcmStatus.PXCM_STATUS_NO_ERROR)
            {
                throw new RealSenseInitializationException("The speech recognition could not be instantiated");
            }
        }

        public void UpdateConfiguration(SpeechRecognitionConfiguration configuration)
        {
            var startRecognition = false;
            if (recognitionStarted)
            {
                StopRecognition();
                startRecognition = true;
            }

            ApplyConfiguration(configuration);

            if (startRecognition)
            {
                StartRecognition();
            }
        }

        private void ApplyConfiguration(SpeechRecognitionConfiguration configuration)
        {
            audioSource.SetVolume(configuration.Volume);

            if (configuration.UsingGrammar)
            {
                var grammarId = grammarBuilder.GetGrammarId(configuration.Grammar);
                speechRecognition.SetGrammar(grammarId);
            }
            else
            {
                speechRecognition.SetDictation();
            }
        }

        public void StartRecognition()
        {
            if (recognitionStarted)
            {
                return;
            }
            if (speechRecognition.StartRec(audioSource, recognitionHandler) < pxcmStatus.PXCM_STATUS_NO_ERROR)
            {
                throw new RealSenseInitializationException("The speech recognition could not be started");
            }
            recognitionStarted = true;
        }

        public void StopRecognition()
        {
            if (!recognitionStarted)
            {
                return;
            }

            speechRecognition.StopRec();
            recognitionStarted = false;
        }

        public void RestartRecognition()
        {
            StopRecognition();
            StopRecognition();
        }

        public void Stop()
        {
            if (speechRecognition != null)
            {
                speechRecognition.StopRec();
                speechRecognition.Dispose();
                speechRecognition = null;
            }
            if (audioSource != null)
            {
                audioSource.Dispose();
                audioSource = null;
            }
        }

        private void OnRecognition(PXCMSpeechRecognition.RecognitionData data)
        {
            if (data.scores[0].label < 0)
            {
                var sentence = data.scores[0].sentence;
                InvokeSpeech(sentence);
            }
        }

        private void InvokeSpeech(string sentence)
        {
            if (Speech != null)
            {
                var speechEventArgs = factory.Events.SpeechEvent()
                    .WithSentence(sentence).Build();
                Speech.Invoke(speechEventArgs);
            }
        }

        public bool RecognitionStarted
        {
            get { return recognitionStarted; }
        }

        public SpeechRecognitionDeterminerComponent WithSpeechListener(SpeechEventListener speechRecognitionCallback)
        {
            Speech += speechRecognitionCallback;
            return this;
        }

        public class Builder
        {
            private readonly GrammarBuilder grammarBuilder;
            private RealSenseFactory factory;
            private NativeSense nativeSense;
            private RealSensePropertiesManager propertiesManager;
            private RealSenseConfiguration configuration;

            public Builder(GrammarBuilder grammarBuilder)
            {
                this.grammarBuilder = grammarBuilder;
            }

            public Builder WithFactory(RealSenseFactory factory)
            {
                this.factory = factory;
                return this;
            }

            public Builder WithNativeSense(NativeSense nativeSense)
            {
                this.nativeSense = nativeSense;
                return this;
            }

            public Builder WithPropertiesManager(RealSensePropertiesManager propertiesManager)
            {
                this.propertiesManager = propertiesManager;
                return this;
            }

            public Builder WithConfiguration(RealSenseConfiguration configuration)
            {
                this.configuration = configuration;
                return this;
            }

            public SpeechRecognitionDeterminerComponent Build()
            {
                factory.Check(Preconditions.IsNotNull,
                    "The factory must be set in order to create the speech recognition determiner component");
                nativeSense.Check(Preconditions.IsNotNull,
                    "The native RealSense must be set in order to create the speech recognition determiner component");
                propertiesManager.Check(Preconditions.IsNotNull,
                    "The properties manager must be set to create the speech recognition determiner component");
                configuration.Check(Preconditions.IsNotNull,
                    "The RealSense configuration must be set in order to create the speech recognition determiner component");

                return new SpeechRecognitionDeterminerComponent(grammarBuilder, factory, nativeSense, propertiesManager, configuration);
            }
        }
    }
}