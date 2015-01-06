using System.Collections.Generic;
using System.Drawing;
using IntelRealSenseStart.Code.RealSense.Config.RealSense;

namespace IntelRealSenseStart.Code.RealSense.Config.Image
{
    public class ImageCreatorConfiguration
    {
        private Size resolution;

        private ImageBackground backgroundImage;

        private readonly List<ImageOverlay> overlays;
        
        private ImageCreatorConfiguration()
        {
            resolution = ImageConfiguration.DEFAULT_RESOLUTION;
            overlays = new List<ImageOverlay>();
        }

        public Size Resolution
        {
            get { return resolution;  }
        }

        public ImageBackground BackgroundImage
        {
            get { return backgroundImage; }
        }

        public List<ImageOverlay> Overlays
        {
            get { return overlays; }
        } 

        public class Builder
        {
            private readonly ImageCreatorConfiguration handImageConfiguration;

            public Builder()
            {
                handImageConfiguration = new ImageCreatorConfiguration();
            }

            public Builder WithBackgroundImage(ImageBackground backgroundImage)
            {
                handImageConfiguration.backgroundImage = backgroundImage;
                return this;
            }

            public Builder WithOverlay(ImageOverlay overlay)
            {
                handImageConfiguration.overlays.Add(overlay);
                return this;
            }

            public Builder WithResolution(Size size)
            {
                handImageConfiguration.resolution = size;
                return this;
            }

            public ImageCreatorConfiguration Build()
            {
                return handImageConfiguration;
            }
        }
    }
}