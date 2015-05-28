using System.Drawing;
using System.Linq;
using IntelRealSenseStart.Code.RealSense.Config.Image;
using IntelRealSenseStart.Code.RealSense.Config.RealSense;
using IntelRealSenseStart.Code.RealSense.Data.Determiner;
using IntelRealSenseStart.Code.RealSense.Helper;

namespace IntelRealSenseStart.Code.RealSense.Component.Creator
{
    public class FaceImageCreator : ImageCreator
    {
        private const int CONFIDENCE_THRESHOLD = 80;

        // ReSharper disable once NotAccessedField.Local
        private readonly RealSenseConfiguration realSenseConfiguration;

        private FaceImageCreator(RealSenseConfiguration realSenseConfiguration)
        {
            this.realSenseConfiguration = realSenseConfiguration;
        }

        public Bitmap Create(Bitmap bitmap, DeterminerData determinerData,
            ImageCreatorConfiguration imageCreatorConfiguration)
        {
            return new FaceImageCreatorRun(bitmap, determinerData, imageCreatorConfiguration).Create();
        }

        public class FaceImageCreatorRun
        {
            private readonly Pen landmarkPen;
            private readonly float landmarkSize;

            private readonly Bitmap bitmap;
            private readonly DeterminerData determinerData;

            private readonly ImageCreatorConfiguration imageCreatorConfiguration;

            public FaceImageCreatorRun(Bitmap bitmap, DeterminerData determinerData,
                ImageCreatorConfiguration imageCreatorConfiguration)
            {
                this.bitmap = bitmap;
                this.determinerData = determinerData;

                this.imageCreatorConfiguration = imageCreatorConfiguration;

                landmarkSize = 3.0f*bitmap.Width/imageCreatorConfiguration.Resolution.Width;
                landmarkPen = new Pen(Color.Red,
                    landmarkSize/2.0f*bitmap.Width/imageCreatorConfiguration.Resolution.Width);
            }

            public Bitmap Create()
            {
                OverlayBitmap();
                return bitmap;
            }

            private void OverlayBitmap()
            {
                if (imageCreatorConfiguration.Overlays.Contains(ImageOverlay.ColorCoordinateFaceLandmarks))
                {
                    OverlayBitmapWithFaceLandmarks();
                }
            }

            private void OverlayBitmapWithFaceLandmarks()
            {
                var graphics = Graphics.FromImage(bitmap);
                foreach (FaceDeterminerData face in determinerData.FacesData.Faces)
                {
                    if (face.LandmarkPoints != null)
                    {
                        AddFaceLandmarksFor(graphics, face);
                    }
                }
            }

            private void AddFaceLandmarksFor(Graphics graphics, FaceDeterminerData faceDeterminerData)
            {
                faceDeterminerData.LandmarkPoints
                    .Where(point => point.confidenceImage > CONFIDENCE_THRESHOLD)
                    .Do(point => DrawPoint(graphics, point.image));
            }

            private void DrawPoint(Graphics graphics, PXCMPointF32 point)
            {
                graphics.DrawEllipse(landmarkPen, point.x - landmarkSize, point.y - landmarkSize,
                    landmarkSize*2, landmarkSize*2);
            }
        }

        public class Builder
        {
            private RealSenseConfiguration realSenseConfiguration;

            public Builder WithRealSenseConfiguration(RealSenseConfiguration realSenseConfiguration)
            {
                this.realSenseConfiguration = realSenseConfiguration;
                return this;
            }

            public FaceImageCreator Build()
            {
                return new FaceImageCreator(realSenseConfiguration);
            }
        }
    }
}