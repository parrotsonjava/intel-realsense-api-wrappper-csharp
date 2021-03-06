﻿namespace IntelRealSenseStart.Code.RealSense.Data.Common
{
    public class Point2D
    {
        public float X { get; private set; }
        public float Y { get; private set; }

        public int Confidence { get; private set; }

        private Point2D()
        {
        }

        public class Builder
        {
            private readonly Point2D point2D;

            public Builder()
            {
                point2D = new Point2D();
            }

            public Builder From(PXCMPointF32 point)
            {
                point2D.X = point.x;
                point2D.Y = point.y;
                return this;
            }

            public Builder From(PXCMPoint3DF32 point)
            {
                point2D.X = point.x;
                point2D.Y = point.y;
                return this;
            }
            
            public Builder WithX(float x)
            {
                point2D.X = x;
                return this;
            }

            public Builder WithY(float y)
            {
                point2D.Y = y;
                return this;
            }

            public Builder WithConfidence(int confidence)
            {
                point2D.Confidence = confidence;
                return this;
            }

            public Point2D Build()
            {
                return point2D;
            }
        }
    }
}