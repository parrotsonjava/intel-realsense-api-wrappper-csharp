using System.Collections.Generic;

namespace IntelRealSenseStart.Code.RealSense.Data.Properties
{
    public class RealSenseProperties
    {
        public static readonly RealSenseProperties DEFAULT_PROPERTIES;

        private readonly List<DeviceProperties> deviceProperties;

        static RealSenseProperties()
        {
            DEFAULT_PROPERTIES = new RealSenseProperties();
        }

        private RealSenseProperties()
        {
            deviceProperties = new List<DeviceProperties>();
        }

        public List<DeviceProperties> Devices
        {
            get { return deviceProperties; }
        }

        public class Builder
        {
            private readonly RealSenseProperties realSenseProperties;
            
            public Builder()
            {
                realSenseProperties = new RealSenseProperties();   
            }

            public Builder WithDeviceProperties(DeviceProperties.Builder deviceProperties)
            {
                realSenseProperties.Devices.Add(deviceProperties.Build());
                return this;
            }

            public RealSenseProperties Build()
            {
                return realSenseProperties;
            }
        }
    }
}