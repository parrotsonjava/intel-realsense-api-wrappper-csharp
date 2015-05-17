using IntelRealSenseStart.Code.RealSense.Exception;

namespace IntelRealSenseStart.Code.RealSense.Config.RealSense
{
    public class RealSenseConfiguration
    {
        private DeviceConfiguration deviceConfiguration;
        private ImageConfiguration imageConfiguration;
        private HandsConfiguration handsConfiguration;
        private FaceConfiguration faceConfiguration;
        private SpeechConfiguration speechConfiguration;

        private RealSenseConfiguration()
        {
            deviceConfiguration = DeviceConfiguration.DEFAULT_CONFIGURATION;
            imageConfiguration = ImageConfiguration.DEFAULT_CONFIGURATION;
        }

        public DeviceConfiguration Device
        {
            get { return deviceConfiguration; }
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

        public bool SpeechDetectionEnabled
        {
            get { return speechConfiguration != null; }
        }

        public SpeechConfiguration SpeechRecognition
        {
            get
            {
                if (speechConfiguration == null)
                {
                    throw new RealSenseException("Speech recognition is not enabled, but tried to access it");
                }
                return speechConfiguration;
            }
        }

        public class Builder
        {
            private readonly RealSenseConfiguration configuration;

            public Builder()
            {
                configuration = new RealSenseConfiguration();
            }

            public Builder UsingDeviceConfiguration(DeviceConfiguration.Builder deviceConfiguration)
            {
                configuration.deviceConfiguration = deviceConfiguration.Build();
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

            public Builder WithSpeechRecognition(SpeechConfiguration.Builder speechConfiguration)
            {
                configuration.speechConfiguration = speechConfiguration.Build();
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