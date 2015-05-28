using System.Collections.Generic;

namespace IntelRealSenseStart.Code.RealSense.Data.Event
{
    public class HandData
    {
        private readonly Dictionary<HandJoint, DetectionPoint> detectionPoints;

        private HandData()
        {
            detectionPoints = new Dictionary<HandJoint, DetectionPoint>();
        }

        protected Dictionary<HandJoint, DetectionPoint> DetectionPoints
        {
            get { return detectionPoints; }
        }

        public DetectionPoint GetPoint(HandJoint landmark)
        {
            return detectionPoints[landmark];
        }

        public class Builder
        {
            private readonly HandData handData;

            public Builder()
            {
                handData = new HandData();
            }

            public Builder WithDetectionPoint(HandJoint handJoint, DetectionPoint.Builder detectionPoint)
            {
                handData.detectionPoints[handJoint] = detectionPoint.Build();
                return this;
            }

            public HandData Build()
            {
                return handData;
            }
        }
    }
}