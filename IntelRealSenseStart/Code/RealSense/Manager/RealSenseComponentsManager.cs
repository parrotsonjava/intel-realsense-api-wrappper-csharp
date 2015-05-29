using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using IntelRealSenseStart.Code.RealSense.Component.Common;
using IntelRealSenseStart.Code.RealSense.Component.Creator;
using IntelRealSenseStart.Code.RealSense.Component.Determiner;
using IntelRealSenseStart.Code.RealSense.Config.RealSense;
using IntelRealSenseStart.Code.RealSense.Data.Determiner;
using IntelRealSenseStart.Code.RealSense.Data.Status;
using IntelRealSenseStart.Code.RealSense.Event;
using IntelRealSenseStart.Code.RealSense.Exception;
using IntelRealSenseStart.Code.RealSense.Factory;
using IntelRealSenseStart.Code.RealSense.Helper;
using IntelRealSenseStart.Code.RealSense.Manager.Builder;
using IntelRealSenseStart.Code.RealSense.Provider;

namespace IntelRealSenseStart.Code.RealSense.Manager
{
    public class RealSenseComponentsManager
    {
        private const int IDLE_DETERMINER_TIMEOUT = 500;

        public event ReadyEventListener Ready;
        public event FrameEventListener Frame;

        private readonly IEnumerable<RealSenseComponent> components;

        private readonly OverallImageCreator overallImageCreator;
        private readonly FacesBuilder facesLandmarksBuilder;
        private readonly HandsBuilder handsJointsBuilder;

        private readonly RealSenseFactory factory;
        private readonly RealSenseConfiguration realSenseConfiguration;

        private readonly NativeSense nativeSense;

        private Thread determinerThread;
        private Thread reconnectThread;

        private volatile DeterminerStatus determinerStatus;
        
        private RealSenseComponentsManager(RealSenseFactory factory, NativeSense nativeSense,
            RealSenseConfiguration realSenseConfiguration, RealSenseComponentsBuilder componentsBuilder)
        {
            this.factory = factory;
            this.nativeSense = nativeSense;
            this.realSenseConfiguration = realSenseConfiguration;

            determinerStatus = DeterminerStatus.STOPPED;

            components = GetComponents(componentsBuilder);
            overallImageCreator = GetImageCreator(componentsBuilder);
            facesLandmarksBuilder = componentsBuilder.GetFacesLandmarksBuilder();
            handsJointsBuilder = componentsBuilder.getHandsJointsBuilder();
        }

        private IEnumerable<RealSenseComponent> GetComponents(RealSenseComponentsBuilder componentsBuilder)
        {
            var realSenseComponents = new RealSenseComponent[]
            {
                componentsBuilder.CreateHandsDeterminerComponent(), 
                componentsBuilder.CreateFaceDeterminerComponent(), 
                componentsBuilder.CreatePictureDeterminerComponent(), 
                componentsBuilder.CreateDeviceDeterminerComponent(),
                componentsBuilder.CreateSpeechRecognitionDeterminerComponent(),
                componentsBuilder.CreateSpeechSynthesisOutputComponent()
            };
            return realSenseComponents;
        }

        private OverallImageCreator GetImageCreator(RealSenseComponentsBuilder componentsBuilder)
        {
            return componentsBuilder.CreateOverallImageCreator(new ImageCreator[]
            {
                componentsBuilder.CreateBasicImageCreatorComponent(),
                componentsBuilder.CreateHandsImageCreatorComponent(),
                componentsBuilder.CreateFaceImageCreatorComponent(),
                componentsBuilder.CreateUserIdsImageCreator()
            });
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

                GetActiveComponents().Do(component => component.EnableFeatures());
                nativeSense.SenseManager.Init();
                (determinerThread = new Thread(StartDetection)).Start();
            }
            catch (RealSenseException e)
            {
                determinerStatus = DeterminerStatus.RECONNECTING;
                Console.WriteLine(e.Message);
            }
        }

        private void StartReconnecting()
        {
            reconnectThread = new Thread(StartReconnect);
            reconnectThread.Start();
        }

        private void StartDetection()
        {
            try
            {
                TryToStartDetection();
            }
            catch (RealSenseException e)
            {
                Console.WriteLine(@"Reconnecting due to error: {0}", e.Message);
                determinerStatus = DeterminerStatus.RECONNECTING;
                ResetSession();
            }
        }

        private void ResetSession()
        {
            nativeSense.Initialize();
        }

        private void TryToStartDetection()
        {
            StartComponents();
            while (determinerStatus == DeterminerStatus.STARTED)
            {
                if (realSenseConfiguration.NeedsFrame)
                {
                    ProcessFrame();
                }
                else
                {
                    Thread.Sleep(IDLE_DETERMINER_TIMEOUT);
                }
            }
            StopComponents();
        }

