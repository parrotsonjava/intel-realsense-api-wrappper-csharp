using System.Collections.Generic;
using IntelRealSenseStart.Code.RealSense.Data.Properties;
using IntelRealSenseStart.Code.RealSense.Factory;
using IntelRealSenseStart.Code.RealSense.Helper;

namespace IntelRealSenseStart.Code.RealSense.Component.Property
{
    public class VideoPropertiesDeterminer : PropertiesComponent<RealSenseProperties.Builder>
    {
        private RealSenseFactory factory;
        private readonly List<PropertiesComponent<VideoProperties.Builder>> videoPropertiesComponents; 

        private VideoPropertiesDeterminer()
        {
            videoPropertiesComponents = new List<PropertiesComponent<VideoProperties.Builder>>();
        }

        public void UpdateProperties(RealSenseProperties.Builder realSenseProperties)
        {
            var videoProperties = factory.Data.Properties.Video();
            videoPropertiesComponents.Do(audioPropertiesComponent => audioPropertiesComponent.UpdateProperties(videoProperties));
            realSenseProperties.WithVideoProperties(videoProperties);
        }

        public class Builder
        {
            private readonly VideoPropertiesDeterminer videoPropertiesDeterminer;

            public Builder()
            {
                videoPropertiesDeterminer = new VideoPropertiesDeterminer();
            }

            public Builder WithFactory(RealSenseFactory factory)
            {
                videoPropertiesDeterminer.factory = factory;
                return this;
            }

            public Builder WithVideoPropertiesComponent(PropertiesComponent<VideoProperties.Builder> videoPropertiesComponent)
            {
                videoPropertiesDeterminer.videoPropertiesComponents.Add(videoPropertiesComponent);
                return this;
            }

            public VideoPropertiesDeterminer Build()
            {
                videoPropertiesDeterminer.factory.Check(Preconditions.IsNotNull,
                    "The factory must be set in order to create the video properties determiner");

                return videoPropertiesDeterminer;
            }
        }
    }
}