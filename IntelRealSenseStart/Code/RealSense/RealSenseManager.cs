using System;
using IntelRealSenseStart.Code.RealSense.Config.RealSense;
using IntelRealSenseStart.Code.RealSense.Event;
using IntelRealSenseStart.Code.RealSense.Exception;
using IntelRealSenseStart.Code.RealSense.Factory;
using IntelRealSenseStart.Code.RealSense.Factory.Configuration;
using IntelRealSenseStart.Code.RealSense.Helper;
using IntelRealSenseStart.Code.RealSense.Manager;
using IntelRealSenseStart.Code.RealSense.Properties;

namespace IntelRealSenseStart.Code.RealSense
{
    public class RealSenseManager
    {
        public delegate void FrameEventListener(FrameEventArgs frameEventArgs);

        public event FrameEventListener Frame;

        public delegate Configuration.Builder FeatureConfigurer(DeterminerConfigurationFactory featureFactory);

        private readonly RealSenseFactory factory;
        private readonly Configuration configuration;
        private RealSenseProperties properties;

        private Boolean stopped = true;

        private RealSenseDeterminerManager componentsManager;
        private PXCMSenseManager manager;

        public static Builder Create()
        {
            return new Builder(new RealSenseFactory());
        }

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
            DetermineProperties();

            CreateComponentsManager();
            InitializeManager();

            componentsManager.Start();
        }

        private void DetermineProperties()
        {
            var propertiesDeterminer = factory.Manager.PropertiesManager()
                .WithFactory(factory)
                .WithSession(factory.Native.CurrentSession)
                .Build();
            properties = propertiesDeterminer.GetProperties();
        }

        private void CreateComponentsManager()
        {
            componentsManager = factory.Manager.ComponentsManager()
                .WithFactory(factory)
                .WithManager(manager)
                .WithConfiguration(configuration)
                .Build();

            componentsManager.Frame += componentsManager_Frame;
            componentsManager.EnableFeatures();
        }

        private void InitializeManager()
        {
            manager.Init();
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
            private readonly RealSenseFactory factory;
            private Configuration configuration;

            public Builder(RealSenseFactory factory)
            {
                this.factory = factory;
            }

            public Builder Configure(FeatureConfigurer configurer)
            {
                configuration = configurer.Invoke(factory.Configuration.Determiner).Build();
                return this;
            }

            public RealSenseManager Build()
            {
                configuration.CheckState(Preconditions.IsNotNull,
                    "The RealSense manager must be configured before using it");

                return new RealSenseManager(factory, configuration);
            }
        }
    }
}