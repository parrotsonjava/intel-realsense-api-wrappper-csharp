using System;
using IntelRealSenseStart.Code.RealSense.Factory;
using IntelRealSenseStart.Code.RealSense.Properties;

namespace IntelRealSenseStart.Code.RealSense.Manager
{
    public class RealSensePropertiesManager
    {
        private RealSenseFactory factory;
        private PXCMSession session;

        private RealSensePropertiesManager()
        {
        }

        public RealSenseProperties GetProperties()
        {
            // TODO implement
            return null;
        }

        public class Builder
        {
            private readonly RealSensePropertiesManager propertiesDeterminer;

            public Builder()
            {
                propertiesDeterminer = new RealSensePropertiesManager();    
            }
            
            public Builder WithFactory(RealSenseFactory factory)
            {
                propertiesDeterminer.factory = factory;
                return this;
            }

            public Builder WithSession(PXCMSession session)
            {
                propertiesDeterminer.session = session;
                return this;
            }

            public RealSensePropertiesManager Build()
            {
                return propertiesDeterminer;
            }
        }
    }
}
