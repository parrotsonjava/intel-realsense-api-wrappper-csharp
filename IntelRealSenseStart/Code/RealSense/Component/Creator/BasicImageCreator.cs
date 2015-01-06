using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using IntelRealSenseStart.Code.RealSense.Config.Image;
using IntelRealSenseStart.Code.RealSense.Config.RealSense;
using IntelRealSenseStart.Code.RealSense.Data.Determiner;
using IntelRealSenseStart.Code.RealSense.Exception;

namespace IntelRealSenseStart.Code.RealSense.Component.Creator
{
    public class BasicImageCreator : ImageCreator
    {
        // ReSharper disable once NotAccessedField.Local
        private readonly RealSenseConfiguration realSenseConfiguration;

        private BasicImageCreator(RealSenseConfiguration realSenseConfiguration)
        {
            this.realSenseConfiguration = realSenseConfiguration;
        }

        public Bitmap Create(Bitmap bitmap, DeterminerData determinerData, ImageCreatorConfiguration imageCreatorConfiguration)
        {
            SetBackground(bitmap, determinerData, imageCreatorConfiguration);
            return bitmap;
        }

        private void SetBackground(Bitmap bitmap, DeterminerData determinerData, ImageCreatorConfiguration imageConfiguration)
        {
            if (imageConfiguration.BackgroundImage == ImageBackground.ColorImage)
            {
                CopyImageDataToBitmap(bitmap, determinerData.ImageData.ColorImage);
            }
            else if (imageConfiguration.BackgroundImage == ImageBackground.DepthImage)
            {
                CopyImageDataToBitmap(bitmap, determinerData.ImageData.DepthImage);
            }
        }

        private void CopyImageDataToBitmap(Bitmap bitmap, PXCMImage sourceImage)
        {
            PXCMImage.ImageData sourceImageData;
            pxcmStatus status = sourceImage.AcquireAccess(PXCMImage.Access.ACCESS_READ,
                PXCMImage.PixelFormat.PIXEL_FORMAT_RGB32, out sourceImageData);

            if (status < pxcmStatus.PXCM_STATUS_NO_ERROR)
            {
                throw new RealSenseException("Error retrieving colorImage data");
            }

            int width = sourceImage.info.width;
            int height = sourceImage.info.height;
            byte[] pixels = sourceImageData.ToByteArray(0, sourceImageData.pitches[0] * height);

            sourceImage.ReleaseAccess(sourceImageData);
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly,
                PixelFormat.Format32bppRgb);

            Marshal.Copy(pixels, 0, data.Scan0, width * height * 4);
            bitmap.UnlockBits(data);
        }

        public class Builder
        {
            private RealSenseConfiguration realSenseConfiguration;

            public Builder WithRealSenseConfiguration(RealSenseConfiguration realSenseConfiguration)
            {
                this.realSenseConfiguration = realSenseConfiguration;
                return this;
            }

            public BasicImageCreator Build()
            {
                return new BasicImageCreator(realSenseConfiguration);
            }
        }
    }
}