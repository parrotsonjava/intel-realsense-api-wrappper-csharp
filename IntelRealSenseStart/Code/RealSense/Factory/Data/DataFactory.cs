namespace IntelRealSenseStart.Code.RealSense.Factory.Data
{
    public class DataFactory
    {
        private readonly DeterminerDataFactory determinerDataFactory;
        private readonly PropertiesDataFactory propertiesDataFactory;

        public DataFactory()
        {
            determinerDataFactory = new DeterminerDataFactory();
            propertiesDataFactory = new PropertiesDataFactory();
        }

        public DeterminerDataFactory Determiner
        {
            get { return determinerDataFactory; }
        }

        public PropertiesDataFactory Properties
        {
            get { return propertiesDataFactory;  }
        }
    }
}