using System;
using IntelRealSenseStart.Code.RealSense.Config.RealSense;
using IntelRealSenseStart.Code.RealSense.Data.Properties;
using IntelRealSenseStart.Code.RealSense.Event;
using IntelRealSenseStart.Code.RealSense.Exception;
using IntelRealSenseStart.Code.RealSense.Factory;
using IntelRealSenseStart.Code.RealSense.Factory.Configuration;
using IntelRealSenseStart.Code.RealSense.Helper;
using IntelRealSenseStart.Code.RealSense.Manager;

namespace IntelRealSenseStart.Code.RealSense
{
    public class RealSenseManager
    {
        public delegate void FrameEventListener(FrameEventArgs frameEventArgs);

        public event FrameEventListener Frame;

        public delegate RealSenseConfiguration.Builder FeatureConfigurer(DeterminerConfigurationFactory featureFactory);

        private readonly RealSenseFactory factory;
        private readonly RealSenseConfiguration configuration;

        private Boolean stopped = true;

        private RealSenseDeterminerManager componentsManager;
        private readonly PXCMSenseManager manager;
        private readonly RealSensePropertiesManager propertiesManager;

        public static Builder Create()
        {
            return new Builder(new RealSenseFactory());
        }

        private RealSenseManager(RealSenseFactory factory, RealSenseConfiguration configuration,
            PXCMSenseManager manager, RealSensePropertiesManager propertiesManager)
        {
            this.factory = factory;
            this.configuration = configuration;
            this.manager = manager;
            this.propertiesManager = propertiesManager;
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
            CreateComponentsManager();

            manager.Init();
            componentsManager.Start();
        }

        private void CreateComponentsManager()
        {
            componentsManager = factory.Manager.ComponentsManager()
                .WithFactory(factory)
                .WithManager(manager)
                .WithPropertiesManager(propertiesManager)
                .WithConfiguration(configuration)
                .Build();

            componentsManager.Frame += componentsManager_Frame;
            componentsManager.EnableFeatures();
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
            private readonly PXCMSenseManager manager;

            private readonly RealSensePropertiesManager propertiesManager;
            private readonly RealSenseProperties properties;

            private readonly RealSenseFactory factory;
            private RealSenseConfiguration configuration;

            public Builder(RealSenseFactory factory)
            {
                this.factory = factory;
                
                propertiesManager = GetPropertiesManager();
                manager = factory.Native.CreateSenseManager();
                properties = DetermineProperties(propertiesManager);
            }

            private RealSensePropertiesManager GetPropertiesManager()
            {
                return factory.Manager.PropertiesManager()
                    .WithFactory(factory)
                    .WithSession(factory.Native.CurrentSession)
                    .Build();
            }

            private RealSenseProperties DetermineProperties(RealSensePropertiesManager propertiesManager)
            {
                return propertiesManager.GetProperties();
            }

            public Builder Configure(FeatureConfigurer configurer)
            {
                configuration = configurer.Invoke(factory.Configuration.Determiner).Build();
                return this;
            }

            public RealSenseManager Build()
            {
                configuration.Check(Preconditions.IsNotNull,
                    "The RealSense manager must be configured before using it");

                return new RealSenseManager(factory, configuration, manager, propertiesManager);
            }

            public RealSenseProperties Properties
            {
                get { return properties; }
            }
        }
    }
}