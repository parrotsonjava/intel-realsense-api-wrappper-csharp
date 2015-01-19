namespace IntelRealSenseStart.Code.RealSense.Data.Common
{
    public class Point3D
    {
        public float X { get; private set; }
        public float Y { get; private set; }
        public float Z { get; private set; }

        private Point3D()
        {
        }

        public class Builder
        {
            private readonly Point3D point3D;

            public Builder()
            {
                point3D = new Point3D();
            }

            public Builder From(PXCMPoint3DF32 point)
            {
                point3D.X = point.x;
                point3D.Y = point.y;
                point3D.Z = point.z;
            }

            public Builder WithX(float x)
            {
                point3D.X = x;
                return this;
            }

            public Builder WithY(float y)
            {
                point3D.Y = y;
                return this;
            }

            public Builder WithZ(float z)
            {
                point3D.Z = z;
                return this;
            }

            public Point3D Build()
            {
                return point3D;
            }
        }
    }
}