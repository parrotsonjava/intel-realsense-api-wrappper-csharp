using IntelRealSenseStart.Code.RealSense.Factory.Component;
using IntelRealSenseStart.Code.RealSense.Factory.Configuration;
using IntelRealSenseStart.Code.RealSense.Factory.Data;

namespace IntelRealSenseStart.Code.RealSense.Factory
{
    public class RealSenseFactory
    {
        private readonly NativeFactory nativeFactory;
        private readonly ManagerFactory managerFactory;
        private readonly ComponentsFactory componentsFactory;
        private readonly ConfigurationFactory configurationFactory;
        private readonly DataFactory dataFactory;
        private readonly EventsFactory eventsFactory;

        public RealSenseFactory()
        {
            nativeFactory = new NativeFactory();
            managerFactory = new ManagerFactory();
            componentsFactory = new ComponentsFactory();
            configurationFactory = new ConfigurationFactory();
            dataFactory = new DataFactory();
            eventsFactory = new EventsFactory(this);
        }

        public NativeFactory Native
        {
            get { return nativeFactory; }
        }

        public ManagerFactory Manager
        {
            get { return managerFactory; }
        }

        public ComponentsFactory Components
        {
            get { return componentsFactory; }
        }

        public ConfigurationFactory Configuration
        {
            get { return configurationFactory; }
        }

        public DataFactory Data
        {
            get { return dataFactory; }
        }

        public EventsFactory Events
        {
            get { return eventsFactory; }
        }
    }
}