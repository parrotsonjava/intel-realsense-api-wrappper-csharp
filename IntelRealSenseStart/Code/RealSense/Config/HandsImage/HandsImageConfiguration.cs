using System.Collections.Generic;

namespace IntelRealSenseStart.Code.RealSense.Config.HandsImage
{
    public class HandsImageConfiguration
    {
        private HandsImageBackground backgroundImage;

        private List<HandsImageOverlay> overlays;

        private HandsImageConfiguration()
        {
            overlays = new List<HandsImageOverlay>();
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

            public HandsImageConfiguration Build()
            {
                return handImageConfiguration;
            }
        }
    }
}