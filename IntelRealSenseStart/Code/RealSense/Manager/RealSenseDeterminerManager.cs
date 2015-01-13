using System;
using System.Linq;
using System.Threading;
using IntelRealSenseStart.Code.RealSense.Component.Creator;
using IntelRealSenseStart.Code.RealSense.Component.Determiner;
using IntelRealSenseStart.Code.RealSense.Config.RealSense;
using IntelRealSenseStart.Code.RealSense.Data.Determiner;
using IntelRealSenseStart.Code.RealSense.Data.Status;
using IntelRealSenseStart.Code.RealSense.Event;
using IntelRealSenseStart.Code.RealSense.Exception;
using IntelRealSenseStart.Code.RealSense.Factory;
using IntelRealSenseStart.Code.RealSense.Helper;
using IntelRealSenseStart.Code.RealSense.Provider;

namespace IntelRealSenseStart.Code.RealSense.Manager
{
    public class RealSenseDeterminerManager
    {
        public delegate void FrameEventListener(FrameEventArgs frameEventArgs);

        public event FrameEventListener Frame;

        private readonly DeterminerComponent[] components;
        private readonly OverallImageCreator overallImageCreator;

        private readonly RealSenseFactory factory;
        private readonly RealSensePropertiesManager propertiesManager;
        private readonly RealSenseConfiguration realSenseConfiguration;

        private readonly SenseManagerProvider.Builder senseManagerProviderBuilder;
        private readonly SenseManagerProvider senseManagerProvider;

        private Thread determinerThread;
        private Thread reconnectThread;

        private volatile DeterminerStatus determinerStatus = DeterminerStatus.STOPPED;

        private RealSenseDeterminerManager(RealSenseFactory factory, PXCMSenseManager manager,
            RealSensePropertiesManager propertiesManager, RealSenseConfiguration realSenseConfiguration)
        {
            this.factory = factory;
            this.propertiesManager = propertiesManager;
            this.realSenseConfiguration = realSenseConfiguration;

            senseManagerProviderBuilder = factory.Provider.SenseManager().WithSenseManager(manager);
            senseManagerProvider = senseManagerProviderBuilder.Build();

            components = GetComponents().Where(component => component.ShouldBeStarted).ToArray();
            overallImageCreator = GetImageCreator(realSenseConfiguration);

            reconnectThread = new Thread(StartReconnect);
        }

        private DeterminerComponent[] GetComponents()
        {
            var deviceComponent = factory.Components.Determiner.Device()
                .WithPropertiesManager(propertiesManager)
                .WithManager(senseManagerProvider)
                .WithConfiguration(realSenseConfiguration)
                .Build();
            var handsComponent = factory.Components.Determiner.Hands()
                .WithFactory(factory)
                .WithManager(senseManagerProvider)
                .WithConfiguration(realSenseConfiguration)
                .Build();
            var faceComponent = factory.Components.Determiner.Face()
                .WithFactory(factory)
                .WithManager(senseManagerProvider)
                .WithConfiguration(realSenseConfiguration)
                .Build();
            var pictureComponent = factory.Components.Determiner.Image()
                .WithFactory(factory)
                .WithManager(senseManagerProvider)
                .WithConfiguration(realSenseConfiguration)
                .Build();

            return new DeterminerComponent[] {handsComponent, faceComponent, pictureComponent, deviceComponent};
        }

        private OverallImageCreator GetImageCreator(RealSenseConfiguration realSenseConfiguration)
        {
            var basicImageCreator = factory.Components.Creator.BasicImageCreator()
                .WithRealSenseConfiguration(realSenseConfiguration)
                .Build();
            var handsImageCreator = factory.Components.Creator.HandsImageCreator()
                .WithRealSenseConfiguration(realSenseConfiguration)
                .Build();
            var faceImageCreator = factory.Components.Creator.FaceImageCreator()
                .WithRealSenseConfiguration(realSenseConfiguration)
                .Build();

            return factory.Components.Creator.OverallImageCreator()
                .WithImageCreators(new ImageCreator[] {basicImageCreator, handsImageCreator, faceImageCreator})
                .Build();
        }

        public DeterminerStatus Status
        {
            get { return determinerStatus; }
        }

        public void Start()
        {
            if (determinerStatus != DeterminerStatus.STOPPED)
            {
                throw new RealSenseException("Components cannot be started because it is already running");
            }

            StartDeterminer();
            StartReconnecting();
        }

