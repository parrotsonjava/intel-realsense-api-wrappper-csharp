using IntelRealSenseStart.Code.RealSense.Config.RealSense;

namespace IntelRealSenseStart.Code.RealSense.Factory.Configuration
{
    public class DeterminerConfigurationFactory
    {
        public Config.RealSense.Configuration.Builder Configuration()
        {
            return new Config.RealSense.Configuration.Builder();
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

        public FaceConfiguration.Builder FaceDetection()
        {
            return new FaceConfiguration.Builder();
        }

        public ImageConfiguration.Builder Image()
        {
            return new ImageConfiguration.Builder();
        }
    }
}
