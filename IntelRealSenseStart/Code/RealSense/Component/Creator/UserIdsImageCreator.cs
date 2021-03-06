﻿using System;
using System.Drawing;
using System.Globalization;
using IntelRealSenseStart.Code.RealSense.Config.Image;
using IntelRealSenseStart.Code.RealSense.Config.RealSense;
using IntelRealSenseStart.Code.RealSense.Data.Determiner;

namespace IntelRealSenseStart.Code.RealSense.Component.Creator
{
    public class UserIdsImageCreator : ImageCreator
    {
        private const int RIGHT_LANDMARK_INDEX = 68;

        private readonly RealSenseConfiguration realSenseConfiguration;

        private UserIdsImageCreator(RealSenseConfiguration realSenseConfiguration)
        {
            this.realSenseConfiguration = realSenseConfiguration;
        }

        public Bitmap Create(Bitmap bitmap, DeterminerData determinerData,
            ImageCreatorConfiguration imageCreatorConfiguration)
        {
            return new UserIdsImageCreatorRun(bitmap, determinerData, realSenseConfiguration, imageCreatorConfiguration).Create();
        }

        public class UserIdsImageCreatorRun
        {
            private readonly int sizeMeasure;

            private readonly Font font;
            private readonly SolidBrush brush;

            private readonly Bitmap bitmap;
            private readonly DeterminerData determinerData;

            private readonly RealSenseConfiguration realSenseConfiguration;
            private readonly ImageCreatorConfiguration imageCreatorConfiguration;

            public UserIdsImageCreatorRun(Bitmap bitmap, DeterminerData determinerData,
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
                        face.LandmarkPoints.Length > RIGHT_LANDMARK_INDEX)
                    {
                        DrawUserIds(graphics, face);
                    }
                }
            }

            private void DrawUserIds(Graphics graphics, FaceDeterminerData face)
            {
                var centerPoint = face.LandmarkPoints[RIGHT_LANDMARK_INDEX].image;

                DrawFaceId(graphics, face, centerPoint);
                DrawRecognizedId(graphics, face, centerPoint);
            }

            private void DrawFaceId(Graphics graphics, FaceDeterminerData face, PXCMPointF32 centerPoint)
            {
                var faceId = face.FaceId == -1 ? "N/A" : face.FaceId.ToString(CultureInfo.CurrentCulture);
                var faceIdText = String.Format("Face ID: {0}", faceId);

                graphics.DrawString(faceIdText, font, brush,
                    centerPoint.x + sizeMeasure, centerPoint.y);
            }

            private void DrawRecognizedId(Graphics graphics, FaceDeterminerData face, PXCMPointF32 centerPoint)
            {
                if (!realSenseConfiguration.FaceDetection.UseIdentification)
                {
                    return;
                }

                var recognizedId = face.FaceId == -1 ? "N/A" : face.RecognizedId.ToString(CultureInfo.CurrentCulture);
                var recognizedIdText = String.Format("Rec. ID: {0}", recognizedId);

                graphics.DrawString(recognizedIdText, font, brush,
                    centerPoint.x + sizeMeasure, centerPoint.y + (int) (1.5f * sizeMeasure));
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

            public UserIdsImageCreator Build()
            {
                return new UserIdsImageCreator(realSenseConfiguration);
            }
        }
    }
}