using IntelRealSenseStart.Code.RealSense.Exception;

namespace IntelRealSenseStart.Code.RealSense.Config.RealSense
{
    public class RealSenseConfiguration
    {
        private BaseConfiguration baseConfiguration;
        private ImageConfiguration imageConfiguration;
        private HandsConfiguration handsConfiguration;
        private FaceConfiguration faceConfiguration;
        private SpeechRecognitionConfiguration speechRecognitionConfiguration;
        private SpeechSynthesisConfiguration speechSynthesisConfiguration;

        private RealSenseConfiguration()
        {
            baseConfiguration = BaseConfiguration.DEFAULT_CONFIGURATION;
            imageConfiguration = ImageConfiguration.DEFAULT_CONFIGURATION;
        }

        public BaseConfiguration Base
        {
            get { return baseConfiguration; }
        }

        public ImageConfiguration Image
        {
            get { return imageConfiguration; }
        }

        public bool HandsDetectionEnabled
        {
            get { return handsConfiguration != null; }
        }

        public HandsConfiguration HandsDetection
        {
            get
            {
                if (handsConfiguration == null)
                {
                    throw new RealSenseException("Hands detection is not enabled, but tried to access it");
                }
                return handsConfiguration;
            }
        }

        public bool FaceDetectionEnabled
        {
            get { return faceConfiguration != null; }
        }

        public FaceConfiguration FaceDetection
        {
            get
            {
                if (faceConfiguration == null)
                {
                    throw new RealSenseException("Face detection is not enabled, but tried to access it");
                }
                return faceConfiguration;
            }
        }

        public bool SpeechRecognitionEnabled
        {
            get { return speechRecognitionConfiguration != null; }
        }

        public SpeechRecognitionConfiguration SpeechRecognition
        {
            get
            {
                if (speechRecognitionConfiguration == null)
                {
                    throw new RealSenseException("Speech recognition is not enabled, but tried to access it");
                }
                return speechRecognitionConfiguration;
            }
        }

        public bool SpeechSynthesisEnabled
        {
            get { return speechSynthesisConfiguration != null; }
        }

        public SpeechSynthesisConfiguration SpeechSynthesis
        {
            get
            {
                if (speechSynthesisConfiguration == null)
                {
                    throw new RealSenseException("Speech synthesis is not enabled, but tried to access it");
                }
                return speechSynthesisConfiguration;
            }
        }

        public class Builder
        {
            private readonly RealSenseConfiguration configuration;

            public Builder()
            {
                configuration = new RealSenseConfiguration();
            }

            public Builder UsingBaseConfiguration(BaseConfiguration.Builder deviceConfiguration)
            {
                configuration.baseConfiguration = deviceConfiguration.Build();
                return this;
            }

            public Builder WithHandsDetection(HandsConfiguration.Builder handsConfiguration)
            {
                configuration.handsConfiguration = handsConfiguration.Build();
                return this;
            }

            public Builder WithFaceDetection(FaceConfiguration.Builder faceConfiguration)
            {
                configuration.faceConfiguration = faceConfiguration.Build();
                return this;
            }

            public Builder WithSpeechRecognition(SpeechRecognitionConfiguration.Builder speechRecognitionConfiguration)
            {
                configuration.speechRecognitionConfiguration = speechRecognitionConfiguration.Build();
                return this;
            }

            public Builder WithSpeechSynthesis(SpeechSynthesisConfiguration.Builder speechSynthesisConfiguration)
            {
                configuration.speechSynthesisConfiguration = speechSynthesisConfiguration.Build();
                return this;
            }

            public Builder WithImage(ImageConfiguration.Builder imageConfiguration)
            {
                configuration.imageConfiguration = imageConfiguration.Build();
                return this;
            }

            public RealSenseConfiguration Build()
            {
                return configuration;
            }
        }
    }
}