using System.Collections.Generic;

namespace IntelRealSenseStart.Code.RealSense.Data.Event
{
    public class HandJointsData
    {
        private readonly Dictionary<HandJoint, DetectionPoint> detectionPoints;

        private HandJointsData()
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
            private readonly HandJointsData handJointsData;

            public Builder()
            {
                handJointsData = new HandJointsData();
            }

            public Builder WithDetectionPoint(HandJoint handJoint, DetectionPoint.Builder detectionPoint)
            {
                handJointsData.detectionPoints[handJoint] = detectionPoint.Build();
                return this;
            }

            public HandJointsData Build()
            {
                return handJointsData;
            }
        }
    }
}
