using IntelRealSenseStart.Code.RealSense.Config.RealSense;
using IntelRealSenseStart.Code.RealSense.Data.Properties;
using IntelRealSenseStart.Code.RealSense.Data.Status;
using IntelRealSenseStart.Code.RealSense.Event;
using IntelRealSenseStart.Code.RealSense.Factory;
using IntelRealSenseStart.Code.RealSense.Factory.Configuration;
using IntelRealSenseStart.Code.RealSense.Helper;
using IntelRealSenseStart.Code.RealSense.Manager;
using IntelRealSenseStart.Code.RealSense.Provider;

namespace IntelRealSenseStart.Code.RealSense
{
    public class RealSenseManager
    {
        public delegate void FrameEventListener(FrameEventArgs frameEventArgs);

        public event FrameEventListener Frame;

        public delegate RealSenseConfiguration.Builder FeatureConfigurer(DeterminerConfigurationFactory featureFactory);

        private readonly RealSenseFactory factory;
        private readonly RealSenseConfiguration configuration;

        private readonly RealSenseDeterminerManager componentsManager;
        private readonly NativeSense nativeSense;
        private readonly RealSensePropertiesManager propertiesManager;

        public static Builder Create()
        {
            return new Builder(new RealSenseFactory());
        }

        private RealSenseManager(RealSenseFactory factory, RealSenseConfiguration configuration,
            NativeSense nativeSense, RealSensePropertiesManager propertiesManager)
        {
            this.factory = factory;
            this.configuration = configuration;
            this.nativeSense = nativeSense;
            this.propertiesManager = propertiesManager;

            componentsManager = CreateComponentsManager();
            componentsManager.Frame += componentsManager_Frame;
        }
        private RealSenseDeterminerManager CreateComponentsManager()
        {
            return factory.Manager.ComponentsManager()
                .WithFactory(factory)
                .WithNativeSense(nativeSense)
                .WithPropertiesManager(propertiesManager)
                .WithConfiguration(configuration)
                .Build();
        }

        public void Start()
        {
            componentsManager.Start();
        }

        public void Stop()
        {
            componentsManager.Stop();
        }

        private void componentsManager_Frame(FrameEventArgs frameEventArgs)
        {
            if (Frame != null)
            {
                Frame.Invoke(frameEventArgs);
            }
        }

        public DeterminerStatus Status
        {
            get { return componentsManager.Status; }
        }

        public class Builder
        {
            private readonly NativeSense nativeSense;

            private readonly RealSensePropertiesManager propertiesManager;
            private readonly RealSenseProperties properties;

            private readonly RealSenseFactory factory;
            private RealSenseConfiguration configuration;

            public Builder(RealSenseFactory factory)
            {
                this.factory = factory;

                nativeSense = factory.Provider.NativeSense().WithFactory(factory.Native).Build();
                nativeSense.Initialize();

                propertiesManager = GetPropertiesManager();
                properties = DetermineProperties(propertiesManager);
            }

            private RealSensePropertiesManager GetPropertiesManager()
            {
                return factory.Manager.PropertiesManager()
                    .WithFactory(factory)
                    .WithNativeSense(nativeSense)
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

                return new RealSenseManager(factory, configuration, nativeSense, propertiesManager);
            }

            public RealSenseProperties Properties
            {
                get { return properties; }
            }
        }
    }
}