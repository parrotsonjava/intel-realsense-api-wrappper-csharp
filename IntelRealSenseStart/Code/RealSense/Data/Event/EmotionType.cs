namespace IntelRealSenseStart.Code.RealSense.Data.Event
{
    public enum EmotionType
    {
        ANGER, CONTEMPT, DISGUST, FEAR, JOY, SADNESS, SURPRISE, POSITIVE, NEGATIVE, NEUTRAL, NONE
    }

    public static class EmotionTypeExtensions
    {
        public static EmotionType EmotionType(this PXCMEmotion.Emotion nativeEmotionType)
        {
            switch (nativeEmotionType)
            {
                case PXCMEmotion.Emotion.EMOTION_PRIMARY_ANGER:
                    return Event.EmotionType.ANGER;
                case PXCMEmotion.Emotion.EMOTION_PRIMARY_CONTEMPT:
                    return Event.EmotionType.CONTEMPT;
                case PXCMEmotion.Emotion.EMOTION_PRIMARY_DISGUST:
                    return Event.EmotionType.DISGUST;
                case PXCMEmotion.Emotion.EMOTION_PRIMARY_FEAR:
                    return Event.EmotionType.FEAR;
                case PXCMEmotion.Emotion.EMOTION_PRIMARY_JOY:
                    return Event.EmotionType.JOY;
                case PXCMEmotion.Emotion.EMOTION_PRIMARY_SADNESS:
                    return Event.EmotionType.SADNESS;
                case PXCMEmotion.Emotion.EMOTION_PRIMARY_SURPRISE:
                    return Event.EmotionType.SURPRISE;
                case PXCMEmotion.Emotion.EMOTION_SENTIMENT_POSITIVE:
                    return Event.EmotionType.POSITIVE;
                case PXCMEmotion.Emotion.EMOTION_SENTIMENT_NEGATIVE:
                    return Event.EmotionType.NEGATIVE;
                case PXCMEmotion.Emotion.EMOTION_SENTIMENT_NEUTRAL :
                    return Event.EmotionType.NEUTRAL;
                default:
                    return Event.EmotionType.NONE;
            }
        }
    }
}