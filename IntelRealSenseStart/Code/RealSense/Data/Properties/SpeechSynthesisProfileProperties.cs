namespace IntelRealSenseStart.Code.RealSense.Data.Properties
{
    public class SpeechSynthesisProfileProperties
    {
        private SpeechSynthesisModuleProperties module;
        private PXCMSpeechSynthesis.ProfileInfo profile;
        private PXCMSpeechSynthesis.LanguageType language;

        private SpeechSynthesisProfileProperties()
        {
        }

        public SpeechSynthesisModuleProperties Module
        {
            get { return module; }
        }

        public PXCMSpeechSynthesis.ProfileInfo Profile
        {
            get { return profile; }
        }

        public PXCMSpeechSynthesis.LanguageType Language
        {
            get { return language; }
        }

        public class Builder
        {
            private readonly SpeechSynthesisProfileProperties speechSynthesisProfileProperties;

            public Builder()
            {
                speechSynthesisProfileProperties = new SpeechSynthesisProfileProperties();
            }

            public Builder WithModule(SpeechSynthesisModuleProperties module)
            {
                speechSynthesisProfileProperties.module = module;
                return this;
            }

            public Builder WithProfile(PXCMSpeechSynthesis.ProfileInfo profile)
            {
                speechSynthesisProfileProperties.profile = profile;
                return this;
            }

            public Builder WithLanguage(PXCMSpeechSynthesis.LanguageType language)
            {
                speechSynthesisProfileProperties.language = language;
                return this;
            }

            public SpeechSynthesisProfileProperties Build()
            {
                return speechSynthesisProfileProperties;
            }
        }
    }
}