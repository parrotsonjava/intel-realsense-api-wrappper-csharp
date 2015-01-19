using IntelRealSenseStart.Code.RealSense.Component.Creator;
using IntelRealSenseStart.Code.RealSense.Config.RealSense;
using IntelRealSenseStart.Code.RealSense.Data.Determiner;
using IntelRealSenseStart.Code.RealSense.Data.Event;
using IntelRealSenseStart.Code.RealSense.Factory;

namespace IntelRealSenseStart.Code.RealSense.Event
{
    public class FrameEventArgs
    {
        private ImageBuilder imageBuilder;

        private FacesLandmarksBuilder facesLandmarksBuilder;

        private DeterminerData determinerData;

        public ImageBuilder CreateImage()
        {
            return imageBuilder;
        }

        public FacesLandmarksData FaceLandmarks
        {
            get { return facesLandmarksBuilder.GetLandmarkData(determinerData.FacesData.Faces); }
        }

        public class Builder
        {
            private readonly FrameEventArgs frameEventArgs;
            
            private readonly ImageBuilder.Builder handsImageBuilderBuilder;
            private OverallImageCreator overallImageCreator;
            private FacesLandmarksBuilder facesLandmarksBuilder;

            private RealSenseConfiguration realSenseConfiguration;

            public Builder(RealSenseFactory factory)
            {
                frameEventArgs = new FrameEventArgs();
                
                handsImageBuilderBuilder = factory.Components.Creator.HandsImageBuilder();
            }

            public Builder WithFacesLandmarksBuilder(FacesLandmarksBuilder facesLandmarksBuilder)
            {
                this.facesLandmarksBuilder = facesLandmarksBuilder;
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

                return frameEventArgs;
            }
        }
    }
}