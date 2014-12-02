using System.Collections.Generic;
using System.Drawing;

namespace IntelRealSenseStart.Code.RealSense.Config.HandsImage
{
    public class HandsImageConfiguration
    {
        private Size resolution;

        private HandsImageBackground backgroundImage;

        private List<HandsImageOverlay> overlays;
        
        private HandsImageConfiguration()
        {
            resolution = ImageConfiguration.DEFAULT_RESOLUTION;
            overlays = new List<HandsImageOverlay>();
        }

        public Size Resolution
        {
            get { return resolution;  }
        }

        public HandsImageBackground BackgroundImage
        {
            get { return backgroundImage; }
        }

        public List<HandsImageOverlay> Overlays
        {
            get { return overlays; }
        } 

        public class Builder
        {
            private HandsImageConfiguration handImageConfiguration;

            public Builder()
            {
                handImageConfiguration = new HandsImageConfiguration();
            }

            public Builder WithBackgroundImage(HandsImageBackground backgroundImage)
            {
                handImageConfiguration.backgroundImage = backgroundImage;
                return this;
            }

            public Builder WithOverlay(HandsImageOverlay overlay)
            {
                handImageConfiguration.overlays.Add(overlay);
                return this;
            }

            public Builder WithResolution(Size size)
            {
                handImageConfiguration.resolution = size;
                return this;
            }

            public HandsImageConfiguration Build()
            {
                return handImageConfiguration;
            }
        }
    }
}