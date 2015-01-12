using System.Drawing;
using IntelRealSenseStart.Code.RealSense.Config.Image;
using IntelRealSenseStart.Code.RealSense.Config.RealSense;
using IntelRealSenseStart.Code.RealSense.Data.Determiner;
using IntelRealSenseStart.Code.RealSense.Exception;
using IntelRealSenseStart.Code.RealSense.Factory;
using IntelRealSenseStart.Code.RealSense.Helper;

namespace IntelRealSenseStart.Code.RealSense.Component.Creator
{
    public class ImageBuilder
    {
        private readonly ImageCreatorConfiguration.Builder imageConfigurationBuilder;
        private OverallImageCreator _overallImageCreator;

        private RealSenseConfiguration realSenseConfiguration;
        private DeterminerData determinerData;

        private ImageBuilder(RealSenseFactory factory)
        {
            imageConfigurationBuilder = factory.Configuration.ImageCreator();
        }

        public ImageBuilder WithResolution(Size size)
        {
            imageConfigurationBuilder.WithResolution(size);
            return this;
        }

        public ImageBuilder WithBackgroundImage(ImageBackground backgroundImage)
        {
            if (backgroundImage == ImageBackground.ColorImage && !realSenseConfiguration.Image.ColorEnabled)
            {
                throw new RealSenseException("Cannot use background image since it is not configured");
            }

            imageConfigurationBuilder.WithBackgroundImage(backgroundImage);
            return this;
        }

        public ImageBuilder WithOverlay(ImageOverlay overlay)
        {
            switch (overlay)
            {
                case ImageOverlay.ColorCoordinateHandJoints:
                    AddColorCoordinateHandJoints();
                    break;
                case ImageOverlay.DepthCoordinateHandJoints:
                    AddDepthCoordinateHandJoints();
                    break;
                case ImageOverlay.ColorCoordinateFaceLandmarks:
                    AddColorCoordinateFaceLandmarks();
                    break;
                case ImageOverlay.DepthCoordinateHandsSegmentationImage:
                    AddDepthCoordinateHandsSegmentationImage();
                    break;
            }
            return this;
        }

        private void AddColorCoordinateHandJoints()
        {
            if (!(realSenseConfiguration.HandsDetectionEnabled && realSenseConfiguration.Image.ProjectionEnabled))
            {
                throw new RealSenseException(
                    "Cannot use projected hand joints since hands detection or image projection is not configured");
            }

            imageConfigurationBuilder.WithOverlay(ImageOverlay.ColorCoordinateHandJoints);
        }

        private void AddDepthCoordinateHandJoints()
        {
            if (!realSenseConfiguration.HandsDetectionEnabled)
            {
                throw new RealSenseException("Cannot use hand joints since hands detection is not configured");
            }
            imageConfigurationBuilder.WithOverlay(ImageOverlay.DepthCoordinateHandJoints);
        }

        private void AddColorCoordinateFaceLandmarks()
        {
            if (!(realSenseConfiguration.FaceDetectionEnabled && realSenseConfiguration.FaceDetection.UseLandmarks
                  && realSenseConfiguration.Image.ProjectionEnabled))
            {
                throw new RealSenseException(
                    "Cannot use projected face landmarks since landmark detection or image projection is not configured");
            }

            imageConfigurationBuilder.WithOverlay(ImageOverlay.ColorCoordinateFaceLandmarks);
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
            var imageConfiguration = imageConfigurationBuilder.Build();
            CheckConfiguration(imageConfiguration);

            return _overallImageCreator.Create(determinerData, realSenseConfiguration, imageConfiguration);
        }

        // ReSharper disable once UnusedParameter.Local
        private void CheckConfiguration(ImageCreatorConfiguration configuration)
        {
            if (configuration.Overlays.Contains(ImageOverlay.DepthCoordinateHandsSegmentationImage) &&
                !realSenseConfiguration.Image.ColorStreamConfiguration.Resolution.Equals(
                    realSenseConfiguration.Image.DepthStreamConfiguration.Resolution))
            {
                throw new RealSenseException(
                    "The hand segmentation image can only be rendered when color and depth resolution are the same");
            }
        }

        public class Builder
        {
            private readonly ImageBuilder imageBuilder;

            public Builder(RealSenseFactory factory)
            {
                imageBuilder = new ImageBuilder(factory);
            }

            public Builder WithImageCreator(OverallImageCreator _overallImageCreator)
            {
                imageBuilder._overallImageCreator = _overallImageCreator;
                return this;
            }

            public Builder WithConfiguration(RealSenseConfiguration realSenseConfiguration)
            {
                imageBuilder.realSenseConfiguration = realSenseConfiguration;
                return this;
            }

            public Builder WithDeterminerData(DeterminerData determinerData)
            {
                imageBuilder.determinerData = determinerData;
                return this;
            }

            public ImageBuilder Build()
            {
                imageBuilder.realSenseConfiguration.Check(Preconditions.IsNotNull,
                    "The RealSense configuration must be set in order to create the hands image builder");

                return imageBuilder;
            }
        }
    }
}