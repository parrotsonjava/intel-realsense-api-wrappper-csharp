using System;
using IntelRealSenseStart.Code.RealSense.Config.RealSense;
using IntelRealSenseStart.Code.RealSense.Data.Properties;
using IntelRealSenseStart.Code.RealSense.Data.Status;
using IntelRealSenseStart.Code.RealSense.Event;
using IntelRealSenseStart.Code.RealSense.Factory;
using IntelRealSenseStart.Code.RealSense.Factory.Configuration;
using IntelRealSenseStart.Code.RealSense.Helper;
using IntelRealSenseStart.Code.RealSense.Manager;
using IntelRealSenseStart.Code.RealSense.Manager.Builder;
using IntelRealSenseStart.Code.RealSense.Provider;

namespace IntelRealSenseStart.Code.RealSense
{
    public class RealSenseManager
    {
        public event ReadyEventListener Ready;
        public event FrameEventListener Frame;
        public event SpeechRecognitionEventListener SpeechRecognized;
        public event SpeechOutputStatusListener SpeechOutput;

        public delegate RealSenseConfiguration.Builder FeatureConfigurer(DeterminerConfigurationFactory featureFactory);

        private readonly RealSenseComponentsManager componentsManager;

        public static Builder Create()
        {
            return new Builder(new RealSenseFactory());
        }

        private RealSenseManager(RealSenseFactory factory, RealSenseConfiguration configuration,
            NativeSense nativeSense, RealSenseComponentsBuilder componentsBuilder)
        {
            componentsManager = factory.Manager.Components()
                .WithFactory(factory)
                .WithNativeSense(nativeSense)
                .WithConfiguration(configuration)
                .WithComponentsBuilder(componentsBuilder)
                .Build();

            componentsManager.OnReady(componentsManager_Ready);
            componentsManager.OnFrame(componentsManager_Frame);
            componentsManager.OnSpeechRecognized(componentsManager_SpeechRecognized);
            componentsManager.OnSpeechOutput(componentsManager_SpeechOutput);
        }



        public void Start()
        {
            componentsManager.Start();
        }

        public void Stop()
        {
            componentsManager.Stop();
        }

        public void Speak(String sentence)
        {
            componentsManager.Speak(sentence);
        }

        public void ConfigureRecognition(SpeechRecognitionConfiguration configuration)
        {
            componentsManager.ConfigureRecognition(configuration);
        }

        public void StartRecognition()
        {
            componentsManager.StartRecognition();
        }

        public void StopRecognition()
        {
            componentsManager.StopRecognition();
        }

        private void componentsManager_Frame(FrameEventArgs frameEventArgs)
        {
            if (Frame != null)
            {
                Frame.Invoke(frameEventArgs);
            }
        }

        private void componentsManager_SpeechRecognized(SpeechRecognitionEventArgs eventArgs)
        {
            if (SpeechRecognized != null)
            {
                SpeechRecognized.Invoke(eventArgs);
            }
        }

        private void componentsManager_SpeechOutput(SpeechOutputStatusEventArgs eventArgs)
        {
            if (SpeechRecognized != null)
            {
                SpeechOutput.Invoke(eventArgs);
            }
        }

        private void componentsManager_Ready()
        {
            if (Ready != null)
            {
                Ready.Invoke();
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

                propertiesManager = CreatePropertiesManager();
                properties = DetermineProperties(propertiesManager);
            }

            private RealSensePropertiesManager CreatePropertiesManager()
            {
                var componentsBuilder = factory.Manager.PropertyComponentsBuilder()
                    .WithFactory(factory)
                    .WithNativeSense(nativeSense)
                    .Build();

                return factory.Manager.PropertiesManager()
                    .WithFactory(factory)
                    .WithComponentsBuilder(componentsBuilder)
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

                var componentsBuilder = factory.Manager.ComponentsBuilder()
                    .WithFactory(factory)
                    .WithNativeSense(nativeSense)
                    .WithConfiguration(configuration)
                    .WithPropertiesManager(propertiesManager)
                    .Build();

                return new RealSenseManager(factory, configuration, nativeSense, componentsBuilder);
            }

            public RealSenseProperties Properties
            {
                get { return properties; }
            }
        }
    }
}