using System.Collections.Generic;
using System.Linq;
using IntelRealSenseStart.Code.RealSense.Data.Properties;
using IntelRealSenseStart.Code.RealSense.Factory;
using IntelRealSenseStart.Code.RealSense.Helper;
using IntelRealSenseStart.Code.RealSense.Provider;

namespace IntelRealSenseStart.Code.RealSense.Component.Property
{
    public class AudioDevicePropertiesDeterminer : PropertiesComponent
    {
        private readonly RealSenseFactory factory;
        private readonly PXCMSession session;

        private AudioDevicePropertiesDeterminer(RealSenseFactory factory, NativeSense nativeSense)
        {
            this.factory = factory;
            session = nativeSense.Session;
        }

        public void UpdateProperties(RealSenseProperties.Builder realSenseProperties)
        {
            DetermineAudioProperties(realSenseProperties);
        }

        private void DetermineAudioProperties(RealSenseProperties.Builder realSenseProperties)
        {
            GetAudioDevices()
                .Select(GetDeviceProperties)
                .Do(audioDeviceProperties => realSenseProperties.WithAudioDeviceProperties(audioDeviceProperties));
        }

        private IEnumerable<PXCMAudioSource.DeviceInfo> GetAudioDevices()
        {
            var devices = new List<PXCMAudioSource.DeviceInfo>();
            var audioSource = session.CreateAudioSource();
            audioSource.ScanDevices();

            for (var i = 0;; i++)
            {
                PXCMAudioSource.DeviceInfo deviceInfo;
                if (audioSource.QueryDeviceInfo(i, out deviceInfo) < pxcmStatus.PXCM_STATUS_NO_ERROR)
                {
                    break;
                }

                devices.Add(deviceInfo);
            }

            audioSource.Dispose();
            return devices;
        }

        private AudioDeviceProperties.Builder GetDeviceProperties(PXCMAudioSource.DeviceInfo deviceInfo)
        {
            return factory.Data.Properties.AudioDevice()
                .WithDeviceName(deviceInfo.name)
                .WithDeviceInfo(deviceInfo);
        }

        public class Builder
        {
            private RealSenseFactory factory;
            private NativeSense nativeSense;

            public Builder WithFactory(RealSenseFactory factory)
            {
                this.factory = factory;
                return this;
            }

            public Builder WithNativeSense(NativeSense nativeSense)
            {
                this.nativeSense = nativeSense;
                return this;
            }

            public AudioDevicePropertiesDeterminer Build()
            {
                return new AudioDevicePropertiesDeterminer(factory, nativeSense);
            }
        }
    }
}