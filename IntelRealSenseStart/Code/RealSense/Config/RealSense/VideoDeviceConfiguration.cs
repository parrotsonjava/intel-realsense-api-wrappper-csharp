using IntelRealSenseStart.Code.RealSense.Data.Properties;

namespace IntelRealSenseStart.Code.RealSense.Config.RealSense
{
    public class VideoDeviceConfiguration
    {
        public static readonly VideoDeviceConfiguration DEFAULT_CONFIGURATION;

        private DeviceProperties deviceProperties;

        static VideoDeviceConfiguration()
        {
            DEFAULT_CONFIGURATION = new VideoDeviceConfiguration();
        }

        private VideoDeviceConfiguration()
        {
        }

        public DeviceProperties Device
        {
            get { return deviceProperties; }
        }

        public class Builder
        {
            private readonly VideoDeviceConfiguration videoDeviceConfiguration;

            public Builder()
            {
                videoDeviceConfiguration = new VideoDeviceConfiguration();
            }

            public Builder WithVideoDevice(DeviceProperties deviceProperties)
            {
                videoDeviceConfiguration.deviceProperties = deviceProperties;
                return this;
            }

            public VideoDeviceConfiguration Build()
            {
                return videoDeviceConfiguration;
            }
        }
    }
}