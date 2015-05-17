using IntelRealSenseStart.Code.RealSense.Component.Property;
using IntelRealSenseStart.Code.RealSense.Data.Properties;
using IntelRealSenseStart.Code.RealSense.Factory;
using IntelRealSenseStart.Code.RealSense.Helper;
using IntelRealSenseStart.Code.RealSense.Provider;

namespace IntelRealSenseStart.Code.RealSense.Manager
{
    public class RealSensePropertiesManager
    {
        private readonly PropertiesComponent[] components;

        private readonly RealSenseFactory factory;
        private readonly NativeSense nativeSense;

        private RealSensePropertiesManager(RealSenseFactory factory, NativeSense nativeSense)
        {
            this.factory = factory;
            this.nativeSense = nativeSense;

            components = GetComponents();
        }

        private PropertiesComponent[] GetComponents()
        {
            var audioDeviceComponent = factory.Components.Properties.AudioDevice()
                .WithFactory(factory).WithNativeSense(nativeSense).Build();
            var videoDeviceComponent = factory.Components.Properties.VideoDevice()
                .WithFactory(factory).WithNativeSense(nativeSense).Build();

            return new PropertiesComponent[] { videoDeviceComponent, audioDeviceComponent };
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
            private NativeSense nativeSense;
           
            public Builder WithFactory(RealSenseFactory factory)
            {
                this.factory = factory;
                return this;
            }

            public Builder WithNativeSense(NativeSense nativeSense)
            {
                this.nativeSense = nativeSense;
                return this;
            }

            public RealSensePropertiesManager Build()
            {
                return new RealSensePropertiesManager(factory, nativeSense);
            }
        }
    }
}
