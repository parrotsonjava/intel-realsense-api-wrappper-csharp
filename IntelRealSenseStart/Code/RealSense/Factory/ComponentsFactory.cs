using IntelRealSenseStart.Code.RealSense.Component;
using IntelRealSenseStart.Code.RealSense.Component.Hands;

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

        public HandsImageBuilder.Builder HandsImageBuilder()
        {
            return new HandsImageBuilder.Builder();
        }

        public HandsImageCreator.Builder HandsImageCreator()
        {
            return new HandsImageCreator.Builder();
        }
    }
}