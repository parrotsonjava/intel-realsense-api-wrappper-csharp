using System.Collections.Generic;
using IntelRealSenseStart.Code.RealSense.Component.Property;
using IntelRealSenseStart.Code.RealSense.Data.Properties;
using IntelRealSenseStart.Code.RealSense.Factory;
using IntelRealSenseStart.Code.RealSense.Helper;
using IntelRealSenseStart.Code.RealSense.Manager.Builder;

namespace IntelRealSenseStart.Code.RealSense.Manager
{
    public class RealSensePropertiesManager
    {
        private readonly List<PropertiesComponent<RealSenseProperties.Builder>> components;

        private readonly RealSenseFactory factory;

        private RealSensePropertiesManager(RealSenseFactory factory, RealSensePropertyComponentsBuilder componentsBuilder)
        {
            this.factory = factory;

            components = GetComponents(componentsBuilder);
        }

        private List<PropertiesComponent<RealSenseProperties.Builder>> GetComponents(RealSensePropertyComponentsBuilder componentsBuilder)
        {
            var videoPropertiesDeterminer = componentsBuilder.CreateVideoPropertiesDeterminer();
            var audioPropertiesDeterminer = componentsBuilder.CreateAudioPropertiesDeterminer();

            return new List<PropertiesComponent<RealSenseProperties.Builder>>
            {
                videoPropertiesDeterminer, audioPropertiesDeterminer
            };
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
            private RealSensePropertyComponentsBuilder componentsBuilder;
           
            public Builder WithFactory(RealSenseFactory factory)
            {
                this.factory = factory;
                return this;
            }

            public Builder WithComponentsBuilder(RealSensePropertyComponentsBuilder componentsBuilder)
            {
                this.componentsBuilder = componentsBuilder;
                return this;
            }

            public RealSensePropertiesManager Build()
            {
                factory.Check(Preconditions.IsNotNull,
                    "The factory must be set in order to create the properties manager");
                componentsBuilder.Check(Preconditions.IsNotNull,
                    "The components builder must be set in order to create the properties manager");

                return new RealSensePropertiesManager(factory, componentsBuilder);
            }
        }
    }
}