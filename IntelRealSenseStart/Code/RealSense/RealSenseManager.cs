using System;
using IntelRealSenseStart.Code.RealSense.Config;
using IntelRealSenseStart.Code.RealSense.Event;
using IntelRealSenseStart.Code.RealSense.Exception;
using IntelRealSenseStart.Code.RealSense.Factory;

namespace IntelRealSenseStart.Code.RealSense
{
    public class RealSenseManager
    {
        public delegate void FrameEventListener(FrameEventArgs frameEventArgs);
        public event FrameEventListener Frame;

        public delegate Configuration.Builder FeatureConfigurer(ConfigurationFactory featureFactory);

        private readonly RealSenseFactory factory;
        private readonly Configuration configuration;

        private Boolean stopped = true;

        private RealSenseComponentsManager componentsManager;
        private PXCMSenseManager manager;

        private RealSenseManager(RealSenseFactory factory, Configuration configuration)
        {
            this.factory = factory;
            this.configuration = configuration;
        }

        public void Start()
        {
            if (!stopped)
            {
                throw new RealSenseException("RealSense manager is already running");
            }

            stopped = false;
            StartRealSense();
        }

        private void StartRealSense()
        {
            manager = factory.Native.CreateSenseManager();
            CreateComponentsManager();

            InitializeManager();
            ConfigureDevice();

            componentsManager.Start();
        }

        public static Builder Create()
        {
            return new Builder(new RealSenseFactory());
        }

        private void CreateComponentsManager()
        {
            componentsManager = factory.Components.ComponentsManager().Build(factory, manager, configuration);
            componentsManager.Frame += componentsManager_Frame;
            
            componentsManager.EnableFeatures();
        }
        
        private void InitializeManager()
        {
            manager.Init();
        }

        private void ConfigureDevice()
        {
            PXCMCapture.DeviceInfo deviceInfo;
            manager.QueryCaptureManager().QueryDevice().QueryDeviceInfo(out deviceInfo);

            if (deviceInfo == null || deviceInfo.model != PXCMCapture.DeviceModel.DEVICE_MODEL_IVCAM)
            {
                throw new RealSenseException("No device info found");
            }

            manager.captureManager.device.SetDepthConfidenceThreshold(1);
            manager.captureManager.device.SetMirrorMode(PXCMCapture.Device.MirrorMode.MIRROR_MODE_HORIZONTAL);
            manager.captureManager.device.SetIVCAMFilterOption(6);
        }
        public void Stop()
        {
            if (stopped)
            {
                return;
            }

            stopped = true;
            StopRealSense();
        }

        private void StopRealSense()
        {
            componentsManager.Stop();
            manager.Close();
        }

        private void componentsManager_Frame(FrameEventArgs frameEventArgs)
        {
            if (Frame != null)
            {
                Frame.Invoke(frameEventArgs);
            }
        }

        public bool Started
        {
            get { return !stopped; }
        }

        public class Builder
        {
            private RealSenseFactory factory;

            private Configuration configuration;

            public Builder(RealSenseFactory factory)
            {
                this.factory = factory;
            }

            public Builder Configure(FeatureConfigurer configurer)
            {
                configuration = configurer.Invoke(factory.Configuration).Build();
                return this;
            }

            public RealSenseManager Build()
            {
                if (configuration == null)
                {
                    throw new RealSenseException("The RealSense manager must be configured before using it");
                }
                return new RealSenseManager(factory, configuration);
            }
        }
    }
}