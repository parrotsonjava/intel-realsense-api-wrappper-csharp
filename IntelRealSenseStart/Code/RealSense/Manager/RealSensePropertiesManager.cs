using IntelRealSenseStart.Code.RealSense.Component.Property;
using IntelRealSenseStart.Code.RealSense.Data.Properties;
using IntelRealSenseStart.Code.RealSense.Factory;
using IntelRealSenseStart.Code.RealSense.Helper;

namespace IntelRealSenseStart.Code.RealSense.Manager
{
    public class RealSensePropertiesManager
    {
        private readonly PropertiesComponent[] components;

        private RealSenseFactory factory;
        private PXCMSession session;

        private RealSensePropertiesManager(RealSenseFactory factory, PXCMSession session)
        {
            this.factory = factory;
            this.session = session;

            components = GetComponents();
        }

        private PropertiesComponent[] GetComponents()
        {
            var deviceComponent = factory.Components.Properties.Device()
                .WithFactory(factory).WithSession(session).Build();

            return new PropertiesComponent[] { deviceComponent };
        }

        public RealSenseProperties GetProperties()
        {
            RealSenseProperties.Builder properties = factory.Data.Properties.RealSense();
            components.Do(component => component.UpdateProperties(properties));
            return properties.Build();
        }

        public class Builder
        {
            private RealSenseFactory factory;
            private PXCMSession session;
           
            public Builder WithFactory(RealSenseFactory factory)
            {
                this.factory = factory;
                return this;
            }

            public Builder WithSession(PXCMSession session)
            {
                this.session = session;
                return this;
            }

            public RealSensePropertiesManager Build()
            {
                return new RealSensePropertiesManager(factory, session);
            }
        }
    }
}
