namespace IntelRealSenseStart.Code.RealSense.Properties
{
    public class RealSenseProperties
    {
        public static readonly RealSenseProperties DEFAULT_PROPERTIES;

        private DeviceProperties deviceProperties;

        static RealSenseProperties()
        {
            DEFAULT_PROPERTIES = new RealSenseProperties();
        }

        private RealSenseProperties()
        {
            deviceProperties = DeviceProperties.DEFAULT_PROPERTIES;
        }

        public DeviceProperties Device
        {
            get { return deviceProperties; }
        }

        public class Builder
        {
            private RealSenseProperties realSenseProperties;
            
            public Builder()
            {
                realSenseProperties = new RealSenseProperties();   
            }

            public Builder WithDeviceProperties(DeviceProperties.Builder deviceProperties)
            {
                realSenseProperties.deviceProperties = deviceProperties.Build();
                return this;
            }

            public RealSenseProperties Build()
            {
                return realSenseProperties;
            }
        }
    }
}