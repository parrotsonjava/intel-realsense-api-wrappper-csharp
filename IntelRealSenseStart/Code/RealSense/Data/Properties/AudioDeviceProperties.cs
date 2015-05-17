using System;

namespace IntelRealSenseStart.Code.RealSense.Data.Properties
{
    public class AudioDeviceProperties
    {
        private String deviceName;
        private PXCMAudioSource.DeviceInfo deviceInfo;

        private AudioDeviceProperties()
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
            private readonly AudioDeviceProperties audioDeviceProperties;

            public Builder()
            {
                audioDeviceProperties = new AudioDeviceProperties();
            }

            public Builder WithDeviceName(string deviceName)
            {
                audioDeviceProperties.deviceName = deviceName;
                return this;
            }

            public Builder WithDeviceInfo(PXCMAudioSource.DeviceInfo deviceInfo)
            {
                audioDeviceProperties.deviceInfo = deviceInfo;
                return this;
            }

            public AudioDeviceProperties Build()
            {
                return audioDeviceProperties;
            }
        }
    }
}