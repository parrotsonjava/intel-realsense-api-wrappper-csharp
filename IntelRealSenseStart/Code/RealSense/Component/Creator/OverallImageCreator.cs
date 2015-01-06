using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using IntelRealSenseStart.Code.RealSense.Config.Image;
using IntelRealSenseStart.Code.RealSense.Config.RealSense;
using IntelRealSenseStart.Code.RealSense.Data.Determiner;
using IntelRealSenseStart.Code.RealSense.Helper;

namespace IntelRealSenseStart.Code.RealSense.Component.Creator
{
    public class OverallImageCreator
    {
        private readonly IEnumerable<ImageCreator> imageCreators;

        private OverallImageCreator(IEnumerable<ImageCreator> imageCreators)
        {
            this.imageCreators = imageCreators;
        }

        public Bitmap Create(DeterminerData determinerData, RealSenseConfiguration realSenseConfiguration,
            ImageCreatorConfiguration imageCreatorConfiguration)
        {
            Bitmap bitmap = CreateBitmap(determinerData, imageCreatorConfiguration);
            foreach (var imageCreator in imageCreators)
            {
                bitmap = imageCreator.Create(bitmap, determinerData, imageCreatorConfiguration);
            }
            return FinalizeBitmap(bitmap, imageCreatorConfiguration);
        }

        private Bitmap CreateBitmap(DeterminerData determinerData, ImageCreatorConfiguration imageCreatorConfiguration)
        {
            var backgroundImageSize = GetBackgroundImageResolution(determinerData, imageCreatorConfiguration);
            return new Bitmap(backgroundImageSize.Width, backgroundImageSize.Height, PixelFormat.Format32bppRgb);
        }

        private Size GetBackgroundImageResolution(DeterminerData determinerData,
            ImageCreatorConfiguration imageConfiguration)
        {
            var backgroundImage = GetBackgroundImage(determinerData, imageConfiguration);
            return backgroundImage != null
                ? new Size(backgroundImage.info.width, backgroundImage.info.height)
                : imageConfiguration.Resolution;
        }

        private PXCMImage GetBackgroundImage(DeterminerData determinerData, ImageCreatorConfiguration imageCreatorConfiguration)
        {
            switch (imageCreatorConfiguration.BackgroundImage)
            {
                case ImageBackground.ColorImage:
                    return determinerData.ImageData.ColorImage;
                case ImageBackground.DepthImage:
                    return determinerData.ImageData.DepthImage;
                default:
                    return null;
            }
        }

        private Bitmap FinalizeBitmap(Bitmap bitmap, ImageCreatorConfiguration imageCreatorConfiguration)
        {
            return (Bitmap) bitmap.ResizeImage(imageCreatorConfiguration.Resolution);
        }

        public class Builder
        {
            private IEnumerable<ImageCreator> imageCreators;

            public Builder WithImageCreators(IEnumerable<ImageCreator> imageCreators)
            {
                this.imageCreators = imageCreators;
                return this;
            }

            public OverallImageCreator Build()
            {
                return new OverallImageCreator(imageCreators);
            }
        }
    }
}