        private void StartComponents()
        {
            GetActiveComponents().Do(component => component.Configure());
            determinerStatus = DeterminerStatus.STARTED;
            InvokeReadyEvent();
        }

        private void InvokeReadyEvent()
        {
            if (Ready != null)
            {
                Ready.Invoke();
            }
        }

        private void ProcessFrame()
        {
            try
            {
                AcquireFrame();
                var frameEvent = ProcessComponents();
                ReleaseFrame();
                
                InvokeFrameEvent(frameEvent);
            }
            catch (RealSenseAcquireException)
            {
                ReleaseFrame();
            }
        }

        private void AcquireFrame()
        {
            var acquireFrame = nativeSense.SenseManager.AcquireFrame(true);
            if (acquireFrame < pxcmStatus.PXCM_STATUS_NO_ERROR)
            {
                throw new RealSenseAcquireException("Error while acquiring frame");
            }
        }

        private FrameEventArgs.Builder ProcessComponents()
        {
            DeterminerData.Builder determinerDataBuilder = factory.Data.Determiner.DeterminerData();
            GetActiveComponents().OfType<FrameDeterminerComponent>().Do(component => component.Process(determinerDataBuilder));
            return factory.Events.FrameEvent()
                .WithOverallImageCreator(overallImageCreator)
                .WithFacesLandmarksBuilder(facesLandmarksBuilder)
                .WithHandsJointsBuilder(handsJointsBuilder)
                .WithRealSenseConfiguration(realSenseConfiguration)
                .WithDeterminerData(determinerDataBuilder);
        }

        private void ReleaseFrame()
        {
            nativeSense.SenseManager.ReleaseFrame();
        }

        private void InvokeFrameEvent(FrameEventArgs.Builder eventArgs)
        {
            if (Frame != null)
            {
                Frame.Invoke(eventArgs.Build());
            }
        }

        private void StopComponents()
        {
            GetActiveComponents().Do(component => component.Stop());
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
            nativeSense.SenseManager.Close();
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

        public IEnumerable<RealSenseComponent> GetActiveComponents()
        {
            return components.Where(component => component.ShouldBeStarted);
        } 

        public T GetComponent<T>() where T : RealSenseComponent
        {
            try
            {
                T namedComponent = (T) components.First(component => component.GetType() == typeof(T));
                if (!namedComponent.ShouldBeStarted)
                {
                    throw new IllegalStateException(String.Format("The component of type {0} was not configured", typeof(T).Name));
                }
                return namedComponent;
            }
            catch (InvalidOperationException)
            {
                throw new IllegalStateException(String.Format("No component with type {0} was found", typeof(T).Name));
            }
        }

        public bool IsComponentActive(Type type)
        {
            return GetActiveComponents().Any(component => component.GetType() == type);
        }

        public void OnFrame(FrameEventListener frameEventListener)
        {
            Frame += frameEventListener;
        }

        public void OnReady(ReadyEventListener readyEventListener)
        {
            Ready += readyEventListener;
        }

        public void CheckIfReady()
        {
            this.Check(manager => manager.determinerStatus == DeterminerStatus.STARTED,
                "The component manager is not yet fully started");
        }

        public class Builder
        {
            private RealSenseFactory factory;
            private NativeSense nativeSense;
            private RealSenseConfiguration configuration;
            private RealSenseComponentsBuilder componentsBuilder;

            public Builder WithFactory(RealSenseFactory factory)
            {
                this.factory = factory;
                return this;
            }

            public Builder WithNativeSense(NativeSense nativeSense)
            {
                this.nativeSense = nativeSense;
                return this;
            }

            public Builder WithConfiguration(RealSenseConfiguration configuration)
            {
                this.configuration = configuration;
                return this;
            }

            public Builder WithComponentsBuilder(RealSenseComponentsBuilder componentsBuilder)
            {
                this.componentsBuilder = componentsBuilder;
                return this;
            }

            public RealSenseComponentsManager Build()
            {
                factory.Check(Preconditions.IsNotNull,
                    "The factory must be set in order to create the components manager");
                nativeSense.Check(Preconditions.IsNotNull,
                    "The native sense must be set in order to create the components manager");
                componentsBuilder.Check(Preconditions.IsNotNull,
                    "The components builder must be set in order to create the components manager");
                configuration.Check(Preconditions.IsNotNull,
                    "The RealSense configuration must be set in order to create the components manager");

                return new RealSenseComponentsManager(factory, nativeSense, configuration, componentsBuilder);
            }
        }
    }
}