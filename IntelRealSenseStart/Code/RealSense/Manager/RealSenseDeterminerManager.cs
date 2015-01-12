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

namespace IntelRealSenseStart.Code.RealSense.Manager
{
    public class RealSenseDeterminerManager
    {
        public delegate void FrameEventListener(FrameEventArgs frameEventArgs);

        public event FrameEventListener Frame;

        private readonly DeterminerComponent[] components;
        private readonly OverallImageCreator overallImageCreator;

        private readonly RealSenseFactory factory;
        private readonly PXCMSenseManager manager;
        private readonly RealSensePropertiesManager propertiesManager;
        private readonly RealSenseConfiguration realSenseConfiguration;

        private Thread determinerThread;
        private Thread reconnectThread;

        private volatile DeterminerStatus determinerStatus = DeterminerStatus.STOPPED;

        private RealSenseDeterminerManager(RealSenseFactory factory, PXCMSenseManager manager,
            RealSensePropertiesManager propertiesManager, RealSenseConfiguration realSenseConfiguration)
        {
            this.factory = factory;
            this.manager = manager;
            this.propertiesManager = propertiesManager;
            this.realSenseConfiguration = realSenseConfiguration;
            reconnectThread = new Thread(StartReconnect);

            var allComponents = GetComponents();
            components = allComponents.Where(component => component.ShouldBeStarted).ToArray();
            overallImageCreator = GetImageCreator(realSenseConfiguration);
        }

        private DeterminerComponent[] GetComponents()
        {
            var deviceComponent = factory.Components.Determiner.Device()
                .WithPropertiesManager(propertiesManager)
                .WithManager(manager)
                .WithConfiguration(realSenseConfiguration)
                .Build();
            var handsComponent = factory.Components.Determiner.Hands()
                .WithFactory(factory)
                .WithManager(manager)
                .WithConfiguration(realSenseConfiguration)
                .Build();
            var faceComponent = factory.Components.Determiner.Face()
                .WithFactory(factory)
                .WithManager(manager)
                .WithConfiguration(realSenseConfiguration)
                .Build();
            var pictureComponent = factory.Components.Determiner.Image()
                .WithFactory(factory)
                .WithManager(manager)
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
            manager.Init();
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
                // TODO In this case, the PXCMManager must be rebuilt
                determinerStatus = DeterminerStatus.RECONNECTING;
                Console.WriteLine(e.Message);
            }
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
            if (manager.AcquireFrame(true) < pxcmStatus.PXCM_STATUS_NO_ERROR)
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
            manager.ReleaseFrame();
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
            manager.Close();
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