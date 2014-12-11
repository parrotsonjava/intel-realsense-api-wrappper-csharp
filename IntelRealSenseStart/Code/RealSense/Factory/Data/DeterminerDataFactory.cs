using IntelRealSenseStart.Code.RealSense.Data;
using IntelRealSenseStart.Code.RealSense.Data.Determiner;

namespace IntelRealSenseStart.Code.RealSense.Factory.Data
{
    public class DeterminerDataFactory
    {
        public ImageData.Builder Image()
        {
            return new ImageData.Builder();
        }

        public HandsData.Builder Hands()
        {
            return new HandsData.Builder();
        }

        public HandData.Builder Hand()
        {
            return new HandData.Builder();
        }

        public FacesData.Builder Faces()
        {
            return new FacesData.Builder();
        }

        public FaceData.Builder Face()
        {
            return new FaceData.Builder();
        }
    }
}
