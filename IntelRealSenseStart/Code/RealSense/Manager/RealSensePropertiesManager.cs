using System.Collections.Generic;
using IntelRealSenseStart.Code.RealSense.Component.Property;
using IntelRealSenseStart.Code.RealSense.Data.Properties;
using IntelRealSenseStart.Code.RealSense.Factory;
using IntelRealSenseStart.Code.RealSense.Helper;
using IntelRealSenseStart.Code.RealSense.Provider;

namespace IntelRealSenseStart.Code.RealSense.Manager
{
    public class RealSensePropertiesManager
    {
        private readonly List<PropertiesComponent<RealSenseProperties.Builder>> components;

        private readonly RealSenseFactory factory;
        private readonly NativeSense nativeSense;

        private RealSensePropertiesManager(RealSenseFactory factory, NativeSense nativeSense)
        {
            this.factory = factory;
            this.nativeSense = nativeSense;

            components = GetComponents();
        }

        private List<PropertiesComponent<RealSenseProperties.Builder>> GetComponents()
        {
            var videoDevicePropertiesDeterminer = factory.Components.Properties.VideoDeviceDeterminer()
                .WithFactory(factory).WithNativeSense(nativeSense).Build();
            var videoPropertiesDeterminer = factory.Components.Properties.VideoDeterminer()
                .WithFactory(factory).WithVideoPropertiesComponent(videoDevicePropertiesDeterminer);

            var audioDevicePropertiesDeterminer = factory.Components.Properties.AudioDeviceDeterminer()
                .WithFactory(factory).WithNativeSense(nativeSense).Build();
            var speechSynthesisModuleDeterminer = factory.Components.Properties.SpeechSynthesisModuleDeterminer()
                .WithFactory(factory).WithNativeSense(nativeSense).Build();
            var speechRecognitionModuleDeterminer = factory.Components.Properties.SpeechRecognitionModuleDeterminer()
                .WithFactory(factory).WithNativeSense(nativeSense).Build();
            var audioPropertiesDeterminer = factory.Components.Properties.AudioDeterminer()
                .WithFactory(factory)
                .WithAudioPropertiesComponent(audioDevicePropertiesDeterminer)
                .WithAudioPropertiesComponent(speechRecognitionModuleDeterminer)
                .WithAudioPropertiesComponent(speechSynthesisModuleDeterminer);

            return new List<PropertiesComponent<RealSenseProperties.Builder>>
            {
                videoPropertiesDeterminer.Build(), audioPropertiesDeterminer.Build()
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
                factory.Check(Preconditions.IsNotNull,
                    "The factory must be set in order to create the RealSense properties manager");
                nativeSense.Check(Preconditions.IsNotNull,
                    "The native set must be set in order to create the RealSense properties manager");

                return new RealSensePropertiesManager(factory, nativeSense);
            }
        }
    }
}