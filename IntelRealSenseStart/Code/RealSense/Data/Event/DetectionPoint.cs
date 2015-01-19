using IntelRealSenseStart.Code.RealSense.Data.Common;

namespace IntelRealSenseStart.Code.RealSense.Data.Event
{
    public class DetectionPoint
    {
        // TODO confidence levels

        public Point2D imagePosition;

        public Point3D worldPosition;

        public class Builder
        {
            private readonly DetectionPoint detectionPoint;

            public Builder()
            {
                detectionPoint = new DetectionPoint();
            }

            public Builder WithImagePosition(Point2D.Builder imagePosition)
            {
                detectionPoint.imagePosition = imagePosition.Build();
                return this;
            }

            public Builder WithWorldPosition(Point3D.Builder worldPosition)
            {
                detectionPoint.worldPosition = worldPosition.Build();
                return this;
            }

            public DetectionPoint Build()
            {
                return detectionPoint;
            }
        }
    }
}