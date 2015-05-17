using System;
using System.Linq;
using IntelRealSenseStart.Code.RealSense.Config.RealSense;
using IntelRealSenseStart.Code.RealSense.Data.Properties;
using IntelRealSenseStart.Code.RealSense.Exception;
using IntelRealSenseStart.Code.RealSense.Factory;
using IntelRealSenseStart.Code.RealSense.Manager;
using IntelRealSenseStart.Code.RealSense.Native;
using IntelRealSenseStart.Code.RealSense.Provider;

namespace IntelRealSenseStart.Code.RealSense.Component.Output
{
    public class RealSenseSpeechSynthesisOutputComponent : OutputComponent
    {
        private readonly RealSenseFactory factory;
        private readonly NativeSense nativeSense;
        private readonly RealSensePropertiesManager propertiesManager;
        private readonly RealSenseConfiguration configuration;

        private PXCMSpeechSynthesis speechSynthesis;
        private PXCMSpeechSynthesis.ProfileInfo profile;

        private RealSenseSpeechSynthesisOutputComponent(RealSenseFactory factory, NativeSense nativeSense,
            RealSensePropertiesManager propertiesManager, RealSenseConfiguration configuration)
        {
            this.factory = factory;
            this.nativeSense = nativeSense;
            this.propertiesManager = propertiesManager;
            this.configuration = configuration;
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
        }

        public void Speak(String sentence)
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

        public void Stop()
        {
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

            public RealSenseSpeechSynthesisOutputComponent Build()
            {
                return new RealSenseSpeechSynthesisOutputComponent(factory, nativeSense, propertiesManager, configuration);
            }
        }
    }
}