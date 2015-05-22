using IntelRealSenseStart.Code.RealSense.Manager;
using IntelRealSenseStart.Code.RealSense.Manager.Builder;

namespace IntelRealSenseStart.Code.RealSense.Factory
{
    public class ManagerFactory
    {
        public RealSensePropertyComponentsBuilder.Builder PropertyComponentsBuilder()
        {
            return new RealSensePropertyComponentsBuilder.Builder();
        }

        public RealSensePropertiesManager.Builder PropertiesManager()
        {
            return new RealSensePropertiesManager.Builder();
        }

        public RealSenseDeterminerComponentsBuilder.Builder DeterminerComponentsBuilder()
        {
            return new RealSenseDeterminerComponentsBuilder.Builder();
        }

        public RealSenseComponentsManager.Builder Components()
        {
            return new RealSenseComponentsManager.Builder();
        }
    }
}