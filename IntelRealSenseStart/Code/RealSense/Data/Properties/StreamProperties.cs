using System.Drawing;

namespace IntelRealSenseStart.Code.RealSense.Data.Properties
{
    public class StreamProperties
    {
        private PXCMCapture.StreamType streamType;
        private PXCMImage.PixelFormat format;

        private Size resolution;
        private int frameRate;

        private StreamProperties()
        {
        }

        public PXCMCapture.StreamType StreamType
        {
            get { return streamType; }
        }

        public Size Resolution
        {
            get { return resolution; }
        }

        public int FrameRate
        {
            get { return frameRate; }
        }

        public PXCMImage.PixelFormat Format
        {
            get { return format;  }
        }

        public class Builder
        {
            private StreamProperties streamProperties;

            public Builder()
            {
                streamProperties = new StreamProperties();
            }

            public Builder WithStreamType(PXCMCapture.StreamType streamType)
            {
                streamProperties.streamType = streamType;
                return this;
            }

            public Builder WithResolution(Size resolution)
            {
                streamProperties.resolution = resolution;
                return this;
            }

            public Builder WithFrameRate(int frameRate)
            {
                streamProperties.frameRate = frameRate;
                return this;
            }
            public Builder WithFormat(PXCMImage.PixelFormat format)
            {
                streamProperties.format = format;
                return this;
            }
            
            public StreamProperties Build()
            {
                return streamProperties;
            }

   
        }
    }
}