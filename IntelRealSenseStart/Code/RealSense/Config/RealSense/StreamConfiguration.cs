using System.Drawing;
using IntelRealSenseStart.Code.RealSense.Data.Properties;

namespace IntelRealSenseStart.Code.RealSense.Config.RealSense
{
    public class StreamConfiguration
    {
        private Size resolution;
        
        public int frameRate;

        public Size Resolution
        {
            get { return resolution; }
        }

        public int FrameRate
        {
            get { return frameRate; }
        }

        public class Builder
        {
            private StreamConfiguration config;

            public Builder()
            {
                config = new StreamConfiguration();
            }

            public StreamConfiguration From(Size resolution, int frameRate)
            {
                config.resolution = resolution;
                config.frameRate = frameRate;
                return config;
            }

            public StreamConfiguration FromStreamProperties(StreamProperties streamProperties)
            {
                config.resolution = streamProperties.Resolution;
                config.frameRate = streamProperties.FrameRate;
                return config;
            }
        }

    }
}