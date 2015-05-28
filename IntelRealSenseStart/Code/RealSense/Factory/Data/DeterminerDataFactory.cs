using IntelRealSenseStart.Code.RealSense.Data.Determiner;

namespace IntelRealSenseStart.Code.RealSense.Factory.Data
{
    public class DeterminerDataFactory
    {
        public DeterminerData.Builder DeterminerData()
        {
            return new DeterminerData.Builder();
        }

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

        public FaceDeterminerData.Builder Face()
        {
            return new FaceDeterminerData.Builder();
        }
    }
}
