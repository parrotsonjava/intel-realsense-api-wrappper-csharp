using System.Drawing;
using IntelRealSenseStart.Code.RealSense.Config;
using IntelRealSenseStart.Code.RealSense.Config.HandsImage;
using IntelRealSenseStart.Code.RealSense.Data;
using IntelRealSenseStart.Code.RealSense.Exception;
using IntelRealSenseStart.Code.RealSense.Factory;

namespace IntelRealSenseStart.Code.RealSense.Component.Hands
{
    public class HandsImageBuilder
    {
        private readonly HandsData handsData;
        private readonly HandsImageConfiguration.Builder handsImageConfigurationBuilder;
        private readonly HandsImageCreator.Builder handsImageCreator;

        private readonly ImageData imageData;
        private readonly Configuration realSenseConfiguration;
        private readonly PXCMCapture.Device device;

        private HandsImageBuilder(RealSenseFactory factory, Configuration realSenseConfiguration, PXCMCapture.Device device, HandsData handsData, ImageData imageData)
        {
            handsImageConfigurationBuilder = factory.Events.HandsImageConfiguration();
            handsImageCreator = factory.Components.HandsImageCreator();

            this.realSenseConfiguration = realSenseConfiguration;
            this.device = device;
            this.handsData = handsData;
            this.imageData = imageData;
        }

        public HandsImageBuilder WithBackgroundImage(HandsImageBackground backgroundImage)
        {
            if (backgroundImage == HandsImageBackground.ColorImage && !realSenseConfiguration.ColorImageEnabled)
            {
                throw new RealSenseException("Cannot use background image since it is not configured");
            }

            handsImageConfigurationBuilder.WithBackgroundImage(backgroundImage);
            return this;
        }

        public HandsImageBuilder WithOverlay(HandsImageOverlay overlay)
        {
            if (overlay == HandsImageOverlay.HandsSegmentationImage &&
                (!realSenseConfiguration.HandsDetectionEnabled ||
                 !realSenseConfiguration.HandsDetection.SegmentationImageEnabled))
            {
                throw new RealSenseException("Cannot use hand segmentation image since it is not configured");
            }
            if (overlay == HandsImageOverlay.HandJoints && !realSenseConfiguration.HandsDetectionEnabled)
            {
                throw new RealSenseException("Cannot use hand joints since hands detection is not configured");
            }

            handsImageConfigurationBuilder.WithOverlay(overlay);
            return this;
        }

        public Bitmap Create()
        {
            return
                handsImageCreator.Build(device, handsData, imageData, realSenseConfiguration,
                    handsImageConfigurationBuilder.Build()).Create();
        }

        public class Builder
        {
            public HandsImageBuilder Build(RealSenseFactory factory, Configuration realSenseConfiguration, PXCMCapture.Device device, HandsData handsData, ImageData imageData)
            {
                return new HandsImageBuilder(factory, realSenseConfiguration, device, handsData, imageData);
            }
        }
    }
}