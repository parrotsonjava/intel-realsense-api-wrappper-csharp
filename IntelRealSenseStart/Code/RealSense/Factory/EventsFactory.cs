using IntelRealSenseStart.Code.RealSense.Component.Event;

namespace IntelRealSenseStart.Code.RealSense.Factory
{
    public class EventsFactory
    {
        public FrameEventArgs.Builder FrameEvent()
        {
            return new FrameEventArgs.Builder();
        }
    }
}