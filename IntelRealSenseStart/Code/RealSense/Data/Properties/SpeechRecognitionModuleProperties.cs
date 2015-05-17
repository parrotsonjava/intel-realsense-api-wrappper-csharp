using System;
using System.Collections.Generic;
using IntelRealSenseStart.Code.RealSense.Helper;

namespace IntelRealSenseStart.Code.RealSense.Data.Properties
{
    public class SpeechRecognitionModuleProperties
    {
        private String moduleName;
        private PXCMSession.ImplDesc module;
        private readonly List<SpeechRecognitionProfileProperties> profiles; 

        private SpeechRecognitionModuleProperties()
        {
            profiles = new List<SpeechRecognitionProfileProperties>();
        }

        public String ModuleName
        {
            get { return moduleName; }
        }

        public PXCMSession.ImplDesc Module
        {
            get { return module; }
        }

        public List<SpeechRecognitionProfileProperties> Profiles
        {
            get { return profiles; }
        } 

        public class Builder
        {
            private readonly SpeechRecognitionModuleProperties speechRecognitionModuleProperties;

            public Builder()
            {
                speechRecognitionModuleProperties = new SpeechRecognitionModuleProperties();
            }

            public Builder WithModuleName(String moduleName)
            {
                speechRecognitionModuleProperties.moduleName = moduleName;
                return this;
            }

            public Builder WithDeviceInfo(PXCMSession.ImplDesc module)
            {
                speechRecognitionModuleProperties.module = module;
                return this;
            }

            public Builder WithProfile(SpeechRecognitionProfileProperties.Builder profile)
            {
                speechRecognitionModuleProperties.profiles.Add(profile.Build());
                return this;
            }

            public Builder WithProfiles(List<SpeechRecognitionProfileProperties.Builder> profiles)
            {
                profiles.Do(profile =>
                {
                    profile.WithModule(speechRecognitionModuleProperties);
                    WithProfile(profile);
                });
                return this;
            }

            public SpeechRecognitionModuleProperties Build()
            {
                return speechRecognitionModuleProperties;
            }
        }
    }
}