using System;
using System.Collections.Generic;
using IntelRealSenseStart.Code.RealSense.Helper;

namespace IntelRealSenseStart.Code.RealSense.Data.Properties
{
    public class AudioModuleProperties
    {
        private String moduleName;
        private PXCMSession.ImplDesc module;
        private readonly List<PXCMSpeechRecognition.LanguageType> supportedLanguages; 

        private AudioModuleProperties()
        {
            supportedLanguages = new List<PXCMSpeechRecognition.LanguageType>();
        }

        public String ModuleName
        {
            get { return moduleName; }
        }

        public PXCMSession.ImplDesc Module
        {
            get { return module; }
        }

        public List<PXCMSpeechRecognition.LanguageType> SupportedLanguages
        {
            get { return supportedLanguages; }
        } 

        public class Builder
        {
            private readonly AudioModuleProperties audioModuleProperties;

            public Builder()
            {
                audioModuleProperties = new AudioModuleProperties();
            }

            public Builder WithModuleName(String moduleName)
            {
                audioModuleProperties.moduleName = moduleName;
                return this;
            }

            public Builder WithDeviceInfo(PXCMSession.ImplDesc module)
            {
                audioModuleProperties.module = module;
                return this;
            }

            public Builder WithSupportedLanguage(PXCMSpeechRecognition.LanguageType language)
            {
                audioModuleProperties.supportedLanguages.Add(language);
                return this;
            }

            public Builder WithSupportedLanguages(List<PXCMSpeechRecognition.LanguageType> supportedLanguages)
            {
                supportedLanguages.Do(supportedLanguage => WithSupportedLanguage(supportedLanguage));
                return this;
            }

            public AudioModuleProperties Build()
            {
                return audioModuleProperties;
            }
        }
    }
}