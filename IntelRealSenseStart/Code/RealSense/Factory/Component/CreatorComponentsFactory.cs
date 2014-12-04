using IntelRealSenseStart.Code.RealSense.Component.Creator;

namespace IntelRealSenseStart.Code.RealSense.Factory.Component
{
    public class CreatorComponentsFactory
    {
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
