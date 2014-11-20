﻿using IntelRealSenseStart.Code.RealSense.Component.Event;
using IntelRealSenseStart.Code.RealSense.Config;
using IntelRealSenseStart.Code.RealSense.Data;

namespace IntelRealSenseStart.Code.RealSense.Factory
{
    public class RealSenseFactory
    {
        private readonly ConfigurationFactory configurationFactory;
        private readonly ComponentsFactory componentsFactory;
        private readonly DataFactory dataFactory;
        private readonly EventsFactory eventsFactory;

        public RealSenseFactory()
        {
            configurationFactory = new ConfigurationFactory();
            componentsFactory = new ComponentsFactory();
            dataFactory = new DataFactory();
            eventsFactory = new EventsFactory();
        }

        public ConfigurationFactory Configuration
        {
            get { return configurationFactory; }
        }

        public ComponentsFactory Components
        {
            get { return componentsFactory;  }
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