using System.Drawing;
using IntelRealSenseStart.Code.RealSense.Data.Determiner;

namespace IntelRealSenseStart.Code.RealSense.Helper
{
    public static class ProjectionExtensions
    {
        public static PXCMPoint3DF32 MapToColorPositionIn(this PXCMPoint3DF32 depthPosition, Bitmap bitmap,
            ImageData imageData)
        {
            Size backgroundImageResolution = bitmap.Size;
            return new PXCMPoint3DF32
            {
                x = imageData.UVMap[
                    (int) depthPosition.y*imageData.DepthImage.info.width +
                    (int) depthPosition.x
                    ].x*backgroundImageResolution.Width,
                y = imageData.UVMap[
                    (int) depthPosition.y*imageData.DepthImage.info.width +
                    (int) depthPosition.x
                    ].y*backgroundImageResolution.Height
            };
        }

        public static PXCMPoint3DF32 MapToDepthPositionIn(this PXCMPoint3DF32 depthPosition, Bitmap bitmap, ImageData imageData)
        {
            var backgroundImageResolution = bitmap.Size;
            var depthResolution = new Size(imageData.DepthImage.info.width, imageData.DepthImage.info.height);

            if (depthResolution.Equals(backgroundImageResolution))
            {
                return depthPosition;
            }

            return new PXCMPoint3DF32
            {
                x = (depthPosition.x / depthResolution.Width) * backgroundImageResolution.Width,
                y = (depthPosition.y / depthResolution.Height) * backgroundImageResolution.Height
            };
        }
    }
}