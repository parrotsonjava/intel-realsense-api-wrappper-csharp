using System;
using System.Collections.Generic;
using System.Linq;
using IntelRealSenseStart.Code.RealSense.Exception;

namespace IntelRealSenseStart.Code.RealSense.Data.Properties
{
    public class VideoProperties
    {
        private readonly List<VideoDeviceProperties> videoDeviceProperties;

        private VideoProperties()
        {
            videoDeviceProperties = new List<VideoDeviceProperties>();
        }

        public VideoDeviceProperties FindDeviceBy(Func<VideoDeviceProperties, bool> selectorFunction)
        {
            var properties = Devices.First(selectorFunction);
            if (properties == null)
            {
                throw new RealSenseException(String.Format("No camera with the specified selector is attached"));
            }
            return properties;
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