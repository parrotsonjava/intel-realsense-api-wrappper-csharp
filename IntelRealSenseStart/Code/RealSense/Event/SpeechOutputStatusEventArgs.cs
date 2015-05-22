using IntelRealSenseStart.Code.RealSense.Event.Data;

namespace IntelRealSenseStart.Code.RealSense.Event
{
    public class SpeechOutputStatusEventArgs
    {
        private SpeechOutputStatus status;

        public SpeechOutputStatus Status
        {
            get { return status; }
        }

        public class Builder
        {
            private readonly SpeechOutputStatusEventArgs eventArgs;

            public Builder()
            {
                eventArgs = new SpeechOutputStatusEventArgs();
                WithDefaultValues();
            }

            public Builder WithDefaultValues()
            {
                return WithStatus(SpeechOutputStatus.STARTED_SPEAKING);
            }

            public Builder WithStatus(SpeechOutputStatus status)
            {
                eventArgs.status = status;
                return this;
            }

            public SpeechOutputStatusEventArgs Build()
            {
                return eventArgs;
            }
        }
    }
}