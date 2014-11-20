using IntelRealSenseStart.Code.RealSense.Component;

namespace IntelRealSenseStart.Code.RealSense.Factory
{
    public class ComponentsFactory
    {
        public RealSenseComponentsManager.Builder ComponentsManager()
        {
            return new RealSenseComponentsManager.Builder();
        }

        public ImageComponent.Builder ImageComponent()
        {
            return new ImageComponent.Builder();
        }

        public HandsComponent.Builder HandsComponent()
        {
            return new HandsComponent.Builder();
        }
    }
}