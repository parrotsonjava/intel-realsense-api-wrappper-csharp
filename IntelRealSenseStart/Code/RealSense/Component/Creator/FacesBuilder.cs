using System.Collections.Generic;
using System.Linq;
using IntelRealSenseStart.Code.RealSense.Data.Common;
using IntelRealSenseStart.Code.RealSense.Data.Determiner;
using IntelRealSenseStart.Code.RealSense.Data.Event;
using IntelRealSenseStart.Code.RealSense.Factory;
using IntelRealSenseStart.Code.RealSense.Helper;
using FaceData = IntelRealSenseStart.Code.RealSense.Data.Event.FaceData;
using FacesData = IntelRealSenseStart.Code.RealSense.Data.Event.FacesData;

namespace IntelRealSenseStart.Code.RealSense.Component.Creator
{
    public class FacesBuilder
    {
        private const int NUMBER_OF_DISTINCT_EMOTIONS = 7;
        private const int NUMBER_OF_DISTINCT_FEELINGS = 3;

        private const float INTENSITY_THRESHOLD = 0.5f;

        private readonly RealSenseFactory factory;

            public FacesBuilder(RealSenseFactory factory)
        {
            this.factory = factory;
        }

        public FacesData GetFacesData(List<FaceDeterminerData> facesDeterminerData)
        {
            var facesLandmarks = factory.Data.Events.Faces();
            facesDeterminerData.Do(faceData => facesLandmarks.WithFaceLandmarks(GetFaceData(faceData)));
            return facesLandmarks.Build();
        }

        private FaceData.Builder GetFaceData(FaceDeterminerData faceDeterminerData)
        {
            var face = factory.Data.Events.Face();
            if (faceDeterminerData.LandmarkPoints != null)
            {
                0.To(faceDeterminerData.LandmarkPoints.Length - 1).ToArray().Do(index =>
                    face.WithDetectionPoint(
                        GetLandmarkName(index),
                        GetDetectionPoint(faceDeterminerData.LandmarkPoints[index])));
            }

            return face.WithPulseData(faceDeterminerData.PulseData)
                .WithFaceId(faceDeterminerData.FaceId)
                .WithRecognizedId(GetRecognizedId(faceDeterminerData))
                .WithEmotionsData(GetEmotionsData(faceDeterminerData));
        }

        private FaceLandmark GetLandmarkName(int index)
        {
            return (FaceLandmark) index;
        }

        private DetectionPoint.Builder GetDetectionPoint(PXCMFaceData.LandmarkPoint landmarkPoint)
        {
            return factory.Data.Events.DetectionPoint()
                .WithImagePosition(GetPoint2DFrom(landmarkPoint.image, landmarkPoint.confidenceImage))
                .WithWorldPosition(GetPoint3DFrom(landmarkPoint.world, landmarkPoint.confidenceWorld));
        }

        private Point2D.Builder GetPoint2DFrom(PXCMPointF32 point, int confidence)
        {
            return factory.Data.Common.Point2D().From(point).WithConfidence(confidence);
        }

        private Point3D.Builder GetPoint3DFrom(PXCMPoint3DF32 point, int confidence)
        {
            return factory.Data.Common.Point3D().From(point).WithConfidence(confidence);
        }

        private int? GetRecognizedId(FaceDeterminerData faceDeterminerData)
        {
            if (faceDeterminerData.RecognizedId == -1)
            {
                return null;
            }
            return faceDeterminerData.RecognizedId;
        }

        private EmotionsData.Builder GetEmotionsData(FaceDeterminerData faceDeterminerData)
        {
            var emotions = factory.Data.Events.Emotions();
            if (faceDeterminerData.Emotions == null)
            {
                return emotions;
            }

            DetermineEmotionsFor(faceDeterminerData, emotions);
            return emotions
                .WithPrimaryEmotion(GetPrimaryEmotion(faceDeterminerData))
                .WithPrimaryFeeling(GetPrimaryFeeling(faceDeterminerData));
        }

        private void DetermineEmotionsFor(FaceDeterminerData faceDeterminerData, EmotionsData.Builder emotions)
        {
            faceDeterminerData.Emotions.Select(GetEmotionData).Do(emotion => emotions.WithEmotion(emotion));
        }

        private PresentEmotionData.Builder GetPrimaryEmotion(FaceDeterminerData faceDeterminerData)
        {
            var determinerEmotionData = faceDeterminerData.Emotions
                .Take(NUMBER_OF_DISTINCT_EMOTIONS)
                .OrderBy(emotion => emotion.evidence)
                .LastOrDefault(emotion => emotion.intensity > INTENSITY_THRESHOLD);

            return determinerEmotionData == null ? null : GetEmotionData(determinerEmotionData);
        }

        private PresentEmotionData.Builder GetPrimaryFeeling(FaceDeterminerData faceDeterminerData)
        {
            var determinerEmotionData = faceDeterminerData.Emotions
                .Reverse()
                .Take(NUMBER_OF_DISTINCT_FEELINGS)
                .OrderBy(emotion => emotion.evidence)
                .LastOrDefault(emotion => emotion.intensity > INTENSITY_THRESHOLD);

            return determinerEmotionData == null ? null : GetEmotionData(determinerEmotionData);
        }

        private PresentEmotionData.Builder GetEmotionData(PXCMEmotion.EmotionData emotionData)
        {
            return factory.Data.Events.PresentEmotion()
                .WithEmotionType(emotionData.eid.EmotionType())
                .WithEvidence(emotionData.evidence)
                .WithIntensity(emotionData.intensity);
        }

        public class Builder
        {
            private readonly FacesBuilder facesLandmarksBuilder;

            public Builder(RealSenseFactory factory)
            {
                facesLandmarksBuilder = new FacesBuilder(factory);
            }

            public FacesBuilder Build()
            {
                return facesLandmarksBuilder;
            }
        }
    }
}