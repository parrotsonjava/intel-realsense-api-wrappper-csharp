namespace IntelRealSenseStart.Code.RealSense.Config
{
    public class ConfigurationFactory
    {
        public Configuration.Builder Configuration()
        {
            return new Configuration.Builder();
        }

        public DeviceConfiguration.Builder DeviceConfiguration()
        {
            return new DeviceConfiguration.Builder();
        }

        public VideoDeviceConfiguration.Builder VideoDeviceConfiguration()
        {
            return new VideoDeviceConfiguration.Builder();
        }

        public HandsConfiguration.Builder HandsDetection()
        {
            return new HandsConfiguration.Builder();
        }

        public ImageConfiguration.Builder Image()
        {
            return new ImageConfiguration.Builder();
        }
    }
}