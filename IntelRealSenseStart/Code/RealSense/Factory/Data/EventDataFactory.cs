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

        public HandsJointsData.Builder HandsJoints()
        {
            return new HandsJointsData.Builder();
        }

        public HandJointsData.Builder HandJoints()
        {
            return new HandJointsData.Builder();
        }

        public DetectionPoint.Builder DetectionPoint()
        {
            return new DetectionPoint.Builder();
        }
    }
}