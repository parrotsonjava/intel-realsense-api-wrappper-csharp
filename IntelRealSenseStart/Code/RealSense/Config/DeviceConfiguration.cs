namespace IntelRealSenseStart.Code.RealSense.Config
{
    public class DeviceConfiguration
    {
        private VideoDeviceConfiguration videoDeviceConfiguration;

        private DeviceConfiguration()
        {
            // TODO set default video device config 
        }

        public VideoDeviceConfiguration VideoDeviceConfiguration
        {
            get { return videoDeviceConfiguration; }
        }

        public class Builder
        {
            private readonly DeviceConfiguration deviceConfiguration;

            public Builder()
            {
                deviceConfiguration = new DeviceConfiguration();
            }

            public Builder WithVideoDeviceConfiguration(VideoDeviceConfiguration.Builder videoDeviceConfiguration)
            {
                deviceConfiguration.videoDeviceConfiguration = videoDeviceConfiguration.Build();
                return this;
            }

            public DeviceConfiguration Build()
            {
                return deviceConfiguration;
            }
        }
    }
}
