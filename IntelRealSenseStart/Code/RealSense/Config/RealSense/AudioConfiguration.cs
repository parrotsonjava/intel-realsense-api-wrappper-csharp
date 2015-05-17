using System;
using IntelRealSenseStart.Code.RealSense.Data.Properties;

namespace IntelRealSenseStart.Code.RealSense.Config.RealSense
{
    public class AudioConfiguration
    {
        public static readonly AudioConfiguration DEFAULT_CONFIGURATION;

        private Func<AudioDeviceProperties, bool> deviceSelectorFunction;
        private Func<AudioModuleProfileProperties, bool> profileSelectorFunction;

        static AudioConfiguration()
        {
            DEFAULT_CONFIGURATION = new Builder().WithDefaultConfiguration().Build();
        }

        private AudioConfiguration()
        {
        }

        public Func<AudioDeviceProperties, bool> DeviceSelectorFunction
        {
            get { return deviceSelectorFunction; }
        }

        public Func<AudioModuleProfileProperties, bool> ProfileSelectorFunction
        {
            get { return profileSelectorFunction; }
        }

        public class Builder
        {
            private readonly AudioConfiguration configuration;

            public Builder()
            {
                configuration = new AudioConfiguration();
                WithDefaultConfiguration();
            }

            public Builder WithDefaultConfiguration()
            {
                configuration.deviceSelectorFunction = device => true;
                configuration.profileSelectorFunction = profile => true;
                return this;
            }

            public Builder UsingAudioDevice(Func<AudioDeviceProperties, bool> selectorFunction)
            {
                configuration.deviceSelectorFunction = selectorFunction;
                return this;
            }

            public Builder UsingAudioModule(Func<AudioModuleProfileProperties, bool> selectorFunction)
            {
                configuration.profileSelectorFunction = selectorFunction;
                return this;
            }

            public AudioConfiguration Build()
            {
                return configuration;
            }
        }
    }
}