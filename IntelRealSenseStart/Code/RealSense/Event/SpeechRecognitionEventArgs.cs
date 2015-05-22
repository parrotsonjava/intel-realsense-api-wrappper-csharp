using System;

namespace IntelRealSenseStart.Code.RealSense.Event
{
    public class SpeechRecognitionEventArgs
    {
        private String sentence;

        private SpeechRecognitionEventArgs()
        { }

        public String Sentence
        {
            get { return sentence; }
        }

        public class Builder
        {
            private readonly SpeechRecognitionEventArgs _recognitionEventArgs;

            public Builder()
            {
                _recognitionEventArgs = new SpeechRecognitionEventArgs();
            }

            public Builder WithSentence(String sentence)
            {
                _recognitionEventArgs.sentence = sentence;
                return this;
            }

            public SpeechRecognitionEventArgs Build()
            {
                return _recognitionEventArgs;
            }
        }
    }
}