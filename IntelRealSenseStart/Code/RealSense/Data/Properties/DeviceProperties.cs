using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using IntelRealSenseStart.Code.RealSense.Exception;

namespace IntelRealSenseStart.Code.RealSense.Data.Properties
{
    public class DeviceProperties
    {
        private String deviceName;
        private PXCMCapture.DeviceInfo deviceInfo;

        private readonly List<StreamProperties> supportedColorStreamProperties; 
        private readonly List<StreamProperties> supportedDepthStreamProperties;

        private DeviceProperties()
        {
            supportedColorStreamProperties = new List<StreamProperties>();
            supportedDepthStreamProperties = new List<StreamProperties>();
        }

        public String DeviceName
        {
            get { return deviceName; }
        }

        public PXCMCapture.DeviceInfo DeviceInfo
        {
            get { return deviceInfo; }
        }

        public List<StreamProperties> SupportedColorStreamProperties
        {
            get { return supportedColorStreamProperties;  }
        }

        public StreamProperties ColorStreamPropertyWithResolution(Size resolution)
        {
            var streamProperties = supportedColorStreamProperties.Find(properties => properties.Resolution.Equals(resolution));
            if (streamProperties == null)
            {
                throw new RealSenseException("Color stream resolution is not supported by the device");
            }
            return streamProperties;
        }

        public List<StreamProperties> SupportedDepthStreamProperties
        {
            get { return supportedDepthStreamProperties; }
        }

        public StreamProperties DepthStreamPropertyWithResolution(Size resolution)
        {
            var streamProperties = supportedDepthStreamProperties.Find(properties => properties.Resolution.Equals(resolution));
            if (streamProperties == null)
            {
                throw new RealSenseException("Depth stream resolution is not supported by the device");
            }
            return streamProperties;
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

            public Builder WithDeviceInfo(PXCMCapture.DeviceInfo deviceInfo)
            {
                deviceProperties.deviceInfo = deviceInfo;
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