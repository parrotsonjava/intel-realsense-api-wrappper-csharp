using System;
using System.Collections.Generic;
using System.Linq;
using IntelRealSenseStart.Code.RealSense.Config.RealSense;
using IntelRealSenseStart.Code.RealSense.Data.Determiner;
using IntelRealSenseStart.Code.RealSense.Data.Properties;
using IntelRealSenseStart.Code.RealSense.Exception;
using IntelRealSenseStart.Code.RealSense.Helper;
using IntelRealSenseStart.Code.RealSense.Manager;
using IntelRealSenseStart.Code.RealSense.Provider;

namespace IntelRealSenseStart.Code.RealSense.Component.Determiner
{
    public class VideoDeviceDeterminerComponent : FrameDeterminerComponent
    {
        private readonly NativeSense nativeSense;
        private readonly RealSensePropertiesManager propertiesManager;
        private readonly RealSenseConfiguration configuration;

        private PXCMCapture.Device device;

        private VideoDeviceDeterminerComponent(NativeSense nativeSense,
            RealSensePropertiesManager propertiesManager, RealSenseConfiguration configuration)
        {
            this.nativeSense = nativeSense;
            this.propertiesManager = propertiesManager;
            this.configuration = configuration;
        }

        public void EnableFeatures()
        {
            var selectorFunction = configuration.Base.Video.SelectorFunction;
            if (selectorFunction != null)
            {
                var properties = propertiesManager.GetProperties();
                var deviceProperties = FindDeviceBy(properties.Video.Devices, selectorFunction);

                nativeSense.SenseManager.captureManager.FilterByDeviceInfo(deviceProperties.DeviceInfo);
            }
        }
        public VideoDeviceProperties FindDeviceBy(List<VideoDeviceProperties> devices, Func<VideoDeviceProperties, bool> selectorFunction)
        {
            var properties = devices.First(selectorFunction);
            if (properties == null)
            {
                throw new RealSenseException(String.Format("No camera with the specified selector is attached"));
            }
            return properties;
        }

        public void Configure()
        {
            PXCMCapture.DeviceInfo deviceInfo;
            var queryCaptureManager = nativeSense.SenseManager.QueryCaptureManager();
            device = queryCaptureManager.QueryDevice();

            if (device == null)
            {
                throw new RealSenseException("No device found for the selected device");
            }

            device.QueryDeviceInfo(out deviceInfo);

            if (deviceInfo == null)
            {
                throw new RealSenseException("No device info found for the selected device");
            }

            nativeSense.SenseManager.captureManager.device.SetDepthConfidenceThreshold(1);
            nativeSense.SenseManager.captureManager.device.SetMirrorMode(
                PXCMCapture.Device.MirrorMode.MIRROR_MODE_HORIZONTAL);
            nativeSense.SenseManager.captureManager.device.SetIVCAMFilterOption(6);
        }

        public void Stop()
        {
            // Nothing to doa
        }

        public void Process(DeterminerData.Builder determinerData)
        {
            determinerData.WithDevice(device);
        }

        public bool ShouldBeStarted
        {
            get { return configuration.HandsDetectionEnabled ||
                configuration.FaceDetectionEnabled ||
                configuration.Image.ColorEnabled ||
                configuration.Image.DepthEnabled; }
        }

        public class Builder
        {
            private NativeSense nativeSense;
            private RealSensePropertiesManager propertiesManager;
            private RealSenseConfiguration configuration;

            public Builder WithNativeSense(NativeSense nativeSense)
            {
                this.nativeSense = nativeSense;
                return this;
            }

            public Builder WithPropertiesManager(RealSensePropertiesManager propertiesManager)
            {
                this.propertiesManager = propertiesManager;
                return this;
            }

            public Builder WithConfiguration(RealSenseConfiguration configuration)
            {
                this.configuration = configuration;
                return this;
            }

            public VideoDeviceDeterminerComponent Build()
            {
                propertiesManager.Check(Preconditions.IsNotNull,
                    "The properties manager must be set to create the device component");
                nativeSense.Check(Preconditions.IsNotNull,
                    "The native RealSense must be set to create the device component");
                configuration.Check(Preconditions.IsNotNull,
                    "The RealSense configuration must be set to create the device component");

                return new VideoDeviceDeterminerComponent(nativeSense, propertiesManager, configuration);
            }
        }


    }
}