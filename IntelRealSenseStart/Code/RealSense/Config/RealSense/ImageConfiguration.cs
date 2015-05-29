using System.Drawing;
using IntelRealSenseStart.Code.RealSense.Helper;

namespace IntelRealSenseStart.Code.RealSense.Config.RealSense
{
    public class ImageConfiguration
    {
        public static readonly ImageConfiguration DEFAULT_CONFIGURATION = new ImageConfiguration();
        public static readonly Size DEFAULT_RESOLUTION = new Size(640, 480);

        protected ImageConfiguration()
        {
            ColorEnabled = false;
            DepthEnabled = false;
            ProjectionEnabled = false;
        }

        public bool ColorEnabled { get; private set; }
        public bool DepthEnabled { get; private set; }
        public bool ProjectionEnabled { get; private set; }

        public StreamConfiguration ColorStreamConfiguration { get; private set; }
        public StreamConfiguration DepthStreamConfiguration { get; private set; }

        public class Builder
        {
            private readonly ImageConfiguration imageConfiguration;

            public Builder()
            {
                imageConfiguration = new ImageConfiguration();
            }

            public Builder WithColorEnabled()
            {
                if (!imageConfiguration.ColorEnabled)
                {
                    imageConfiguration.ColorEnabled = true;
                }
                return this;
            }

            public Builder WithColorDisabled()
            {
                imageConfiguration.ColorEnabled = false;
                return this;
            }

            public Builder WithDepthEnabled()
            {
                if (!imageConfiguration.ColorEnabled)
                {
                    imageConfiguration.DepthEnabled = true;
                }
                return this;
            }

            public Builder WithDepthDisabled()
            {
                imageConfiguration.DepthEnabled = false;
                return this;
            }

            public Builder WithColorStream(StreamConfiguration streamConfiguration)
            {
                imageConfiguration.ColorEnabled = true;
                imageConfiguration.ColorStreamConfiguration = streamConfiguration;
                return this;
            }

            public Builder WithDepthStream(StreamConfiguration streamConfiguration)
            {
                imageConfiguration.DepthEnabled = true;
                imageConfiguration.DepthStreamConfiguration = streamConfiguration;
                return this;
            }

            public Builder WithProjectionEnabled()
            {
                WithColorEnabled();
                WithDepthEnabled();

                imageConfiguration.ProjectionEnabled = true;
                return this;
            }

            public Builder WithProjectionDisabled()
            {
                imageConfiguration.ProjectionEnabled = false;
                return this;
            }

            public ImageConfiguration Build()
            {
                (!imageConfiguration.ColorEnabled || imageConfiguration.ColorStreamConfiguration != null).Check(
                    "When enabling color images, a color stream profile must be set");
                (!imageConfiguration.DepthEnabled || imageConfiguration.DepthStreamConfiguration != null).Check(
                    "When enabling depth images, a depth stream profile must be set");

                return imageConfiguration;
            }
        }
    }
}