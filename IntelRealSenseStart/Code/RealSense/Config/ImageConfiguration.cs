using System.Drawing;

namespace IntelRealSenseStart.Code.RealSense.Config
{
    public class ImageConfiguration : ConfigurationOption
    {
        public static readonly Size DEFAULT_RESOLUTION = new Size(640, 480);

        private Size resolution;

        protected ImageConfiguration()
        {
            resolution = DEFAULT_RESOLUTION;
        }

        public Size Resolution
        {
            get { return resolution; }
        }

        public class Builder
        {
            private readonly ImageConfiguration imageFeature;

            public Builder()
            {
                imageFeature = new ImageConfiguration();
            }

            public Builder WithResolution(Size resolution)
            {
                imageFeature.resolution = resolution;
                return this;
            }

            public ImageConfiguration Build()
            {
                return imageFeature;
            }
        }
    }
}