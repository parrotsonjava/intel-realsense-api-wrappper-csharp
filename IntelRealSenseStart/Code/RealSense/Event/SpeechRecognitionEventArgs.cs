using System;
using System.Collections.Generic;
using IntelRealSenseStart.Code.RealSense.Event.Data;

namespace IntelRealSenseStart.Code.RealSense.Event
{
    public class SpeechRecognitionEventArgs
    {
        private List<SpeechRecognitionMatch> matches;

        private SpeechRecognitionEventArgs()
        {
            matches = new List<SpeechRecognitionMatch>();
        }

        public List<SpeechRecognitionMatch> Matches
        {
            get { return matches; }
        }

        public class Builder
        {
            private readonly SpeechRecognitionEventArgs eventArgs;

            public Builder()
            {
                eventArgs = new SpeechRecognitionEventArgs();
            }

            public Builder WithSentence(String sentence, int confidence)
            {
                eventArgs.matches.Add(new SpeechRecognitionMatch(sentence, confidence));
                return this;
            }

            public SpeechRecognitionEventArgs Build()
            {
                return eventArgs;
            }
        }
    }
}