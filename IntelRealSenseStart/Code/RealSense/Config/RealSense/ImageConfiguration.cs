using System.Drawing;

namespace IntelRealSenseStart.Code.RealSense.Config.RealSense
{
    public class ImageConfiguration
    {
        public static readonly ImageConfiguration DEFAULT_CONFIGURATION = new ImageConfiguration();
        public static readonly Size DEFAULT_RESOLUTION = new Size(640, 480);
        
        public bool ColorEnabled { get; private set; }
        public bool DepthEnabled { get; private set; }
        public bool ProjectionEnabled { get; private set; }

        public Size ColorResolution { get; private set; }
        public Size DepthResolution { get; private set; }

        protected ImageConfiguration()
        {
            ColorEnabled = true;
            DepthEnabled = true;
            ProjectionEnabled = true;

            ColorResolution = DEFAULT_RESOLUTION;
            DepthResolution = DEFAULT_RESOLUTION;
        }

        public class Builder
        {
            private readonly ImageConfiguration imageFeature;

            public Builder()
            {
                imageFeature = new ImageConfiguration();
            }

            public Builder WithColorEnabled()
            {
                if (!imageFeature.ColorEnabled)
                {
                    imageFeature.ColorEnabled = true;
                    imageFeature.ColorResolution = DEFAULT_RESOLUTION;
                }
                return this;
            }

            public Builder WithColorDisabled()
            {
                imageFeature.ColorEnabled = false;
                return this;
            }

            public Builder WithDepthEnabled()
            {
                if (!imageFeature.ColorEnabled)
                {
                    imageFeature.DepthEnabled = true;
                    imageFeature.DepthResolution = DEFAULT_RESOLUTION;
                }
                return this;
            }

            public Builder WithDepthDisabled()
            {
                imageFeature.DepthEnabled = false;
                return this;
            }

            public Builder WithColorResolution(Size resolution)
            {
                imageFeature.ColorEnabled = true;
                imageFeature.ColorResolution = resolution;
                return this;
            }

            public Builder WithDepthResolution(Size resolution)
            {
                imageFeature.DepthEnabled = true;
                imageFeature.DepthResolution = resolution;
                return this;
            }

            public Builder WithProjectionEnabled()
            {
                WithColorEnabled();
                WithDepthEnabled();

                imageFeature.ProjectionEnabled = true;
                return this;
            }

            public Builder WithProjectionDisabled()
            {
                imageFeature.ProjectionEnabled = false;
                return this;
            }

            public ImageConfiguration Build()
            {
                return imageFeature;
            }
        }
    }
}