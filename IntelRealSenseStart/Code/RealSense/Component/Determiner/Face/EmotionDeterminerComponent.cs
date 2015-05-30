using IntelRealSenseStart.Code.RealSense.Config.RealSense;
using IntelRealSenseStart.Code.RealSense.Data.Determiner;
using IntelRealSenseStart.Code.RealSense.Exception;
using IntelRealSenseStart.Code.RealSense.Helper;
using IntelRealSenseStart.Code.RealSense.Provider;

namespace IntelRealSenseStart.Code.RealSense.Component.Determiner.Face
{
    public class EmotionDeterminerComponent : FaceComponent
    {
        private readonly NativeSense nativeSense;
        private readonly RealSenseConfiguration configuration;

        private EmotionDeterminerComponent(NativeSense nativeSense, RealSenseConfiguration configuration)
        {
            this.nativeSense = nativeSense;
            this.configuration = configuration;
        }

        public void EnableFeatures()
        {
            var status = nativeSense.SenseManager.EnableEmotion();
            if (status < pxcmStatus.PXCM_STATUS_NO_ERROR)
            {
                throw new RealSenseInitializationException("Emotion detection could not be enabled");
            }
        }

        public void Configure(PXCMFaceConfiguration moduleConfiguration)
        {
            // Nothing to do
        }

        public void Process(int index, PXCMFaceData.Face face, FaceDeterminerData.Builder faceDeterminerData)
        {
            if (!configuration.FaceDetection.UseEmotions)
            {
                return;
            }

            var emotionData = GetEmotionDataForFaceIndex(index);
            faceDeterminerData.WithEmotions(emotionData);
        }

        private PXCMEmotion.EmotionData[] GetEmotionDataForFaceIndex(int index)
        {
            var emotion = nativeSense.SenseManager.QueryEmotion();
            PXCMEmotion.EmotionData[] emotionData;
            emotion.QueryAllEmotionData(index, out emotionData);
            return emotionData;
        }

        public void Stop(PXCMFaceData faceData)
        {
            // Nothing to do
        }

        public class Builder
        {
            private NativeSense nativeSense;
            private RealSenseConfiguration configuration;

            public Builder WithNativeSense(NativeSense nativeSense)
            {
                this.nativeSense = nativeSense;
                return this;
            }

            public Builder WithConfiguration(RealSenseConfiguration configuration)
            {
                this.configuration = configuration;
                return this;
            }

            public EmotionDeterminerComponent Build()
            {
                configuration.Check(Preconditions.IsNotNull,
                    "The RealSense configuration must be set in order to create the emotion determiner component");
                configuration.Check(Preconditions.IsNotNull,
                    "The native sense must be set in order to create the emotion determiner component");

                return new EmotionDeterminerComponent(nativeSense, configuration);
            }
        }
    }
}