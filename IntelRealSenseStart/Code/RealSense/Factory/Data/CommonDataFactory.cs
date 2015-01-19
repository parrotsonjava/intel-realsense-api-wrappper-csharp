using IntelRealSenseStart.Code.RealSense.Data.Common;

namespace IntelRealSenseStart.Code.RealSense.Factory.Data
{
    public class CommonDataFactory
    {
        public Point2D.Builder Point2D()
        {
            return new Point2D.Builder();
        }

        public Point3D.Builder Point3D()
        {
            return new Point3D.Builder();
        }
    }
}
