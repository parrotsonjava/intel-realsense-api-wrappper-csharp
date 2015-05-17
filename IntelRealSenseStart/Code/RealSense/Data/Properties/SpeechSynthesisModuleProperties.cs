using System;
using System.Collections.Generic;
using IntelRealSenseStart.Code.RealSense.Helper;

namespace IntelRealSenseStart.Code.RealSense.Data.Properties
{
    public class SpeechSynthesisModuleProperties
    {
        private String moduleName;
        private PXCMSession.ImplDesc module;
        private readonly List<SpeechSynthesisProfileProperties> profiles;

        private SpeechSynthesisModuleProperties()
        {
            profiles = new List<SpeechSynthesisProfileProperties>();
        }

        public String ModuleName
        {
            get { return moduleName; }
        }

        public PXCMSession.ImplDesc Module
        {
            get { return module; }
        }

        public List<SpeechSynthesisProfileProperties> Profiles
        {
            get { return profiles; }
        } 

        public class Builder
        {
            private readonly SpeechSynthesisModuleProperties speechSynthesisModuleProperties;

            public Builder()
            {
                speechSynthesisModuleProperties = new SpeechSynthesisModuleProperties();
            }

            public Builder WithModuleName(String moduleName)
            {
                speechSynthesisModuleProperties.moduleName = moduleName;
                return this;
            }

            public Builder WithDeviceInfo(PXCMSession.ImplDesc module)
            {
                speechSynthesisModuleProperties.module = module;
                return this;
            }

            public Builder WithProfile(SpeechSynthesisProfileProperties.Builder profile)
            {
                speechSynthesisModuleProperties.profiles.Add(profile.Build());
                return this;
            }

            public Builder WithProfiles(List<SpeechSynthesisProfileProperties.Builder> profiles)
            {
                profiles.Do(profile =>
                {
                    profile.WithModule(speechSynthesisModuleProperties);
                    WithProfile(profile);
                });
                return this;
            }

            public SpeechSynthesisModuleProperties Build()
            {
                return speechSynthesisModuleProperties;
            }
        }
    }
}