using System;
using System.Drawing;
using System.Globalization;
using IntelRealSenseStart.Code.RealSense.Config.Image;
using IntelRealSenseStart.Code.RealSense.Data.Determiner;
using IntelRealSenseStart.Code.RealSense.Data.Event;

namespace IntelRealSenseStart.Code.RealSense.Component.Creator
{
    public class EmotionsImageCreator : ImageCreator
    {
        private readonly FacesBuilder facesBuilder;

        private EmotionsImageCreator(FacesBuilder facesBuilder)
        {
            this.facesBuilder = facesBuilder;
        }

        public Bitmap Create(Bitmap bitmap, DeterminerData determinerData,
            ImageCreatorConfiguration imageCreatorConfiguration)
        {
            return new EmotionsImageCreatorRun(facesBuilder, bitmap, determinerData, imageCreatorConfiguration).Create();
        }

        public class EmotionsImageCreatorRun
        {
            private readonly int sizeMeasure;

            private readonly Font font;
            private readonly SolidBrush brush;

            private readonly FacesBuilder facesBuilder;

            private readonly Bitmap bitmap;
            private readonly DeterminerData determinerData;

            private readonly ImageCreatorConfiguration imageCreatorConfiguration;

            public EmotionsImageCreatorRun(FacesBuilder facesBuilder,
                Bitmap bitmap, DeterminerData determinerData,
                ImageCreatorConfiguration imageCreatorConfiguration)
            {
                this.facesBuilder = facesBuilder;

                this.bitmap = bitmap;
                this.determinerData = determinerData;

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
                if (imageCreatorConfiguration.Overlays.Contains(ImageOverlay.Emotions))
                {
                    OverlayBitmapWithEmotions();
                }
            }

            private void OverlayBitmapWithEmotions()
            {
                var faces = facesBuilder.GetFacesData(determinerData.FacesData.Faces);

                var graphics = Graphics.FromImage(bitmap);
                foreach (var face in faces.Faces)
                {
                    if (face.GetPoint(FaceLandmark.LEFT_CONTOUR_MID_UP) != null)
                    {
                        DrawCurrentEmotion(graphics, face);
                    }
                }
            }

            private void DrawCurrentEmotion(Graphics graphics, FaceData face)
            {
                var centerPoint = face.GetPoint(FaceLandmark.LEFT_CONTOUR_MID_UP).imagePosition;
                var emotionText = GetEmotionText(face);

                graphics.DrawString(emotionText, font, brush,
                    centerPoint.X - sizeMeasure, centerPoint.Y,
                    new StringFormat(StringFormatFlags.DirectionRightToLeft));
            }

            private string GetEmotionText(FaceData face)
            {
                String emotionText = "";
                if (face.Emotions.PrimaryEmotion.Present)
                {
                    emotionText += face.Emotions.PrimaryEmotion.Type.ToString();
                }
                if (face.Emotions.PrimaryEmotion.Present && face.Emotions.PrimaryFeeling.Present)
                {
                    emotionText += "\n";
                }
                if (face.Emotions.PrimaryFeeling.Present)
                {
                    emotionText += face.Emotions.PrimaryFeeling.Type.ToString();
                }
                return emotionText;
            }
        }

        public class Builder
        {
            private FacesBuilder facesBuilder;

            public Builder WithFacesBuilder(FacesBuilder facesBuilder)
            {
                this.facesBuilder = facesBuilder;
                return this;
            }

            public EmotionsImageCreator Build()
            {
                return new EmotionsImageCreator(facesBuilder);
            }
        }
    }
}