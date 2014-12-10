using IntelRealSenseStart.Code.RealSense.Data;
using IntelRealSenseStart.Code.RealSense.Data.Determiner;

namespace IntelRealSenseStart.Code.RealSense.Factory.Data
{
    public class DeterminerDataFactory
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
