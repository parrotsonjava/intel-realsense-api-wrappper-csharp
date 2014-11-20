namespace IntelRealSenseStart.Code.RealSense.Config
{
    public class ConfigurationFactory
    {
        public Configuration.Builder Configuration()
        {
            return new Configuration.Builder();
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