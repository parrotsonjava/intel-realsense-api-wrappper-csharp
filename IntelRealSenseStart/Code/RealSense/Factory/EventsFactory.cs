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

        public FrameEventArgs.Builder FrameEvent()
        {
            return new FrameEventArgs.Builder(factory);
        }

        public SpeechEventArgs.Builder SpeechEvent()
        {
            return new SpeechEventArgs.Builder();
        }
    }
}