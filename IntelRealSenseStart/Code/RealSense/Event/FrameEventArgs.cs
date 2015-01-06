using IntelRealSenseStart.Code.RealSense.Component.Creator;
using IntelRealSenseStart.Code.RealSense.Config.RealSense;
using IntelRealSenseStart.Code.RealSense.Data.Determiner;
using IntelRealSenseStart.Code.RealSense.Factory;

namespace IntelRealSenseStart.Code.RealSense.Event
{
    public class FrameEventArgs
    {
        private ImageBuilder imageBuilder;

        private DeterminerData determinerData;

        public ImageBuilder CreateImage()
        {
            return imageBuilder;
        }

        public class Builder
        {
            private readonly FrameEventArgs frameEventArgs;
            
            private readonly ImageBuilder.Builder handsImageBuilderBuilder;
            private readonly OverallImageCreator overallImageCreator;

            private readonly RealSenseConfiguration realSenseConfiguration;

            public Builder(RealSenseFactory factory, OverallImageCreator overallImageCreator, RealSenseConfiguration realSenseConfiguration)
            {
                frameEventArgs = new FrameEventArgs();
                
                handsImageBuilderBuilder = factory.Components.Creator.HandsImageBuilder();
                this.overallImageCreator = overallImageCreator;

                this.realSenseConfiguration = realSenseConfiguration;
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
                return frameEventArgs;
            }
        }
    }
}