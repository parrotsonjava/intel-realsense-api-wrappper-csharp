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
        private readonly PXCMCapture.Device device;
        private readonly HandsData handsData;
        private readonly HandsImageConfiguration.Builder handsImageConfigurationBuilder;
        private readonly HandsImageCreator.Builder handsImageCreator;

        private readonly ImageData imageData;
        private readonly Configuration realSenseConfiguration;

        private HandsImageBuilder(RealSenseFactory factory, Configuration realSenseConfiguration,
            PXCMCapture.Device device, HandsData handsData, ImageData imageData)
        {
            handsImageConfigurationBuilder = factory.Events.HandsImageConfiguration();
            handsImageCreator = factory.Components.HandsImageCreator();

            this.realSenseConfiguration = realSenseConfiguration;
            this.device = device;
            this.handsData = handsData;
            this.imageData = imageData;
        }
        public HandsImageBuilder WithResolution(Size size)
        {
            handsImageConfigurationBuilder.WithResolution(size);
            return this;
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
            switch (overlay)
            {
                case HandsImageOverlay.ColorCoordinateHandJoints:
                    AddColorCoordinateHandJoints();
                    break;
                case HandsImageOverlay.DepthCoordinateHandJoints:
                    AddDepthCoordinateHandJoints();
                    break;
                case HandsImageOverlay.DepthCoordinateHandsSegmentationImage:
                    AddDepthCoordinateHandsSegmentationImage();
                    break;
            }
            return this;
        }

        private void AddColorCoordinateHandJoints()
        {
            if (!(realSenseConfiguration.HandsDetectionEnabled && realSenseConfiguration.ColorImageEnabled && realSenseConfiguration.DepthImageEnabled))
            {
                throw new RealSenseException("Cannot use projected hand joints since hands detection or color image or depth image is not configured");
            }

            handsImageConfigurationBuilder.WithOverlay(HandsImageOverlay.ColorCoordinateHandJoints);
        }

        private void AddDepthCoordinateHandJoints()
        {
            if (!realSenseConfiguration.HandsDetectionEnabled)
            {
                throw new RealSenseException("Cannot use hand joints since hands detection is not configured");
            }
            handsImageConfigurationBuilder.WithOverlay(HandsImageOverlay.DepthCoordinateHandJoints);
        }

        private void AddDepthCoordinateHandsSegmentationImage()
        {
            if (!realSenseConfiguration.HandsDetectionEnabled || !realSenseConfiguration.HandsDetection.SegmentationImageEnabled)
            {
                throw new RealSenseException("Cannot use hand segmentation image since it is not configured");
            }
        }
        
        public Bitmap Create()
        {
            var imageConfiguration = handsImageConfigurationBuilder.Build();
            CheckConfiguration(imageConfiguration);

            return handsImageCreator.Build(device, handsData, imageData, realSenseConfiguration, imageConfiguration).Create();
        }

        private void CheckConfiguration(HandsImageConfiguration configuration)
        {
            if (configuration.Overlays.Contains(HandsImageOverlay.DepthCoordinateHandsSegmentationImage) &&
                !realSenseConfiguration.ColorImage.Resolution.Equals(realSenseConfiguration.DepthImage.Resolution))
            {
                throw new RealSenseException("The hand segmentation image can only be rendered when color and depth resolution are the same");
            }
        }

        public class Builder
        {
            public HandsImageBuilder Build(RealSenseFactory factory, Configuration realSenseConfiguration,
                PXCMCapture.Device device, HandsData handsData, ImageData imageData)
            {
                return new HandsImageBuilder(factory, realSenseConfiguration, device, handsData, imageData);
            }
        }
    }
}