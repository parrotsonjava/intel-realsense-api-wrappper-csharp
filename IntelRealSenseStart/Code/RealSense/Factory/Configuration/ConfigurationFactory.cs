using IntelRealSenseStart.Code.RealSense.Config.Image;

namespace IntelRealSenseStart.Code.RealSense.Factory.Configuration
{
    public class ConfigurationFactory
    {
        private readonly DeterminerConfigurationFactory determinerConfigurationFactory;

        public ConfigurationFactory()
        {
            determinerConfigurationFactory = new DeterminerConfigurationFactory();
        }

        public DeterminerConfigurationFactory Determiner
        {
            get { return determinerConfigurationFactory;  }
        }

        public ImageCreatorConfiguration.Builder ImageCreator()
        {
            return new ImageCreatorConfiguration.Builder();
        }
    }
}