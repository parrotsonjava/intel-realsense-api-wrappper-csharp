using System.Collections.Generic;

namespace IntelRealSenseStart.Code.RealSense.Data.Properties
{
    public class VideoProperties
    {
        private readonly List<VideoDeviceProperties> videoDeviceProperties;

        private VideoProperties()
        {
            videoDeviceProperties = new List<VideoDeviceProperties>();
        }

        public List<VideoDeviceProperties> Devices
        {
            get { return videoDeviceProperties; }
        }

        public class Builder
        {
            private readonly VideoProperties videoProperties;

            public Builder()
            {
                videoProperties = new VideoProperties();
            }

            public Builder WithVideoDevice(VideoDeviceProperties.Builder videoDeviceProperties)
            {
                videoProperties.videoDeviceProperties.Add(videoDeviceProperties.Build());
                return this;
            }

            public VideoProperties Build()
            {
                return videoProperties;
            }
        }
    }
}