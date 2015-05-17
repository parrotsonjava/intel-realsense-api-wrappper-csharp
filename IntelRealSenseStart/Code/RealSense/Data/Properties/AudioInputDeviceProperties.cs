using System;

namespace IntelRealSenseStart.Code.RealSense.Data.Properties
{
    public class AudioInputDeviceProperties
    {
        private String deviceName;
        private PXCMAudioSource.DeviceInfo deviceInfo;

        private AudioInputDeviceProperties()
        {
        }

        public String DeviceName
        {
            get { return deviceName; }
        }

        public PXCMAudioSource.DeviceInfo DeviceInfo
        {
            get { return deviceInfo; }
        }

        public class Builder
        {
            private readonly AudioInputDeviceProperties audioInputDeviceProperties;

            public Builder()
            {
                audioInputDeviceProperties = new AudioInputDeviceProperties();
            }

            public Builder WithDeviceName(string deviceName)
            {
                audioInputDeviceProperties.deviceName = deviceName;
                return this;
            }

            public Builder WithDeviceInfo(PXCMAudioSource.DeviceInfo deviceInfo)
            {
                audioInputDeviceProperties.deviceInfo = deviceInfo;
                return this;
            }

            public AudioInputDeviceProperties Build()
            {
                return audioInputDeviceProperties;
            }
        }
    }
}