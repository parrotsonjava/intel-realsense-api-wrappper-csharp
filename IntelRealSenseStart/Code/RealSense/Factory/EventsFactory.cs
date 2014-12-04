
using IntelRealSenseStart.Code.RealSense.Config.HandsImage;
using IntelRealSenseStart.Code.RealSense.Event;

namespace IntelRealSenseStart.Code.RealSense.Factory
{
    public class EventsFactory
    {
        private readonly RealSenseFactory factory;

        public EventsFactory(RealSenseFactory factory)
        {
            this.factory = factory;
        }

        public FrameEventArgs.Builder FrameEvent(Config.RealSense.Configuration configuration)
        {
            return new FrameEventArgs.Builder(factory, configuration);
        }

        public HandsImageConfiguration.Builder HandsImageConfiguration()
        {
            return new HandsImageConfiguration.Builder();
        }
    }
}