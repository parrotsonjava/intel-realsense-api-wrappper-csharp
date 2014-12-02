using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelRealSenseStart.Code.RealSense.Config
{
    public class VideoDeviceConfiguration
    {
        public class Builder
        {
            private readonly VideoDeviceConfiguration videoDeviceConfiguration;

            public Builder()
            {
                videoDeviceConfiguration = new VideoDeviceConfiguration();
            }

            public VideoDeviceConfiguration Build()
            {
                return videoDeviceConfiguration;
            }
        }
    }
}