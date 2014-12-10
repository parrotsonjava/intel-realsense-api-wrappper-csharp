using IntelRealSenseStart.Code.RealSense.Exception;

namespace IntelRealSenseStart.Code.RealSense.Config.RealSense
{
    public class Configuration
    {
        private DeviceConfiguration deviceConfiguration;
        private ImageConfiguration colorImageConfig;
        private ImageConfiguration depthImageConfig;
        private HandsConfiguration handsConfiguration;
        private FaceConfiguration faceConfiguration;

        private Configuration()
        {
            deviceConfiguration = DeviceConfiguration.DEFAULT_CONFIGURATION;
        }

        public DeviceConfiguration Device
        {
            get { return deviceConfiguration; }
        }

        public bool ColorImageEnabled
        {
            get { return colorImageConfig != null; }
        }

        public ImageConfiguration ColorImage
        {
            get
            {
                if (colorImageConfig == null)
                {
                    throw new RealSenseException("Hands detection is not enabled, but tried to access it");
                }
                return colorImageConfig;
            }
        }

        public bool DepthImageEnabled
        {
            get { return depthImageConfig != null; }
        }

        public ImageConfiguration DepthImage
        {
            get
            {
                if (depthImageConfig == null)
                {
                    throw new RealSenseException("Hands detection is not enabled, but tried to access it");
                }
                return depthImageConfig;
            }
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

        public class Builder
        {
            private readonly Configuration configuration;

            public Builder()
            {
                configuration = new Configuration();
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

            public Builder WithColorImage(ImageConfiguration.Builder colorImageFeature)
            {
                configuration.colorImageConfig = colorImageFeature.Build();
                return this;
            }

            public Builder WithDepthImage(ImageConfiguration.Builder depthImageFeature)
            {
                configuration.depthImageConfig = depthImageFeature.Build();
                return this;
            }

            public Configuration Build()
            {
                return configuration;
            }
        }
    }
}