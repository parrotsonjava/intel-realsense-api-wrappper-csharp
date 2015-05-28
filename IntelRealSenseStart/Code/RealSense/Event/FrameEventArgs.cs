using IntelRealSenseStart.Code.RealSense.Component.Creator;
using IntelRealSenseStart.Code.RealSense.Config.RealSense;
using IntelRealSenseStart.Code.RealSense.Data.Determiner;
using IntelRealSenseStart.Code.RealSense.Data.Event;
using IntelRealSenseStart.Code.RealSense.Factory;
using FacesData = IntelRealSenseStart.Code.RealSense.Data.Event.FacesData;
using HandsData = IntelRealSenseStart.Code.RealSense.Data.Event.HandsData;

namespace IntelRealSenseStart.Code.RealSense.Event
{
    public class FrameEventArgs
    {
        private ImageBuilder imageBuilder;

        private FacesBuilder facesLandmarksBuilder;
        private HandsBuilder handsJointsBuilder;

        private DeterminerData determinerData;

        public ImageBuilder CreateImage()
        {
            return imageBuilder;
        }

        public FacesData Faces
        {
            get { return facesLandmarksBuilder.GetFacesData(determinerData.FacesData.Faces); }
        }

        public HandsData Hands
        {
            get { return handsJointsBuilder.GetHandsData(determinerData.HandsData.Hands); }
        }

        public class Builder
        {
            private readonly FrameEventArgs frameEventArgs;

            private FacesBuilder facesLandmarksBuilder;
            private HandsBuilder handsJointsBuilder;
            
            private readonly ImageBuilder.Builder handsImageBuilderBuilder;
            private OverallImageCreator overallImageCreator;
            
            private RealSenseConfiguration realSenseConfiguration;

            public Builder(RealSenseFactory factory)
            {
                frameEventArgs = new FrameEventArgs();
                
                handsImageBuilderBuilder = factory.Components.Creator.HandsImageBuilder();
            }

            public Builder WithFacesLandmarksBuilder(FacesBuilder facesLandmarksBuilder)
            {
                this.facesLandmarksBuilder = facesLandmarksBuilder;
                return this;
            }

            public Builder WithHandsJointsBuilder(HandsBuilder handsJointsBuilder)
            {
                this.handsJointsBuilder = handsJointsBuilder;
                return this;
            }

            public Builder WithOverallImageCreator(OverallImageCreator overallImageCreator)
            {
                this.overallImageCreator = overallImageCreator;
                return this;
            }

            public Builder WithRealSenseConfiguration(RealSenseConfiguration realSenseConfiguration)
            {
                this.realSenseConfiguration = realSenseConfiguration;
                return this;
            }

            public Builder WithDeterminerData(DeterminerData.Builder determinerData)
            {
                frameEventArgs.determinerData = determinerData.Build();
                return this;
            }

            public FrameEventArgs Build()
            {
                frameEventArgs.imageBuilder = handsImageBuilderBuilder
                    .WithConfiguration(realSenseConfiguration)
                    .WithImageCreator(overallImageCreator)
                    .WithDeterminerData(frameEventArgs.determinerData)
                    .Build();
                frameEventArgs.facesLandmarksBuilder = facesLandmarksBuilder;
                frameEventArgs.handsJointsBuilder = handsJointsBuilder;

                return frameEventArgs;
            }
        }
    }
}