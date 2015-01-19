using IntelRealSenseStart.Code.RealSense.Data.Event;

namespace IntelRealSenseStart.Code.RealSense.Factory.Data
{
    public class EventDataFactory
    {
        public FacesLandmarksData.Builder FacesLandmarks()
        {
           return new FacesLandmarksData.Builder(); 
        }

        public FaceLandmarksData.Builder FaceLandmarks()
        {
            return new FaceLandmarksData.Builder();
        }

        public DetectionPoint.Builder DetectionPoint()
        {
            return new DetectionPoint.Builder();
        }
    }
}