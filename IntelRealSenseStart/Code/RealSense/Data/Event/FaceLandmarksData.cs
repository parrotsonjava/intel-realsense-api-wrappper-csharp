using System.Collections.Generic;

namespace IntelRealSenseStart.Code.RealSense.Data.Event
{
    public class FaceLandmarksData
    {
        private readonly Dictionary<FaceLandmark, DetectionPoint> detectionPoints;

        public FaceLandmarksData()
        {
            detectionPoints = new Dictionary<FaceLandmark, DetectionPoint>();
        }

        protected Dictionary<FaceLandmark, DetectionPoint> DetectionPoints
        {
            get { return detectionPoints; }
        }

        public DetectionPoint GetPoint(FaceLandmark landmark)
        {
            return detectionPoints[landmark];
        }

        public class Builder
        {
            private readonly FaceLandmarksData faceLandmarksData;

            public Builder()
            {
                faceLandmarksData = new FaceLandmarksData();
            }

            public Builder WithDetectionPoint(FaceLandmark landmark, DetectionPoint.Builder detectionPoint)
            {
                faceLandmarksData.detectionPoints[landmark] = detectionPoint.Build();
                return this;
            }

            public FaceLandmarksData Build()
            {
                return faceLandmarksData;
            }
        }
    }
}