        private void StartDeterminer()
        {
            try
            {
                determinerStatus = DeterminerStatus.STARTING;

                EnableFeatures();
                InitializeManager();
                StartDeterminerThread();
            }
            catch (RealSenseException e)
            {
                determinerStatus = DeterminerStatus.RECONNECTING;
                Console.WriteLine(e.Message);
            }
        }

        private void StartReconnecting()
        {
            reconnectThread.Start();
        }
        
        private void EnableFeatures()
        {
            components.Do(component => component.EnableFeatures());
        }

        private void InitializeManager()
        {
            senseManagerProvider.SenseManager.Init();
        }

        private void StartDeterminerThread()
        {
            determinerThread = new Thread(StartDetection);
            determinerThread.Start();
        }

        private void StartDetection()
        {
            try
            {
                TryToStartDetection();
            }
            catch (RealSenseException e)
            {
                determinerStatus = DeterminerStatus.RECONNECTING;
                RebuildSenseManager();

                Console.WriteLine(e.Message);
            }
        }

        private void RebuildSenseManager()
        {
            PXCMSenseManager manager = factory.Native.SenseManager(factory.Native.Session());
            senseManagerProviderBuilder.WithSenseManager(manager);
        }

        private void TryToStartDetection()
        {
            ConfigureComponents();

            determinerStatus = DeterminerStatus.STARTED;
            while (determinerStatus == DeterminerStatus.STARTED)
            {
                ProcessFrame();
            }
        }

        public void ConfigureComponents()
        {
            components.Do(component => component.Configure());
        }

        private void ProcessFrame()
        {
            AcquireFrame();
            var frameEvent = ProcessComponents();
            ReleaseFrame();

            InvokeFrameEvent(frameEvent);
        }

        private void AcquireFrame()
        {
            if (senseManagerProvider.SenseManager.AcquireFrame(true) < pxcmStatus.PXCM_STATUS_NO_ERROR)
            {
                throw new RealSenseException("Error while acquiring frame");
            }
        }

        private FrameEventArgs.Builder ProcessComponents()
        {
            DeterminerData.Builder determinerDataBuilder = factory.Data.Determiner.DeterminerData();
            components.Do(component => component.Process(determinerDataBuilder));
            return factory.Events.FrameEvent(overallImageCreator, realSenseConfiguration)
                .WithDeterminerData(determinerDataBuilder);
        }

        private void ReleaseFrame()
        {
            senseManagerProvider.SenseManager.ReleaseFrame();
        }

        private void InvokeFrameEvent(FrameEventArgs.Builder eventArgs)
        {
            if (Frame != null)
            {
                Frame.Invoke(eventArgs.Build());
            }
        }

        public void Stop()
        {
            if (determinerStatus != DeterminerStatus.STARTED)
            {
                return;
            }

            determinerStatus = DeterminerStatus.STOPPING;
            StopDeterminer();
            determinerStatus = DeterminerStatus.STOPPED;
        }

        private void StopDeterminer()
        {
            determinerThread.Join();
            reconnectThread.Join();
            senseManagerProvider.SenseManager.Close();
        }

        
        private void StartReconnect()
        {
            while (determinerStatus != DeterminerStatus.STOPPED && determinerStatus != DeterminerStatus.STOPPING)
            {
                if (determinerStatus == DeterminerStatus.RECONNECTING)
                {
                    StartDeterminer();
                }
                Thread.Sleep(5000);
            }
        }

        public class Builder
        {
            private RealSenseFactory factory;
            private PXCMSenseManager manager;
            private RealSensePropertiesManager propertiesManager;
            private RealSenseConfiguration configuration;

            public Builder WithFactory(RealSenseFactory factory)
            {
                this.factory = factory;
                return this;
            }

            public Builder WithManager(PXCMSenseManager manager)
            {
                this.manager = manager;
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

            public RealSenseDeterminerManager Build()
            {
                factory.Check(Preconditions.IsNotNull,
                    "The factory must be set in order to create the determiner manager");
                manager.Check(Preconditions.IsNotNull,
                    "The RealSense manager must be set in order to create the determiner manager");
                propertiesManager.Check(Preconditions.IsNotNull,
                    "The properties manager must be set in order to create the determiner manager");
                configuration.Check(Preconditions.IsNotNull,
                    "The RealSense configuration must be set in order to create the determiner manager");

                return new RealSenseDeterminerManager(factory, manager, propertiesManager, configuration);
            }
        }
    }
}