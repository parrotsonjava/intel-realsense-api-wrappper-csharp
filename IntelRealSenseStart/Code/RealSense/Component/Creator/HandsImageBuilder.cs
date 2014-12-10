using System.Drawing;
using IntelRealSenseStart.Code.RealSense.Config.HandsImage;
using IntelRealSenseStart.Code.RealSense.Config.RealSense;
using IntelRealSenseStart.Code.RealSense.Data;
using IntelRealSenseStart.Code.RealSense.Data.Determiner;
using IntelRealSenseStart.Code.RealSense.Exception;
using IntelRealSenseStart.Code.RealSense.Factory;
using IntelRealSenseStart.Code.RealSense.Helper;

namespace IntelRealSenseStart.Code.RealSense.Component.Creator
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
            handsImageCreator = factory.Components.Creator.HandsImageCreator();

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
            if (
                !(realSenseConfiguration.HandsDetectionEnabled && realSenseConfiguration.ColorImageEnabled &&
                  realSenseConfiguration.DepthImageEnabled))
            {
                throw new RealSenseException(
                    "Cannot use projected hand joints since hands detection or color image or depth image is not configured");
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
            if (!realSenseConfiguration.HandsDetectionEnabled ||
                !realSenseConfiguration.HandsDetection.SegmentationImageEnabled)
            {
                throw new RealSenseException("Cannot use hand segmentation image since it is not configured");
            }
        }

        public Bitmap Create()
        {
            var imageConfiguration = handsImageConfigurationBuilder.Build();
            CheckConfiguration(imageConfiguration);

            return handsImageCreator
                .WithDevice(device)
                .WithHandsData(handsData)
                .WithImageData(imageData)
                .WithRealSenseConfiguration(realSenseConfiguration)
                .WithImageConfiguration(imageConfiguration)
                .Build()
                .Create();
        }

        private void CheckConfiguration(HandsImageConfiguration configuration)
        {
            if (configuration.Overlays.Contains(HandsImageOverlay.DepthCoordinateHandsSegmentationImage) &&
                !realSenseConfiguration.ColorImage.Resolution.Equals(realSenseConfiguration.DepthImage.Resolution))
            {
                throw new RealSenseException(
                    "The hand segmentation image can only be rendered when color and depth resolution are the same");
            }
        }

        public class Builder
        {
            private RealSenseFactory factory;
            private Configuration realSenseConfiguration;
            private PXCMCapture.Device device;
            private HandsData handsData;
            private ImageData imageData;

            public Builder WithFactory(RealSenseFactory factory)
            {
                this.factory = factory;
                return this;
            }

            public Builder WithConfiguration(Configuration realSenseConfiguration)
            {
                this.realSenseConfiguration = realSenseConfiguration;
                return this;
            }

            public Builder WithDevice(PXCMCapture.Device device)
            {
                this.device = device;
                return this;
            }

            public Builder WithHandsData(HandsData handsData)
            {
                this.handsData = handsData;
                return this;
            }

            public Builder WithImageData(ImageData imageData)
            {
                this.imageData = imageData;
                return this;
            }

            public HandsImageBuilder Build()
            {
                factory.CheckState(Preconditions.IsNotNull,
                    "The RealSense factory must be set in order to create the hands image builder");
                realSenseConfiguration.CheckState(Preconditions.IsNotNull,
                    "The RealSense configuration must be set in order to create the hands image builder");

                return new HandsImageBuilder(factory, realSenseConfiguration, device, handsData, imageData);
            }
        }
    }
}