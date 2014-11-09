﻿using System;
using System.Drawing;
using System.Threading;

namespace IntelRealSenseStart.Code
{
    internal class RealSenseHandsDeterminer
    {
        public delegate void NewBitmapDelegate(Bitmap bitmap);

        private readonly RealSenseFactory realSenseFactory;
        private readonly PXCMSenseManager realSenseManager;

        private PXCMHandData handData;
        private Thread realSenseDeterminerThread;

        private volatile bool stopped;


        public RealSenseHandsDeterminer(PXCMSenseManager realSenseManager, RealSenseFactory realSenseFactory)
        {
            this.realSenseManager = realSenseManager;
            this.realSenseFactory = realSenseFactory;
            stopped = true;
        }

        public bool Started
        {
            get { return !stopped; }
        }

        public event NewBitmapDelegate SegmentationImage;

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
            InitializeRealSenseManagerForHandData();
            Console.WriteLine(@"Created RealSense manager");

            ConfigureDevice();
            ConfigureHandDetection();

            StartHandDetection();

            realSenseManager.Close();
            Console.WriteLine(@"Closed RealSense detection");
        }

        private void InitializeRealSenseManagerForHandData()
        {
            realSenseManager.EnableHand();
            realSenseManager.Init();
        }

        private void ConfigureDevice()
        {
            PXCMCapture.DeviceInfo deviceInfo;
            realSenseManager.QueryCaptureManager().QueryDevice().QueryDeviceInfo(out deviceInfo);

            if (deviceInfo == null || deviceInfo.model != PXCMCapture.DeviceModel.DEVICE_MODEL_IVCAM)
            {
                throw new Exception("No device info found");
            }

            realSenseManager.captureManager.device.SetDepthConfidenceThreshold(1);
            realSenseManager.captureManager.device.SetMirrorMode(PXCMCapture.Device.MirrorMode.MIRROR_MODE_HORIZONTAL);
            realSenseManager.captureManager.device.SetIVCAMFilterOption(6);
        }

        private void ConfigureHandDetection()
        {
            PXCMHandModule handModule = realSenseManager.QueryHand();
            handData = handModule.CreateOutput();
            PXCMHandConfiguration handConfiguration = handModule.CreateActiveConfiguration();

            ConfigureHandOptions(handConfiguration);

            handConfiguration.ApplyChanges();
            handConfiguration.Update();

            Console.WriteLine(@"Created hand configuration");
        }

        private void ConfigureHandOptions(PXCMHandConfiguration handConfiguration)
        {
            handConfiguration.DisableAllGestures();
            handConfiguration.EnableSegmentationImage(true);
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
            PXCMCapture.Sample handSample = realSenseManager.QueryHandSample();
            if (handSample != null)
            {
                handData.Update();
                CreateAndProcessHandData(handSample);
            }
        }

        private void CreateAndProcessHandData(PXCMCapture.Sample handSample)
        {
            HandBitmapBuilder handBitmapBuilder = realSenseFactory.CreateHandBitmapBuilder();

            int numberOfHands = handData.QueryNumberOfHands();
            for (int i = 0; i < numberOfHands; i++)
            {
                PXCMHandData.IHand oneHandData;
                handData.QueryHandData(PXCMHandData.AccessOrderType.ACCESS_ORDER_BY_TIME, i, out oneHandData);

                UpdateHandDataWith(oneHandData, handBitmapBuilder);
            }

            InvokeSegmentationImage(handBitmapBuilder.Build());
        }

        private void UpdateHandDataWith(PXCMHandData.IHand oneHandData, HandBitmapBuilder handBitmapBuilder)
        {
            PXCMImage handImage;
            pxcmStatus handImageStatus = oneHandData.QuerySegmentationImage(out handImage);
            var userId = (byte) oneHandData.QueryBodySide();

            if (handImageStatus >= pxcmStatus.PXCM_STATUS_NO_ERROR)
            {
                handBitmapBuilder.addSegmentationImage(handImage, userId);
                handImage.Dispose();
            }
            else
            {
                Console.WriteLine(@"Error");
                //throw new Exception("Could not determine segmentation image");
            }
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

        private void InvokeSegmentationImage(Bitmap bitmap)
        {
            if (bitmap == null)
            {
                return;
            }

            NewBitmapDelegate handler = SegmentationImage;
            if (handler != null) handler(bitmap);
        }
    }
}