﻿using System;
using System.Drawing;
using System.Windows.Forms;
using IntelRealSenseStart.Code.RealSense;
using IntelRealSenseStart.Code.RealSense.Config.HandsImage;
using IntelRealSenseStart.Code.RealSense.Event;

namespace IntelRealSenseStart
{
    public partial class MainForm : Form
    {
        public delegate void BitmapHandler(Bitmap bitmap);

        private readonly RealSenseManager manager;

        public MainForm()
        {
            InitializeComponent();
            var builder = RealSenseManager.Create();
            manager = builder.Configure(factory =>
                factory.Configuration()
                    .WithHandsDetection(factory.HandsDetection().WithSegmentationImage())
                    .WithColorImage(factory.Image().WithResolution(new Size(640, 480)))
                    .WithDepthImage(factory.Image().WithResolution(new Size(640, 480)))).Build();

            manager.Frame += realSense_Hands_Frame;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (!manager.Started)
            {
                manager.Start();
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            if (manager.Started)
            {
                manager.Stop();
            }
        }

        private void realSense_Hands_Frame(FrameEventArgs frameEventArgs)
        {
            Bitmap bitmap = frameEventArgs.CreateImage()
                .WithResolution(new Size(640, 480))
                .WithBackgroundImage(HandsImageBackground.ColorImage)
                .WithOverlay(HandsImageOverlay.ColorCoordinateHandJoints)
                .Create();

            BeginInvoke(new BitmapHandler(SetImage), new object[] { bitmap });
        }

        private void SetImage(Bitmap bitmap)
        {
            pictureBoxHand.Image = bitmap;
        }
    }
}