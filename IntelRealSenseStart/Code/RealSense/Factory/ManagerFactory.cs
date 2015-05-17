using IntelRealSenseStart.Code.RealSense.Manager;

namespace IntelRealSenseStart.Code.RealSense.Factory
{
    public class ManagerFactory
    {
        public RealSensePropertiesManager.Builder PropertiesManager()
        {
            return new RealSensePropertiesManager.Builder();
        }

        public RealSenseComponentsManager.Builder DeterminerManager()
        {
            return new RealSenseComponentsManager.Builder();
        }
    }
}