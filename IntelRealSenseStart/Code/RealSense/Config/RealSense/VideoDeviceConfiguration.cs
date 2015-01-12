using System;

namespace IntelRealSenseStart.Code.RealSense.Config.RealSense
{
    public class VideoDeviceConfiguration
    {
        public static readonly VideoDeviceConfiguration DEFAULT_CONFIGURATION;

        private String deviceName;

        static VideoDeviceConfiguration()
        {
            DEFAULT_CONFIGURATION = new VideoDeviceConfiguration();
        }

        private VideoDeviceConfiguration()
        {
        }

        public String DeviceName
        {
            get { return deviceName; }
        }

        public class Builder
        {
            private readonly VideoDeviceConfiguration videoDeviceConfiguration;

            public Builder()
            {
                videoDeviceConfiguration = new VideoDeviceConfiguration();
            }

            public Builder WithVideoDeviceName(String deviceName)
            {
                videoDeviceConfiguration.deviceName = deviceName;
                return this;
            }

            public VideoDeviceConfiguration Build()
            {
                return videoDeviceConfiguration;
            }
        }
    }
}