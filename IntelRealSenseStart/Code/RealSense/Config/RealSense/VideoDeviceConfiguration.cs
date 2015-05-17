using System;
using IntelRealSenseStart.Code.RealSense.Data.Properties;

namespace IntelRealSenseStart.Code.RealSense.Config.RealSense
{
    public class VideoDeviceConfiguration
    {
        public static readonly VideoDeviceConfiguration DEFAULT_CONFIGURATION;

        private Func<VideoDeviceProperties, bool> selectorFunction;

        static VideoDeviceConfiguration()
        {
            DEFAULT_CONFIGURATION = new Builder().WithDefaultConfiguration().Build();
        }

        private VideoDeviceConfiguration()
        {
        }

        public Func<VideoDeviceProperties, bool> SelectorFunction
        {
            get { return selectorFunction; }
        }

        public class Builder
        {
            private readonly VideoDeviceConfiguration videoDeviceConfiguration;

            public Builder()
            {
                videoDeviceConfiguration = new VideoDeviceConfiguration();
            }

            public Builder WithDefaultConfiguration()
            {
                return this;
            }

            public Builder WithVideoDeviceName(String deviceName)
            {
                videoDeviceConfiguration.selectorFunction =
                    videoDeviceProperties => videoDeviceProperties.DeviceName == deviceName;
                return this;
            }

            public Builder UsingVideoDevice(Func<VideoDeviceProperties, bool> selectorFunction)
            {
                videoDeviceConfiguration.selectorFunction = selectorFunction;
                return this;
            }

            public VideoDeviceConfiguration Build()
            {
                return videoDeviceConfiguration;
            }
        }
    }
}