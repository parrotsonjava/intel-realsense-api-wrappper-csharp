using IntelRealSenseStart.Code.RealSense.Data.Event;

namespace IntelRealSenseStart.Code.RealSense.Factory.Data
{
    public class EventDataFactory
    {
        public FacesData.Builder Faces()
        {
           return new FacesData.Builder(); 
        }

        public FaceData.Builder Face()
        {
            return new FaceData.Builder();
        }

        public HandsData.Builder Hands()
        {
            return new HandsData.Builder();
        }

        public HandData.Builder Hand()
        {
            return new HandData.Builder();
        }

        public DetectionPoint.Builder DetectionPoint()
        {
            return new DetectionPoint.Builder();
        }
    }
}