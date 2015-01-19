namespace IntelRealSenseStart.Code.RealSense.Factory.Data
{
    public class DataFactory
    {
        private readonly CommonDataFactory commonDataFactory;
        private readonly DeterminerDataFactory determinerDataFactory;
        private readonly PropertiesDataFactory propertiesDataFactory;
        private readonly EventDataFactory eventDataFactory;

        public DataFactory()
        {
            commonDataFactory = new CommonDataFactory();
            determinerDataFactory = new DeterminerDataFactory();
            propertiesDataFactory = new PropertiesDataFactory();
            eventDataFactory = new EventDataFactory();
        }

        public CommonDataFactory Common
        {
            get { return commonDataFactory; }
        }

        public DeterminerDataFactory Determiner
        {
            get { return determinerDataFactory; }
        }

        public PropertiesDataFactory Properties
        {
            get { return propertiesDataFactory;  }
        }

        public EventDataFactory Events
        {
            get { return eventDataFactory; }
        }
    }
}