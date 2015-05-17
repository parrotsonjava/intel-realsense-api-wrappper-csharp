using System;
using System.Collections.Generic;
using IntelRealSenseStart.Code.RealSense.Helper;

namespace IntelRealSenseStart.Code.RealSense.Data.Properties
{
    public class AudioModuleProperties
    {
        private String moduleName;
        private PXCMSession.ImplDesc module;
        private readonly List<AudioModuleProfileProperties> profiles; 

        private AudioModuleProperties()
        {
            profiles = new List<AudioModuleProfileProperties>();
        }

        public String ModuleName
        {
            get { return moduleName; }
        }

        public PXCMSession.ImplDesc Module
        {
            get { return module; }
        }

        public List<AudioModuleProfileProperties> Profiles
        {
            get { return profiles; }
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

            public Builder WithProfile(AudioModuleProfileProperties.Builder profile)
            {
                audioModuleProperties.profiles.Add(profile.Build());
                return this;
            }

            public Builder WithProfiles(List<AudioModuleProfileProperties.Builder> profiles)
            {
                profiles.Do(profile =>
                {
                    profile.WithModule(audioModuleProperties);
                    WithProfile(profile);
                });
                return this;
            }

            public AudioModuleProperties Build()
            {
                return audioModuleProperties;
            }
        }
    }
}