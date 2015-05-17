using IntelRealSenseStart.Code.RealSense.Data.Properties;

namespace IntelRealSenseStart.Code.RealSense.Factory.Data
{
    public class PropertiesDataFactory
    {
        public RealSenseProperties.Builder RealSense()
        {
            return new RealSenseProperties.Builder();
        }

        public VideoDeviceProperties.Builder VideoDevice()
        {
            return new VideoDeviceProperties.Builder();
        }

        public AudioDeviceProperties.Builder AudioDevice()
        {
            return new AudioDeviceProperties.Builder();
        }

        public StreamProperties.Builder Stream()
        {
            return new StreamProperties.Builder();
        }
    }
}