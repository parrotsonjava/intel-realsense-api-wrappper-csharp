using IntelRealSenseStart.Code.RealSense.Component.Property;
using IntelRealSenseStart.Code.RealSense.Config.RealSense;
using IntelRealSenseStart.Code.RealSense.Factory;
using IntelRealSenseStart.Code.RealSense.Provider;

namespace IntelRealSenseStart.Code.RealSense.Manager.Builder
{
    public class RealSensePropertyComponentsBuilder
    {
        private RealSenseFactory factory;
        private NativeSense nativeSense;

        public AudioPropertiesDeterminer CreateAudioPropertiesDeterminer()
        {
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
            return audioPropertiesDeterminer.Build();
        }

        public VideoPropertiesDeterminer CreateVideoPropertiesDeterminer()
        {
            var videoDevicePropertiesDeterminer = factory.Components.Properties.VideoDeviceDeterminer()
                .WithFactory(factory).WithNativeSense(nativeSense).Build();
            var videoPropertiesDeterminer = factory.Components.Properties.VideoDeterminer()
                .WithFactory(factory).WithVideoPropertiesComponent(videoDevicePropertiesDeterminer);
            return videoPropertiesDeterminer.Build();
        }

        public class Builder
        {
            private readonly RealSensePropertyComponentsBuilder componentsBuilder;

            public Builder()
            {
                componentsBuilder = new RealSensePropertyComponentsBuilder();
            }

            public Builder WithFactory(RealSenseFactory factory)
            {
                componentsBuilder.factory = factory;
                return this;
            }

            public Builder WithNativeSense(NativeSense nativeSense)
            {
                componentsBuilder.nativeSense = nativeSense;
                return this;
            }

            public RealSensePropertyComponentsBuilder Build()
            {
                return componentsBuilder;
            }
        }
    }
}