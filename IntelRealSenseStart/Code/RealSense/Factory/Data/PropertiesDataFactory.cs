using IntelRealSenseStart.Code.RealSense.Data.Properties;

namespace IntelRealSenseStart.Code.RealSense.Factory.Data
{
    public class PropertiesDataFactory
    {
        public RealSenseProperties.Builder RealSense()
        {
            return new RealSenseProperties.Builder();
        }

        public DeviceProperties.Builder Device()
        {
            return new DeviceProperties.Builder();
        }

        public StreamProperties.Builder Stream()
        {
            return new StreamProperties.Builder();
        }
    }
}