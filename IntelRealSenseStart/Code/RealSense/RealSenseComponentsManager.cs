using System;
using System.Linq;
using System.Threading;
using IntelRealSenseStart.Code.RealSense.Config;
using IntelRealSenseStart.Code.RealSense.Event;
using IntelRealSenseStart.Code.RealSense.Exception;
using IntelRealSenseStart.Code.RealSense.Factory;

namespace IntelRealSenseStart.Code.RealSense
{
    public class RealSenseComponentsManager
    {
        public delegate void FrameEventListener(FrameEventArgs frameEventArgs);
        public event FrameEventListener Frame;

        private readonly Component.Component[] components;

        private readonly RealSenseFactory factory;
        private readonly PXCMSenseManager manager;
        private readonly Configuration configuration;

        private Thread determinerThread;
        private volatile bool stopped = true;

        private RealSenseComponentsManager(RealSenseFactory factory, PXCMSenseManager manager, Configuration configuration)
        {
            this.factory = factory;
            this.manager = manager;
            this.configuration = configuration;

            var handsComponent = factory.Components.HandsComponent().Build(factory, manager, configuration);
            var pictureComponent = factory.Components.ImageComponent().Build(factory, manager, configuration);

            var allComponents = new Component.Component[] {handsComponent, pictureComponent};
            components = allComponents.Where(component => component.ShouldBeStarted).ToArray();
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

            Console.WriteLine("Finished");
        }

        public void ConfigureComponents()
        {
            components.Do(component => component.Configure());
        }

        private void ProcessFrame()
        {
            AcquireFrame();

            FrameEventArgs.Builder frameEvent = factory.Events.FrameEvent(configuration);
            components.Do(component => component.Process(frameEvent));

            manager.ReleaseFrame();
            InvokeFrameEvent(frameEvent);
        }

        private void AcquireFrame()
        {
            if (manager.AcquireFrame(true) < pxcmStatus.PXCM_STATUS_NO_ERROR)
            {
                throw new RealSenseException("Error while acquiring frame");
            }
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
            public RealSenseComponentsManager Build(RealSenseFactory factory, PXCMSenseManager manager, Configuration configuration)
            {
                return new RealSenseComponentsManager(factory, manager, configuration);
            }
        }

    }
}