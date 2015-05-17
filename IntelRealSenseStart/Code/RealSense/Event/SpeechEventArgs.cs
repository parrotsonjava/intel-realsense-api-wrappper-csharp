using System;

namespace IntelRealSenseStart.Code.RealSense.Event
{
    public class SpeechEventArgs
    {
        private String sentence;

        private SpeechEventArgs()
        { }

        public String Sentence
        {
            get { return sentence; }
        }

        public class Builder
        {
            private readonly SpeechEventArgs eventArgs;

            public Builder()
            {
                eventArgs = new SpeechEventArgs();
            }

            public Builder WithSentence(String sentence)
            {
                eventArgs.sentence = sentence;
                return this;
            }

            public SpeechEventArgs Build()
            {
                return eventArgs;
            }
        }
    }
}