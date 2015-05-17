using System;

namespace IntelRealSenseStart.Code.RealSense.Data.Properties
{
    public class AudioModuleProfileProperties
    {
        private AudioModuleProperties audioModuleProperties;
        private PXCMSpeechRecognition.ProfileInfo profile;
        private PXCMSpeechRecognition.LanguageType language;
        private String speaker;

        private AudioModuleProfileProperties()
        {
        }

        public AudioModuleProperties Module
        {
            get { return audioModuleProperties; }
        }

        public PXCMSpeechRecognition.ProfileInfo Profile
        {
            get { return profile; }
        }

        public PXCMSpeechRecognition.LanguageType Language
        {
            get { return language; }
        }

        public String Speaker
        {
            get { return speaker; }
        }

        public class Builder
        {
            private readonly AudioModuleProfileProperties audioModuleProfileProperties;

            public Builder()
            {
                audioModuleProfileProperties = new AudioModuleProfileProperties();
            }

            public Builder WithModule(AudioModuleProperties audioModuleProperties)
            {
                audioModuleProfileProperties.audioModuleProperties = audioModuleProperties;
                return this;
            }

            public Builder WithProfile(PXCMSpeechRecognition.ProfileInfo profile)
            {
                audioModuleProfileProperties.profile = profile;
                return this;
            }

            public Builder WithLanguage(PXCMSpeechRecognition.LanguageType language)
            {
                audioModuleProfileProperties.language = language;
                return this;
            }

            public Builder WithSpeaker(String speaker)
            {
                audioModuleProfileProperties.speaker = speaker;
                return this;
            }

            public AudioModuleProfileProperties Build()
            {
                return audioModuleProfileProperties;
            }


        }
    }
}