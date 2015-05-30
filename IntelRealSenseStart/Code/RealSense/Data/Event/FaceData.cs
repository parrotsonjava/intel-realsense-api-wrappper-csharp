using System.Collections.Generic;

namespace IntelRealSenseStart.Code.RealSense.Data.Event
{
    public class FaceData
    {
        private readonly Dictionary<FaceLandmark, DetectionPoint> detectionPoints;

        private float heartRate;
        private EmotionsData emotionsData;

        private int faceId;
        private int? recognizedId;

        private FaceData()
        {
            detectionPoints = new Dictionary<FaceLandmark, DetectionPoint>();
            emotionsData = null;
        }

        protected Dictionary<FaceLandmark, DetectionPoint> DetectionPoints
        {
            get { return detectionPoints; }
        }

        public DetectionPoint GetPoint(FaceLandmark landmark)
        {
            return detectionPoints[landmark];
        }

        public float HeartRate
        {
            get { return heartRate; }
        }

        public EmotionsData Emotions
        {
            get { return emotionsData; }
        }

        public int FaceId
        {
            get { return faceId; }
        }

        public int? RecognizedId
        {
            get { return recognizedId; }
        }

        public class Builder
        {
            private readonly FaceData faceData;

            public Builder()
            {
                faceData = new FaceData();
            }

            public Builder WithDetectionPoint(FaceLandmark landmark, DetectionPoint.Builder detectionPoint)
            {
                faceData.detectionPoints[landmark] = detectionPoint.Build();
                return this;
            }

            public Builder WithPulseData(PXCMFaceData.PulseData pulseData)
            {
                if (pulseData != null)
                {
                    faceData.heartRate = pulseData.QueryHeartRate();
                }
                return this;
            }

            public Builder WithFaceId(int faceId)
            {
                faceData.faceId = faceId;
                return this;
            }

            public Builder WithRecognizedId(int? recognizedId)
            {
                faceData.recognizedId = recognizedId;
                return this;
            }

            public Builder WithEmotionsData(EmotionsData.Builder emotionsData)
            {
                faceData.emotionsData = emotionsData.Build();
                return this;
            }

            public FaceData Build()
            {
                return faceData;
            }
        }
    }
}