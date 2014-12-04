using System.Collections.Generic;

namespace IntelRealSenseStart.Code.RealSense.Properties
{
    public class DeviceProperties
    {
        public static readonly DeviceProperties DEFAULT_PROPERTIES;
        
        private List<StreamProperties> supportedColorStreamProperties; 
        private List<StreamProperties> supportedDepthStreamProperties;

        static DeviceProperties()
        {
            DEFAULT_PROPERTIES = new DeviceProperties();
        }

        public DeviceProperties()
        {
            supportedColorStreamProperties = new List<StreamProperties>();
            supportedDepthStreamProperties = new List<StreamProperties>();
        }

        public List<StreamProperties> SupportedColorStreamProperties
        {
            get { return supportedColorStreamProperties;  }
        }

        public List<StreamProperties> SupportedDepthStreamProperties
        {
            get { return supportedColorStreamProperties; }
        }

        public class Builder
        {
            private DeviceProperties deviceProperties;

            public Builder()
            {
                deviceProperties = new DeviceProperties();
            }

            public Builder WithSupportedColorStream(int width, int height)
            {
                return this;
            }

            public Builder WithSupportedDepthStream(int width, int height)
            {
                return this;
            }

            public DeviceProperties Build()
            {
                return deviceProperties;
            }
        }
    }
}