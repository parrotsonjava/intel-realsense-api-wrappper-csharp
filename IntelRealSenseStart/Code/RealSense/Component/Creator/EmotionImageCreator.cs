using System;
using System.Drawing;
using System.Globalization;
using IntelRealSenseStart.Code.RealSense.Config.Image;
using IntelRealSenseStart.Code.RealSense.Config.RealSense;
using IntelRealSenseStart.Code.RealSense.Data.Determiner;

namespace IntelRealSenseStart.Code.RealSense.Component.Creator
{
    public class EmotionImageCreator : ImageCreator
    {
        private const int LEFT_LANDMARK_INDEX = 54;

        private readonly RealSenseConfiguration realSenseConfiguration;

        private EmotionImageCreator(RealSenseConfiguration realSenseConfiguration)
        {
            this.realSenseConfiguration = realSenseConfiguration;
        }

        public Bitmap Create(Bitmap bitmap, DeterminerData determinerData,
            ImageCreatorConfiguration imageCreatorConfiguration)
        {
            return new EmotionImageCreatorRun(bitmap, determinerData, realSenseConfiguration, imageCreatorConfiguration).Create();
        }

        public class EmotionImageCreatorRun
        {
            private readonly int sizeMeasure;

            private readonly Font font;
            private readonly SolidBrush brush;

            private readonly Bitmap bitmap;
            private readonly DeterminerData determinerData;

            private readonly RealSenseConfiguration realSenseConfiguration;
            private readonly ImageCreatorConfiguration imageCreatorConfiguration;

            public EmotionImageCreatorRun(Bitmap bitmap, DeterminerData determinerData,
                RealSenseConfiguration realSenseConfiguration,
                ImageCreatorConfiguration imageCreatorConfiguration)
            {
                this.bitmap = bitmap;
                this.determinerData = determinerData;

                this.realSenseConfiguration = realSenseConfiguration;
                this.imageCreatorConfiguration = imageCreatorConfiguration;

                sizeMeasure = 10 * (bitmap.Width / imageCreatorConfiguration.Resolution.Width);
                font = new Font("Arial", sizeMeasure);
                brush = new SolidBrush(Color.White);
            }

            public Bitmap Create()
            {
                OverlayBitmap();
                return bitmap;
            }

            private void OverlayBitmap()
            {
                if (imageCreatorConfiguration.Overlays.Contains(ImageOverlay.UserIds))
                {
                    OverlayBitmapWithUserIds();
                }
            }

            private void OverlayBitmapWithUserIds()
            {
                var graphics = Graphics.FromImage(bitmap);
                foreach (FaceDeterminerData face in determinerData.FacesData.Faces)
                {
                    if (face.LandmarkPoints != null &&
                        face.LandmarkPoints.Length > LEFT_LANDMARK_INDEX)
                    {
                        DrawCurrentEmotion(graphics, face);
                    }
                }
            }

            private void DrawCurrentEmotion(Graphics graphics, FaceDeterminerData face)
            {
                var centerPoint = face.LandmarkPoints[LEFT_LANDMARK_INDEX].image;

                var faceId = face.FaceId == -1 ? "N/A" : face.FaceId.ToString(CultureInfo.CurrentCulture);
                var faceIdText = String.Format("Face ID: {0}", faceId);

                graphics.DrawString(faceIdText, font, brush,
                    centerPoint.x - sizeMeasure, centerPoint.y);
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

            public EmotionImageCreator Build()
            {
                return new EmotionImageCreator(realSenseConfiguration);
            }
        }
    }
}