using System;

namespace IntelRealSenseStart.Code.RealSense.Data.Properties
{
    public class SpeechRecognitionProfileProperties
    {
        private SpeechRecognitionModuleProperties module;
        private PXCMSpeechRecognition.ProfileInfo profile;
        private PXCMSpeechRecognition.LanguageType language;
        private String speaker;

        private SpeechRecognitionProfileProperties()
        {
        }

        public SpeechRecognitionModuleProperties Module
        {
            get { return module; }
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
            private readonly SpeechRecognitionProfileProperties speechRecognitionProfileProperties;

            public Builder()
            {
                speechRecognitionProfileProperties = new SpeechRecognitionProfileProperties();
            }

            public Builder WithModule(SpeechRecognitionModuleProperties module)
            {
                speechRecognitionProfileProperties.module = module;
                return this;
            }

            public Builder WithProfile(PXCMSpeechRecognition.ProfileInfo profile)
            {
                speechRecognitionProfileProperties.profile = profile;
                return this;
            }

            public Builder WithLanguage(PXCMSpeechRecognition.LanguageType language)
            {
                speechRecognitionProfileProperties.language = language;
                return this;
            }

            public Builder WithSpeaker(String speaker)
            {
                speechRecognitionProfileProperties.speaker = speaker;
                return this;
            }

            public SpeechRecognitionProfileProperties Build()
            {
                return speechRecognitionProfileProperties;
            }
        }
    }
}