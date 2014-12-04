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
    }
}