using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using IntelRealSenseStart.Code.RealSense.Data.Properties;
using IntelRealSenseStart.Code.RealSense.Factory;

namespace IntelRealSenseStart.Code.RealSense.Component.Property
{
    public class DevicePropertiesDeterminer : PropertiesComponent
    {
        private readonly RealSenseFactory factory;
        private readonly PXCMSession session;

        private DevicePropertiesDeterminer(RealSenseFactory factory, PXCMSession session)
        {
            this.factory = factory;
            this.session = session;
        }

        public void UpdateProperties(RealSenseProperties.Builder realSenseProperties)
        {
            DetermineVideoDeviceProperties(realSenseProperties);
        }

        private void DetermineVideoDeviceProperties(RealSenseProperties.Builder realSenseProperties)
        {
            var videoDeviceDescription = CreateVideoDeviceDescription();
            for (int i = 0;; i++)
            {
                PXCMSession.ImplDesc deviceDescription;
                PXCMCapture deviceCapture;
                PXCMCapture.DeviceInfo deviceInfo;
                PXCMCapture.Device device;

                if (!GetDeviceData(videoDeviceDescription, i, out deviceDescription, out deviceCapture,
                    out deviceInfo, out device))
                {
                    break;
                }

                realSenseProperties.WithDeviceProperties(GetDevicePropertiesFrom(deviceInfo, device));

                deviceCapture.Dispose();
            }
        }

        private PXCMSession.ImplDesc CreateVideoDeviceDescription()
        {
            var videoDeviceDescription = new PXCMSession.ImplDesc();
            videoDeviceDescription.group = PXCMSession.ImplGroup.IMPL_GROUP_SENSOR;
            videoDeviceDescription.subgroup = PXCMSession.ImplSubgroup.IMPL_SUBGROUP_VIDEO_CAPTURE;
            return videoDeviceDescription;
        }

        private bool GetDeviceData(PXCMSession.ImplDesc videoDeviceDescription, int deviceIndex,
            out PXCMSession.ImplDesc deviceDescription, out PXCMCapture deviceCapture,
            out PXCMCapture.DeviceInfo deviceInfo, out PXCMCapture.Device device)
        {
            deviceCapture = null;
            deviceInfo = null;
            device = null;

            if (session.QueryImpl(videoDeviceDescription, deviceIndex, out deviceDescription) <
                pxcmStatus.PXCM_STATUS_NO_ERROR)
            {
                return false;
            }

            session.CreateImpl(deviceDescription, out deviceCapture);
            if (deviceCapture == null || deviceCapture.QueryDeviceInfo(0, out deviceInfo) < pxcmStatus.PXCM_STATUS_NO_ERROR)
            {
                return false;
            }
            device = deviceCapture.CreateDevice(deviceInfo.didx);
            return device != null;
        }

        private DeviceProperties.Builder GetDevicePropertiesFrom(PXCMCapture.DeviceInfo deviceInfo, PXCMCapture.Device device)
        {
            return factory.Data.Properties.Device()
                .WithDeviceName(deviceInfo.name)
                .WithDeviceInfo(deviceInfo)
                .WithSupportedColorStreams(GetStreamsFor(device, deviceInfo, PXCMCapture.StreamType.STREAM_TYPE_COLOR))
                .WithSupportedDepthStreams(GetStreamsFor(device, deviceInfo, PXCMCapture.StreamType.STREAM_TYPE_DEPTH));
        }

        private IEnumerable<StreamProperties.Builder> GetStreamsFor(PXCMCapture.Device device,
            PXCMCapture.DeviceInfo deviceInfo, PXCMCapture.StreamType streamType)
        {
            return GetStreamProperties(GetAllowedStreamsFor(device, deviceInfo, streamType), streamType);
        }

        private IEnumerable<PXCMCapture.Device.StreamProfile> GetAllowedStreamsFor(PXCMCapture.Device device,
            PXCMCapture.DeviceInfo deviceInfo, PXCMCapture.StreamType streamType)
        {
            var streamProfiles = new List<PXCMCapture.Device.StreamProfile>();
            if (((int) deviceInfo.streams & (int) streamType) == 0)
            {
                return streamProfiles;
            }

            int numberOfStreamProfiles = device.QueryStreamProfileSetNum(streamType);
            for (int i = 0; i < numberOfStreamProfiles; i++)
            {
                PXCMCapture.Device.StreamProfileSet streamProfileSet;
                if (device.QueryStreamProfileSet(streamType, i, out streamProfileSet) < pxcmStatus.PXCM_STATUS_NO_ERROR)
                {
                    continue;
                }
                streamProfiles.Add(streamProfileSet[streamType]);
            }

            return streamProfiles;
        }

        private IEnumerable<StreamProperties.Builder> GetStreamProperties(
            IEnumerable<PXCMCapture.Device.StreamProfile> streamProfiles, PXCMCapture.StreamType streamType)
        {
            return streamProfiles.Select(streamProfile =>
                factory.Data.Properties.Stream()
                    .WithStreamType(streamType)
                    .WithResolution(new Size(streamProfile.imageInfo.width, streamProfile.imageInfo.height))
                    .WithFrameRate((int) streamProfile.frameRate.min)
                    .WithFormat(streamProfile.imageInfo.format)
                );
        }

        public class Builder
        {
            private RealSenseFactory factory;
            private PXCMSession session;

            public Builder WithFactory(RealSenseFactory factory)
            {
                this.factory = factory;
                return this;
            }

            public Builder WithSession(PXCMSession session)
            {
                this.session = session;
                return this;
            }

            public DevicePropertiesDeterminer Build()
            {
                return new DevicePropertiesDeterminer(factory, session);
            }
        }
    }
}