using IntelRealSenseStart.Code.RealSense.Data.Properties;

namespace IntelRealSenseStart.Code.RealSense.Factory.Data
{
    public class PropertiesDataFactory
    {
        public RealSenseProperties.Builder RealSense()
        {
            return new RealSenseProperties.Builder();
        }

        public VideoProperties.Builder Video()
        {
            return new VideoProperties.Builder();
        }

        public VideoDeviceProperties.Builder VideoDevice()
        {
            return new VideoDeviceProperties.Builder();
        }

        public AudioProperties.Builder Audio()
        {
            return new AudioProperties.Builder();
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