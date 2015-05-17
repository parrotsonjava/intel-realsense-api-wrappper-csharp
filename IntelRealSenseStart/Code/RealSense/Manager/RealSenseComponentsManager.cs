using System;
using System.Linq;
using System.Threading;
using IntelRealSenseStart.Code.RealSense.Component.Common;
using IntelRealSenseStart.Code.RealSense.Component.Creator;
using IntelRealSenseStart.Code.RealSense.Component.Determiner;
using IntelRealSenseStart.Code.RealSense.Component.Output;
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
    public class RealSenseComponentsManager
    {
        public event FrameEventListener Frame;
        public event SpeechEventListener Speech;

        private readonly RealSenseComponent[] components;
        private RealSenseSpeechSynthesisOutputComponent speechSynthesizer;

        private readonly OverallImageCreator overallImageCreator;
        private readonly FacesLandmarksBuilder facesLandmarksBuilder;
        private readonly HandsJointsBuilder handsJointsBuilder;

        private readonly RealSenseFactory factory;
        private readonly RealSensePropertiesManager propertiesManager;
        private readonly RealSenseConfiguration realSenseConfiguration;

        private readonly NativeSense nativeSense;

        private Thread determinerThread;
        private readonly Thread reconnectThread;

        private volatile DeterminerStatus determinerStatus = DeterminerStatus.STOPPED;
        
        private RealSenseComponentsManager(RealSenseFactory factory, NativeSense nativeSense,
            RealSensePropertiesManager propertiesManager, RealSenseConfiguration realSenseConfiguration)
        {
            this.factory = factory;
            this.nativeSense = nativeSense;
            this.propertiesManager = propertiesManager;
            this.realSenseConfiguration = realSenseConfiguration;

            components = GetComponents().Where(component => component.ShouldBeStarted).ToArray();
            overallImageCreator = GetImageCreator(realSenseConfiguration);
            facesLandmarksBuilder = GetFacesLandmarksBuilder();
            handsJointsBuilder = getHandsJointsBuilder();

            reconnectThread = new Thread(StartReconnect);
        }

        private RealSenseComponent[] GetComponents()
        {
            var deviceComponent = factory.Components.Determiner.Device()
                .WithPropertiesManager(propertiesManager)
                .WithNativeSense(nativeSense)
                .WithConfiguration(realSenseConfiguration)
                .Build();
            var handsComponent = factory.Components.Determiner.Hands()
                .WithFactory(factory)
                .WithNativeSense(nativeSense)
                .WithConfiguration(realSenseConfiguration)
                .Build();
            var faceComponent = factory.Components.Determiner.Face()
                .WithFactory(factory)
                .WithNativeSense(nativeSense)
                .WithConfiguration(realSenseConfiguration)
                .Build();
            var pictureComponent = factory.Components.Determiner.Image()
                .WithFactory(factory)
                .WithNativeSense(nativeSense)
                .WithConfiguration(realSenseConfiguration)
                .Build();
            var speechRecognitionComponent = factory.Components.Determiner.SpeechRecognition()
                .WithFactory(factory)
                .WithNativeSense(nativeSense)
                .WithPropertiesManager(propertiesManager)
                .WithConfiguration(realSenseConfiguration)
                .Build();
            speechSynthesizer = factory.Components.Output.SpeechSynthesis()
                .WithFactory(factory)
                .WithNativeSense(nativeSense)
                .WithPropertiesManager(propertiesManager)
                .WithConfiguration(realSenseConfiguration)
                .Build();

            InitializeListeners(speechRecognitionComponent);

            return new RealSenseComponent[]
            {
                handsComponent, faceComponent, pictureComponent, deviceComponent,
                speechRecognitionComponent, speechSynthesizer
            };
        }

        private void InitializeListeners(SpeechRecognitionDeterminerComponent speechRecognitionComponent)
        {
            speechRecognitionComponent.Speech += speechRecognitionCompoment_Speech;
        }

        private void speechRecognitionCompoment_Speech(SpeechEventArgs speechEventArgs)
        {
            if (Speech != null)
            {
                Speech.Invoke(speechEventArgs);
            }
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

        private FacesLandmarksBuilder GetFacesLandmarksBuilder()
        {
             return factory.Components.Creator.FacesLandmarksBuilder().Build();
        }

        private HandsJointsBuilder getHandsJointsBuilder()
        {
            return factory.Components.Creator.HandsJointsBuilder().Build();
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
            nativeSense.SenseManager.Init();
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
                ResetSession();

                Console.WriteLine(e.Message);
            }
        }

        private void ResetSession()
        {
            nativeSense.Initialize();
        }

        private void TryToStartDetection()
        {
            ConfigureComponents();

            determinerStatus = DeterminerStatus.STARTED;
            while (determinerStatus == DeterminerStatus.STARTED)
            {
                ProcessFrame();
            }

            StopComponents();
        }

        public void ConfigureComponents()
        {
            components.Do(component => component.Configure());
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
            components.OfType<FrameDeterminerComponent>().Do(component => component.Process(determinerDataBuilder));
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
            components.Do(component => component.Stop());
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

        public void Speak(String sentence)
        {
            speechSynthesizer.Speak(sentence);
        }

        public class Builder
        {
            private RealSenseFactory factory;
            private NativeSense nativeSense;
            private RealSensePropertiesManager propertiesManager;
            private RealSenseConfiguration configuration;

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

            public RealSenseComponentsManager Build()
            {
                factory.Check(Preconditions.IsNotNull,
                    "The factory must be set in order to create the determiner manager");
                nativeSense.Check(Preconditions.IsNotNull,
                    "The native sense must be set in order to create the determiner manager");
                propertiesManager.Check(Preconditions.IsNotNull,
                    "The properties manager must be set in order to create the determiner manager");
                configuration.Check(Preconditions.IsNotNull,
                    "The RealSense configuration must be set in order to create the determiner manager");

                return new RealSenseComponentsManager(factory, nativeSense, propertiesManager, configuration);
            }
        }
    }
}