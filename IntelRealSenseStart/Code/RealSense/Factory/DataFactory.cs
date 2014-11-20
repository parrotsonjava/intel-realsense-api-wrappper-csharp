using IntelRealSenseStart.Code.RealSense.Data;

namespace IntelRealSenseStart.Code.RealSense.Factory
{
    public class DataFactory
    {
        public ImageData.Builder ImageData()
        {
            return new ImageData.Builder();
        }

        public HandsData.Builder HandsData()
        {
            return new HandsData.Builder();
        }

        public HandData.Builder HandData()
        {
            return new HandData.Builder();
        }
    }
}
