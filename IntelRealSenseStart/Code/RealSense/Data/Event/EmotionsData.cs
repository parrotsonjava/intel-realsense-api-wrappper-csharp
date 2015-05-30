namespace IntelRealSenseStart.Code.RealSense.Data.Event
{
    public class EmotionsData
    {
        private EmotionData anger;
        private EmotionData contempt;
        private EmotionData disgust;
        private EmotionData fear;
        private EmotionData joy;
        private EmotionData sadness;
        private EmotionData surprise;

        private EmotionData positive;
        private EmotionData negative;
        private EmotionData neutral;

        private EmotionData primaryEmotion;
        private EmotionData primaryFeeling;

        private EmotionsData()
        {
            var absentEmotion = new AbsentEmotionData.Builder().Build();
            anger = absentEmotion;
            contempt = absentEmotion;
            disgust = absentEmotion;
            fear = absentEmotion;
            joy = absentEmotion;
            sadness = absentEmotion;
            surprise = absentEmotion;

            positive = absentEmotion;
            negative = absentEmotion;
            neutral = absentEmotion;

            primaryEmotion = absentEmotion;
            primaryFeeling = absentEmotion;
        }

        public EmotionData Anger
        {
            get { return anger; }
        }

        public EmotionData Contempt
        {
            get { return contempt; }
        }

        public EmotionData Disgust
        {
            get { return disgust; }
        }

        public EmotionData Fear
        {
            get { return fear; }
        }

        public EmotionData Joy
        {
            get { return joy; }
        }

        public EmotionData Sadness
        {
            get { return sadness; }
        }

        public EmotionData Surprise
        {
            get { return surprise; }
        }

        public EmotionData Positive
        {
            get { return positive; }
        }

        public EmotionData Negative
        {
            get { return negative; }
        }

        public EmotionData Neutral
        {
            get { return neutral; }
        }

        public EmotionData PrimaryEmotion
        {
            get { return primaryEmotion; }
        }

        public EmotionData PrimaryFeeling
        {
            get { return primaryFeeling; }
        }

        public class Builder
        {
            private readonly EmotionsData emotions;

            public Builder()
            {
                emotions = new EmotionsData();
            }

            public Builder WithEmotion(PresentEmotionData.Builder emotion)
            {
                AddEmotion(emotion.Build());
                return this;
            }

            private void AddEmotion(PresentEmotionData emotion)
            {
                switch (emotion.Type)
                {
                    case EmotionType.ANGER:
                        emotions.anger = emotion;
                        break;
                    case EmotionType.CONTEMPT:
                        emotions.contempt = emotion;
                        break;
                    case EmotionType.DISGUST:
                        emotions.disgust = emotion;
                        break;
                    case EmotionType.FEAR:
                        emotions.fear = emotion;
                        break;
                    case EmotionType.JOY:
                        emotions.joy = emotion;
                        break;
                    case EmotionType.SADNESS:
                        emotions.sadness = emotion;
                        break;
                    case EmotionType.SURPRISE:
                        emotions.surprise = emotion;
                        break;
                    case EmotionType.POSITIVE:
                        emotions.positive = emotion;
                        break;
                    case EmotionType.NEGATIVE:
                        emotions.negative = emotion;
                        break;
                    case EmotionType.NEUTRAL:
                        emotions.neutral = emotion;
                        break;
                }
            }

            public Builder WithPrimaryEmotion(PresentEmotionData.Builder emotion)
            {
                if (emotion != null)
                {
                    emotions.primaryEmotion = emotion.Build();
                }
                return this;
            }

            public Builder WithPrimaryFeeling(PresentEmotionData.Builder emotion)
            {
                if (emotion != null)
                {
                    emotions.primaryFeeling = emotion.Build();
                }
                return this;
            }

            public EmotionsData Build()
            {
                return emotions;
            }
        }
    }
}