using System;

namespace IntelRealSenseStart.Code.RealSense.Event.Data
{
    public class SpeechRecognitionMatch
    {
        private readonly String sentence;
        private readonly int confidence;

        public SpeechRecognitionMatch(String sentence, int confidence)
        {
            this.sentence = sentence;
            this.confidence = confidence;
        }

        public String Sentence
        {
            get { return sentence; }
        }

        public int Confidence
        {
            get { return confidence; }
        }
    }
}