using System.Drawing;
using IntelRealSenseStart.Code.RealSense.Helper;

namespace IntelRealSenseStart.Code.RealSense.Config.RealSense
{
    public class ImageConfiguration
    {
        public static readonly ImageConfiguration DEFAULT_CONFIGURATION = new ImageConfiguration();
        public static readonly Size DEFAULT_RESOLUTION = new Size(640, 480);

        public bool ColorEnabled { get; private set; }
        public bool DepthEnabled { get; private set; }
        public bool ProjectionEnabled { get; private set; }

        public StreamConfiguration ColorStreamConfiguration { get; private set; }
        public StreamConfiguration DepthStreamConfiguration { get; private set; }

        protected ImageConfiguration()
        {
            ColorEnabled = true;
            DepthEnabled = true;
            ProjectionEnabled = true;
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
                }
                return this;
            }

            public Builder WithDepthDisabled()
            {
                imageFeature.DepthEnabled = false;
                return this;
            }

            public Builder WithColorStream(StreamConfiguration streamConfiguration)
            {
                imageFeature.ColorEnabled = true;
                imageFeature.ColorStreamConfiguration = streamConfiguration;
                return this;
            }

            public Builder WithDepthStream(StreamConfiguration streamConfiguration)
            {
                imageFeature.DepthEnabled = true;
                imageFeature.DepthStreamConfiguration = streamConfiguration;
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
                (!imageFeature.ColorEnabled || imageFeature.ColorStreamConfiguration != null).Check(
                    "When enabling color images, a color stream profile must be set");
                (!imageFeature.DepthEnabled || imageFeature.DepthStreamConfiguration != null).Check(
                    "When enabling depth images, a depth stream profile must be set");

                return imageFeature;
            }
        }
    }
}