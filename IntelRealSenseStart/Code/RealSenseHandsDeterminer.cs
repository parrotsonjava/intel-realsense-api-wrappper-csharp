using System;
using System.Threading;

namespace IntelRealSenseStart.Code
{
    internal class RealSenseHandsDeterminer
    {
        private Thread realSenseDeterminerThread;
        private PXCMSenseManager realSenseManager;

        private volatile bool stopped;

        public RealSenseHandsDeterminer()
        {
            stopped = true;
        }

        public bool Started
        {
            get { return !stopped; }
        }

        public void Start()
        {
            if (!stopped)
            {
                throw new Exception("The hands determiner is already started");
            }

            Console.WriteLine(@"Starting determiner thread");
            stopped = false;
            realSenseDeterminerThread = new Thread(InitializeRealSense);
            realSenseDeterminerThread.Start();
        }

        private void InitializeRealSense()
        {
            Console.WriteLine(@"Started determiner thread");
            TryToRunHandDetection();
        }

        private void TryToRunHandDetection()
        {
            realSenseManager = PXCMSession.CreateInstance().CreateSenseManager();
            realSenseManager.EnableHand();
            Console.WriteLine(@"Created RealSense manager");

            ConfigureHandDetection();
            realSenseManager.Init();
            Console.WriteLine(@"Initialized RealSense detection");

            StartHandDetection();
            realSenseManager.Close();
        }

        private void ConfigureHandDetection()
        {
            PXCMHandModule handModule = realSenseManager.QueryHand();
            PXCMHandConfiguration handConfiguration = handModule.CreateActiveConfiguration();

            ConfigureHandOptions(handConfiguration);

            handConfiguration.ApplyChanges();
            handConfiguration.Dispose();

            Console.WriteLine(@"Created hand configuration");
        }

        private void ConfigureHandOptions(PXCMHandConfiguration handConfiguration)
        {
            handConfiguration.DisableAllGestures();
        }

        private void StartHandDetection()
        {
            while (!stopped && realSenseManager.AcquireFrame(true) >= pxcmStatus.PXCM_STATUS_NO_ERROR)
            {
                ProcessFrame();
                realSenseManager.ReleaseFrame();
            }
        }

        private void ProcessFrame()
        {
            PXCMHandModule hand = realSenseManager.QueryHand();
            if (hand != null)
            {
                CreateAndProcessHandData(hand);
            }
        }

        private void CreateAndProcessHandData(PXCMHandModule hand)
        {
            PXCMHandData handData = hand.CreateOutput();
            handData.Update();
            ProcessHand(handData);
        }

        private void ProcessHand(PXCMHandData handData)
        {
            int numberOfHands = handData.QueryNumberOfHands();
            Console.WriteLine(numberOfHands);
        }

        public void Stop()
        {
            if (stopped)
            {
                throw new Exception("The hands determiner is already stopped");
            }

            stopped = true;
            realSenseDeterminerThread.Join();
        }
    }
}