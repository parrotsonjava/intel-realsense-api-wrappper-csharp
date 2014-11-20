using System.Drawing;
using IntelRealSenseStart.Code.RealSense.Config;
using IntelRealSenseStart.Code.RealSense.Config.HandsImage;
using IntelRealSenseStart.Code.RealSense.Data;
using IntelRealSenseStart.Code.RealSense.Factory;

namespace IntelRealSenseStart.Code.RealSense.Component.Hands
{
    public class HandsImageBuilder
    {
        private readonly HandsImageConfiguration.Builder handsImageConfigurationBuilder;
        private readonly HandsImageCreator.Builder handsImageCreator;

        private readonly Configuration realSenseConfiguration;
        private readonly HandsData handsData;
        private readonly ImageData imageData;

        private HandsImageBuilder(RealSenseFactory factory, Configuration realSenseConfiguration, HandsData handsData, ImageData imageData)
        {
            handsImageConfigurationBuilder = factory.Events.HandsImageConfiguration();
            handsImageCreator = factory.Components.HandsImageCreator();

            this.realSenseConfiguration = realSenseConfiguration;
            this.handsData = handsData;
            this.imageData = imageData;
        }

        public HandsImageBuilder WithBackgroundImage(HandsImageBackground backgroundImage)
        {
            handsImageConfigurationBuilder.WithBackgroundImage(backgroundImage);
            return this;
        }

        public HandsImageBuilder WithOverlay(HandsImageOverlay overlay)
        {
            handsImageConfigurationBuilder.WithOverlay(overlay);
            return this;
        }

        public Bitmap Create()
        {
            return handsImageCreator.Build(handsData, imageData, realSenseConfiguration, handsImageConfigurationBuilder.Build()).Create();
        }

        public class Builder
        {
            public HandsImageBuilder Build(RealSenseFactory factory, Configuration realSenseConfiguration, HandsData handsData, ImageData imageData)
            {
                return new HandsImageBuilder(factory, realSenseConfiguration, handsData, imageData);
            }
        }
    }
}