using IntelRealSenseStart.Code.RealSense.Manager;

namespace IntelRealSenseStart.Code.RealSense.Factory
{
    public class ManagerFactory
    {
        public RealSensePropertiesManager.Builder PropertiesManager()
        {
            return new RealSensePropertiesManager.Builder();
        }

        public RealSenseDeterminerManager.Builder ComponentsManager()
        {
            return new RealSenseDeterminerManager.Builder();
        }
    }
}
