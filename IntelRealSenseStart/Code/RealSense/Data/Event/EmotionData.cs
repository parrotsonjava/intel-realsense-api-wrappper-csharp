using IntelRealSenseStart.Code.RealSense.Exception;

namespace IntelRealSenseStart.Code.RealSense.Data.Event
{
    public interface EmotionData
    {
        bool Present { get; }


        EmotionType Type { get; }

        int Evidence { get; }

        float Intensity { get; }
    }

    public class AbsentEmotionData : EmotionData
    {
        private AbsentEmotionData()
        {
        }

        public bool Present
        {
            get { return false; }
        }

        public EmotionType Type
        {
            get { throw new IllegalStateException("No emotion was recognized"); }
        }

        public int Evidence
        {
            get { throw new IllegalStateException("No emotion was recognized"); }
        }

        public float Intensity
        {
            get { throw new IllegalStateException("No emotion was recognized"); }
        }

        public class Builder
        {
            private readonly AbsentEmotionData emotion;

            public Builder()
            {
                emotion = new AbsentEmotionData();
            }

            public AbsentEmotionData Build()
            {
                return emotion;
            }
        }
    }

    public class PresentEmotionData : EmotionData
    {
        private EmotionType type;

        private int evidence;
        private float intensity;

        private PresentEmotionData()
        {
        }

        public EmotionType Type
        {
            get { return type; }
        }

        public int Evidence
        {
            get { return evidence; }
        }

        public float Intensity
        {
            get { return intensity; }
        }

        public bool Present
        {
            get { return true; }
        }

        public class Builder
        {
            private readonly PresentEmotionData emotionData;
            
            public Builder()
            {
                emotionData = new PresentEmotionData();
            }

            public Builder WithEmotionType(EmotionType type)
            {
                emotionData.type = type;
                return this;
            }

            public Builder WithEvidence(int evidence)
            {
                emotionData.evidence = evidence;
                return this;
            }

            public Builder WithIntensity(float intensity)
            {
                emotionData.intensity = intensity;
                return this;
            }
            
            public PresentEmotionData Build()
            {
                return emotionData;
            }
        }
    }
}