using System;
using System.Collections.Generic;
using System.Linq;

namespace IntelRealSenseStart.Code.RealSense.Data.Properties
{
    public class DeviceProperties
    {
        private String deviceName;

        private readonly List<StreamProperties> supportedColorStreamProperties; 
        private readonly List<StreamProperties> supportedDepthStreamProperties;

        public DeviceProperties()
        {
            supportedColorStreamProperties = new List<StreamProperties>();
            supportedDepthStreamProperties = new List<StreamProperties>();
        }

        public String DeviceName
        {
            get { return deviceName; }
        }

        public List<StreamProperties> SupportedColorStreamProperties
        {
            get { return supportedColorStreamProperties;  }
        }

        public List<StreamProperties> SupportedDepthStreamProperties
        {
            get { return supportedDepthStreamProperties; }
        }

        public class Builder
        {
            private readonly DeviceProperties deviceProperties;

            public Builder()
            {
                deviceProperties = new DeviceProperties();
            }

            public Builder WithDeviceName(String deviceName)
            {
                deviceProperties.deviceName = deviceName;
                return this;
            }

            public Builder WithSupportedColorStreams(IEnumerable<StreamProperties.Builder> streamProperties)
            {
                deviceProperties.supportedColorStreamProperties.AddRange(streamProperties.Select(builder => builder.Build()));
                return this;
            }

            public Builder WithSupportedDepthStreams(IEnumerable<StreamProperties.Builder> streamProperties)
            {
                deviceProperties.supportedDepthStreamProperties.AddRange(streamProperties.Select(builder => builder.Build()));
                return this;
            }

            public DeviceProperties Build()
            {
                return deviceProperties;
            }
        }
    }
}