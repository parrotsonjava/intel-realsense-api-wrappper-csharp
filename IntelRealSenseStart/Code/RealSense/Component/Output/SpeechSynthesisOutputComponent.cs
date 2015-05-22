using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using IntelRealSenseStart.Code.RealSense.Config.RealSense;
using IntelRealSenseStart.Code.RealSense.Data.Properties;
using IntelRealSenseStart.Code.RealSense.Event;
using IntelRealSenseStart.Code.RealSense.Event.Data;
using IntelRealSenseStart.Code.RealSense.Exception;
using IntelRealSenseStart.Code.RealSense.Factory;
using IntelRealSenseStart.Code.RealSense.Helper;
using IntelRealSenseStart.Code.RealSense.Manager;
using IntelRealSenseStart.Code.RealSense.Native;
using IntelRealSenseStart.Code.RealSense.Provider;

namespace IntelRealSenseStart.Code.RealSense.Component.Output
{
    public class SpeechSynthesisOutputComponent : OutputComponent
    {
        public event SpeechOutputStatusListener Speech;

        private const int TIME_BETWEEN_SENTENCES = 300;
        private const int WAIT_TIMEOUT = 25;

        private readonly RealSenseFactory factory;
        private readonly NativeSense nativeSense;
        private readonly RealSensePropertiesManager propertiesManager;
        private readonly RealSenseConfiguration configuration;

        private PXCMSpeechSynthesis speechSynthesis;
        private PXCMSpeechSynthesis.ProfileInfo profile;

        private readonly Queue<String> sentencesToSpeak;
        private Thread speechOutputThread;

        private volatile bool stopped;

        private SpeechSynthesisOutputComponent(RealSenseFactory factory, NativeSense nativeSense, 
            RealSensePropertiesManager propertiesManager, RealSenseConfiguration configuration)
        {
            this.factory = factory;
            this.nativeSense = nativeSense;
            this.propertiesManager = propertiesManager;
            this.configuration = configuration;

            sentencesToSpeak = new Queue<String>();
        }

        public void EnableFeatures()
        {
        }

        public void Configure()
        {
            var audioProperties = propertiesManager.GetProperties().Audio;
            var profileProperties = GetSelectedProfile(audioProperties);

            CreateSpeechSynthesis();
            CreateProfile(profileProperties);
            SetProfile(profile);

            StartOutputThread();
        }

        private SpeechSynthesisProfileProperties GetSelectedProfile(AudioProperties audioProperties)
        {
            try
            {
                return audioProperties.SpeechSynthesisModules.SelectMany(module => module.Profiles)
                    .First(configuration.Base.Audio.SpeechSynthesisProfileSelectorFunction);
            }
            catch (InvalidOperationException)
            {
                throw new RealSenseInitializationException(
                    String.Format("No profile for the specified selector could be found"));
            }
        }

        private void CreateSpeechSynthesis()
        {
            if (nativeSense.Session.CreateImpl(out speechSynthesis) < pxcmStatus.PXCM_STATUS_NO_ERROR)
            {
                throw new RealSenseInitializationException("The speech synthesis could not be instantiated");
            }
        }

        private void SetProfile(PXCMSpeechSynthesis.ProfileInfo profile)
        {
            if (speechSynthesis.SetProfile(profile) < pxcmStatus.PXCM_STATUS_NO_ERROR)
            {
                throw new RealSenseInitializationException("The speech synthesis profile could not be set");
            }
        }

        private void CreateProfile(SpeechSynthesisProfileProperties profileProperties)
        {
            profile = profileProperties.Profile;
            profile.volume = configuration.SpeechSynthesis.Volume;
            profile.rate = configuration.SpeechSynthesis.SpeechRate;
            profile.pitch = configuration.SpeechSynthesis.Pitch;
        }

        private void StartOutputThread()
        {
            speechOutputThread = new Thread(OutputSpeech);
            speechOutputThread.Start();
        }

        private void OutputSpeech()
        {
            stopped = false;

            bool spokenBefore = false;
            while (!stopped)
            {
                bool spoken = SpeakNextSentence();
                InvokeSpeechEventBasedOn(spokenBefore, spoken);
                spokenBefore = spoken;
            }
        }
        
        private bool SpeakNextSentence()
        {
            if (sentencesToSpeak.Count > 0)
            {
                SpeakSentence(sentencesToSpeak.Dequeue());
                Thread.Sleep(TIME_BETWEEN_SENTENCES);
                return true;
            }

            Thread.Sleep(WAIT_TIMEOUT);
            return false;
        }

        private void SpeakSentence(String sentence)
        {
            if (speechSynthesis.BuildSentence(1, sentence) < pxcmStatus.PXCM_STATUS_NO_ERROR)
            {
                throw new RealSenseException(String.Format("Error building the sentence {0} for speech synthesis", sentence));
            }

            var voiceOut = new VoiceOut(profile.outputs);
            for (int i = 0; ; i++)
            {
                var sample = speechSynthesis.QueryBuffer(1, i);
                if (sample == null)
                {
                    break;
                }
                voiceOut.RenderAudio(sample);
            }
            voiceOut.Close();
        }

        private void InvokeSpeechEventBasedOn(bool spokenBefore, bool spoken)
        {
            if (spokenBefore && !spoken)
            {
                InvokeSpeechEvent(SpeechOutputStatus.ENDED_SPEAKING);
            }
            else if (!spokenBefore && spoken)
            {
                InvokeSpeechEvent(SpeechOutputStatus.STARTED_SPEAKING); 
            }
        }

        private void InvokeSpeechEvent(SpeechOutputStatus status)
        {
            if (Speech != null)
            {
                var eventArgs = factory.Events.SpeechOutputEvent()
                    .WithStatus(status)
                    .Build();
                Speech.Invoke(eventArgs);
            }
        }

        public void Stop()
        {
            if (!stopped)
            {
                stopped = true;
                speechOutputThread.Join();
                speechOutputThread = null;
            }
        }

        public void Speak(String sentence)
        {
            sentencesToSpeak.Enqueue(sentence);
        }

        public bool ShouldBeStarted
        {
            get { return configuration.SpeechSynthesisEnabled; }
        }

        public class Builder
        {
            private RealSenseFactory factory;
            private NativeSense nativeSense;
            private RealSensePropertiesManager propertiesManager;
            private RealSenseConfiguration configuration;

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

            public SpeechSynthesisOutputComponent Build()
            {
                factory.Check(Preconditions.IsNotNull,
                    "The factory must be set in order to create the speech synthesis output component");
                nativeSense.Check(Preconditions.IsNotNull,
                    "The native sense must be set in order to create the speech synthesis output component");
                propertiesManager.Check(Preconditions.IsNotNull,
                    "The properties manager must be set in order to create the hspeech synthesis output component");
                configuration.Check(Preconditions.IsNotNull,
                    "The RealSense configuration must be set in order to create the hspeech synthesis output component");

                return new SpeechSynthesisOutputComponent(factory, nativeSense, propertiesManager, configuration);
            }
        }
    }
}