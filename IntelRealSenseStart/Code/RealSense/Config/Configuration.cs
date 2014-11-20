using IntelRealSenseStart.Code.RealSense.Exception;

namespace IntelRealSenseStart.Code.RealSense.Config
{
    public class Configuration
    {
        private ImageConfiguration colorImageFeature;
        private ImageConfiguration depthImageFeature;
        private HandsConfiguration handsFeature;

        public bool HandsDetectionEnabled
        {
            get { return handsFeature != null; }
        }

        public HandsConfiguration HandsDetection
        {
            get
            {
                if (handsFeature == null)
                {
                    throw new RealSenseException("Hands detection is not enabled, but tried to access it");
                }
                return handsFeature;
            }
        }

        public bool ColorImageEnabled
        {
            get { return colorImageFeature != null; }
        }

        public ImageConfiguration ColorImage
        {
            get
            {
                if (colorImageFeature == null)
                {
                    throw new RealSenseException("Hands detection is not enabled, but tried to access it");
                }
                return colorImageFeature;
            }
        }

        public bool DepthImageEnabled
        {
            get { return depthImageFeature != null; }
        }

        public ImageConfiguration DepthImage
        {
            get
            {
                if (depthImageFeature == null)
                {
                    throw new RealSenseException("Hands detection is not enabled, but tried to access it");
                }
                return depthImageFeature;
            }
        }

        public class Builder
        {
            private readonly Configuration featureList;

            public Builder()
            {
                featureList = new Configuration();
            }

            public Builder WithHandsDetection(HandsConfiguration.Builder handsFeature)
            {
                featureList.handsFeature = handsFeature.Build();
                return this;
            }

            public Builder WithColorImage(ImageConfiguration.Builder colorImageFeature)
            {
                featureList.colorImageFeature = colorImageFeature.Build();
                return this;
            }

            public Builder WithDepthImage(ImageConfiguration.Builder depthImageFeature)
            {
                featureList.depthImageFeature = depthImageFeature.Build();
                return this;
            }

            public Configuration Build()
            {
                return featureList;
            }
        }
    }
}