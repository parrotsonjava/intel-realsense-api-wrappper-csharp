using IntelRealSenseStart.Code.RealSense.Component.Creator;
using IntelRealSenseStart.Code.RealSense.Component.Determiner;
using IntelRealSenseStart.Code.RealSense.Component.Determiner.Face;
using IntelRealSenseStart.Code.RealSense.Component.Output;
using IntelRealSenseStart.Code.RealSense.Config.RealSense;
using IntelRealSenseStart.Code.RealSense.Factory;
using IntelRealSenseStart.Code.RealSense.Provider;

namespace IntelRealSenseStart.Code.RealSense.Manager.Builder
{
    public class RealSenseComponentsBuilder
    {
        private RealSenseFactory factory;
        private NativeSense nativeSense;

        private RealSenseConfiguration configuration;

        private RealSensePropertiesManager propertiesManager;

        public VideoDeviceDeterminerComponent CreateDeviceDeterminerComponent()
        {
            return factory.Components.Determiner.VideoDevice()
                .WithPropertiesManager(propertiesManager)
                .WithNativeSense(nativeSense)
                .WithConfiguration(configuration)
                .Build();
        }

        public HandsDeterminerComponent CreateHandsDeterminerComponent()
        {
            return factory.Components.Determiner.Hands()
                .WithFactory(factory)
                .WithNativeSense(nativeSense)
                .WithConfiguration(configuration)
                .Build();
        }

        public FaceDeterminerComponent CreateFaceDeterminerComponent()
        {
            return factory.Components.Determiner.Face()
                .WithFaceComponent(CreateFaceLandmarksDeterminerComponent().Build())
                .WithFaceComponent(CreateFaceRecognitionDeterminerComponent().Build())
                .WithFaceComponent(CreatePulseDeterminerComponent().Build())
                .WithFaceComponent(CreateEmotionDeterminerComponet().Build())
                .WithFactory(factory)
                .WithNativeSense(nativeSense)
                .WithConfiguration(configuration)
                .Build();
        }

        private FaceLandmarksDeterminerComponent.Builder CreateFaceLandmarksDeterminerComponent()
        {
            return factory.Components.Determiner.FaceLandmarks()
                .WithConfiguration(configuration);
        }

        private FaceRecognitionDeterminerComponent.Builder CreateFaceRecognitionDeterminerComponent()
        {
            return factory.Components.Determiner.FaceRecognition()
                .WithConfiguration(configuration);
        }

        private PulseDeterminerComponent.Builder CreatePulseDeterminerComponent()
        {
            return factory.Components.Determiner.Pulse()
                .WithConfiguration(configuration);
        }

        private EmotionDeterminerComponent.Builder CreateEmotionDeterminerComponet()
        {
            return factory.Components.Determiner.Emotion()
                .WithNativeSense(nativeSense)
                .WithConfiguration(configuration);
        }

        public ImageDeterminerComponent CreatePictureDeterminerComponent()
        {
            return factory.Components.Determiner.Image()
                .WithFactory(factory)
                .WithNativeSense(nativeSense)
                .WithConfiguration(configuration)
                .Build();
        }

        public SpeechRecognitionDeterminerComponent CreateSpeechRecognitionDeterminerComponent()
        {
            return factory.Components.Determiner.SpeechRecognition()
                .WithFactory(factory)
                .WithNativeSense(nativeSense)
                .WithPropertiesManager(propertiesManager)
                .WithConfiguration(configuration)
                .Build();
        }

        public SpeechSynthesisOutputComponent CreateSpeechSynthesisOutputComponent()
        {
            return factory.Components.Output.SpeechSynthesis()
                .WithFactory(factory)
                .WithNativeSense(nativeSense)
                .WithPropertiesManager(propertiesManager)
                .WithConfiguration(configuration)
                .Build();
        }

        public FacesBuilder GetFacesBuilder()
        {
            return factory.Components.Creator.FacesLandmarksBuilder().Build();
        }

        public HandsBuilder getHandsJointsBuilder()
        {
            return factory.Components.Creator.HandsJointsBuilder().Build();
        }

        public BasicImageCreator CreateBasicImageCreatorComponent()
        {
            return factory.Components.Creator.BasicImageCreator()
                .WithRealSenseConfiguration(configuration)
                .Build();
        }

        public HandsImageCreator CreateHandsImageCreatorComponent()
        {
            return factory.Components.Creator.HandsImageCreator()
                .WithRealSenseConfiguration(configuration)
                .Build();
        }

        public FaceImageCreator CreateFaceImageCreatorComponent()
        {
            return factory.Components.Creator.FaceImageCreator()
                .WithRealSenseConfiguration(configuration)
                .Build();
        }

        public UserIdsImageCreator CreateUserIdsImageCreator()
        {
            return factory.Components.Creator.UserIdsImageCreator()
                .WithRealSenseConfiguration(configuration)
                .Build();
        }

        public EmotionsImageCreator CreateEmotionsImageCreator()
        {
            return factory.Components.Creator.EmotionsImageCreator()
                .WithFacesBuilder(GetFacesBuilder())
                .Build();
        }

        public OverallImageCreator CreateOverallImageCreator(ImageCreator[] imageCreators)
        {
            return factory.Components.Creator.OverallImageCreator()
                .WithImageCreators(imageCreators)
                .Build();
        }

        public class Builder
        {
            private readonly RealSenseComponentsBuilder componentsBuilder;

            public Builder()
            {
                componentsBuilder = new RealSenseComponentsBuilder();
            }

            public Builder WithFactory(RealSenseFactory factory)
            {
                componentsBuilder.factory = factory;
                return this;
            }

            public Builder WithPropertiesManager(RealSensePropertiesManager propertiesManager)
            {
                componentsBuilder.propertiesManager = propertiesManager;
                return this;
            }

            public Builder WithNativeSense(NativeSense nativeSense)
            {
                componentsBuilder.nativeSense = nativeSense;
                return this;
            }

            public Builder WithConfiguration(RealSenseConfiguration configuration)
            {
                componentsBuilder.configuration = configuration;
                return this;
            }

            public RealSenseComponentsBuilder Build()
            {
                return componentsBuilder;
            }
        }
    }
}