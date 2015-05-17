using System;
using IntelRealSenseStart.Code.RealSense.Data.Properties;

namespace IntelRealSenseStart.Code.RealSense.Config.RealSense
{
    public class VideoConfiguration
    {
        public static readonly VideoConfiguration DEFAULT_CONFIGURATION;

        private Func<VideoDeviceProperties, bool> selectorFunction;

        static VideoConfiguration()
        {
            DEFAULT_CONFIGURATION = new Builder().WithDefaultConfiguration().Build();
        }

        private VideoConfiguration()
        {
        }

        public Func<VideoDeviceProperties, bool> SelectorFunction
        {
            get { return selectorFunction; }
        }

        public class Builder
        {
            private readonly VideoConfiguration videoConfiguration;

            public Builder()
            {
                videoConfiguration = new VideoConfiguration();
            }

            public Builder WithDefaultConfiguration()
            {
                return this;
            }

            public Builder WithVideoDeviceName(String deviceName)
            {
                videoConfiguration.selectorFunction =
                    videoDeviceProperties => videoDeviceProperties.DeviceName == deviceName;
                return this;
            }

            public Builder UsingVideoDevice(Func<VideoDeviceProperties, bool> selectorFunction)
            {
                videoConfiguration.selectorFunction = selectorFunction;
                return this;
            }

            public VideoConfiguration Build()
            {
                return videoConfiguration;
            }
        }
    }
}