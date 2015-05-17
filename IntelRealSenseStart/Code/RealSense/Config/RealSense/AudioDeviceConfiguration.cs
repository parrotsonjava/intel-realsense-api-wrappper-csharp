using System;
using IntelRealSenseStart.Code.RealSense.Data.Properties;

namespace IntelRealSenseStart.Code.RealSense.Config.RealSense
{
    public class AudioDeviceConfiguration
    {
        public static readonly AudioDeviceConfiguration DEFAULT_CONFIGURATION;

        private Func<AudioDeviceProperties, bool> selectorFunction;

        static AudioDeviceConfiguration()
        {
            DEFAULT_CONFIGURATION = new Builder().WithDefaultConfiguration().Build();
        }

        private AudioDeviceConfiguration()
        {
        }

        private Func<AudioDeviceProperties, bool> SelectorFunction
        {
            get { return selectorFunction; }
        } 

        public class Builder
        {
            private readonly AudioDeviceConfiguration configuration;

            public Builder()
            {
                configuration = new AudioDeviceConfiguration();
            }

            public Builder WithDefaultConfiguration()
            {
                return this;
            }

            public Builder UsingAudioDevice(Func<AudioDeviceProperties, bool> selectorFunction)
            {
                configuration.selectorFunction = selectorFunction;
                return this;
            }

            public AudioDeviceConfiguration Build()
            {
                return configuration;
            }
        }
    }
}