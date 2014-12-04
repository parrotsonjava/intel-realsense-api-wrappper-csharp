using System;
using System.Linq;
using System.Threading;
using IntelRealSenseStart.Code.RealSense.Component.Determiner;
using IntelRealSenseStart.Code.RealSense.Config.RealSense;
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

        private readonly RealSenseFactory factory;
        private readonly PXCMSenseManager manager;
        private readonly Configuration configuration;

        private Thread determinerThread;
        private volatile bool stopped = true;

        private RealSenseDeterminerManager(RealSenseFactory factory, PXCMSenseManager manager,
            Configuration configuration)
        {
            this.factory = factory;
            this.manager = manager;
            this.configuration = configuration;

            var allComponents = GetComponents();
            components = allComponents.Where(component => component.ShouldBeStarted).ToArray();
        }

        private DeterminerComponent[] GetComponents()
        {
            var deviceComponent = factory.Components.Determiner.Device()
                .WithManager(manager)
                .Build();
            var handsComponent = factory.Components.Determiner.Hands()
                .WithFactory(factory)
                .WithManager(manager)
                .WithConfiguration(configuration)
                .Build();
            var pictureComponent = factory.Components.Determiner.Image()
                .WithFactory(factory)
                .WithManager(manager)
                .WithConfiguration(configuration)
                .Build();

            return new DeterminerComponent[] {handsComponent, pictureComponent, deviceComponent};
        }

        public bool Started
        {
            get { return !stopped; }
        }

        public void Start()
        {
            if (!stopped)
            {
                throw new RealSenseException("Components cannot be started because it is already running");
            }

            stopped = false;
            determinerThread = new Thread(StartDetection);
            determinerThread.Start();
        }

        public void Stop()
        {
            if (stopped)
            {
                return;
            }

            stopped = true;
            determinerThread.Join();
        }

        public void EnableFeatures()
        {
            components.Do(component => component.EnableFeatures());
        }

        public void StartDetection()
        {
            ConfigureComponents();
            while (!stopped)
            {
                ProcessFrame();
            }

            Console.WriteLine(@"Finished");
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
            FrameEventArgs.Builder frameEvent = factory.Events.FrameEvent(configuration);
            components.Do(component => component.Process(frameEvent));
            return frameEvent;
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

        public class Builder
        {
            private RealSenseFactory factory;
            private PXCMSenseManager manager;
            private Configuration configuration;

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

            public Builder WithConfiguration(Configuration configuration)
            {
                this.configuration = configuration;
                return this;
            }

            public RealSenseDeterminerManager Build()
            {
                factory.CheckState(Preconditions.IsNotNull,
                    "The factory must be set in order to create the determiner manager");
                manager.CheckState(Preconditions.IsNotNull,
                    "The RealSense manager must be set in order to create the determiner manager");
                configuration.CheckState(Preconditions.IsNotNull,
                    "The RealSense configuration must be set in order to create the determiner manager");

                return new RealSenseDeterminerManager(factory, manager, configuration);
            }
        }
    }
}