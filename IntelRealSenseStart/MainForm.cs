using System;
using System.Drawing;
using System.Windows.Forms;
using IntelRealSenseStart.Code.RealSense;
using IntelRealSenseStart.Code.RealSense.Config.Image;
using IntelRealSenseStart.Code.RealSense.Data.Properties;
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

            DeviceProperties deviceProperties = null; // TODO builder.Properties.Devices.FindByName("foo");
            manager = builder.Configure(factory =>
                factory.Configuration()
                    .UsingDeviceConfiguration(factory.DeviceConfiguration()
                        .WithVideoDeviceConfiguration(factory.VideoDeviceConfiguration()
                            .WithVideoDevice(deviceProperties)))
                    .WithHandsDetection(factory.HandsDetection().WithSegmentationImage())
                    .WithFaceDetection(factory.FaceDetection().UsingLandmarks())
                    .WithImage(factory.Image()
                        .WithColorResolution(new Size(640, 480))
                        .WithDepthResolution(new Size(640, 480))
                        .WithProjectionEnabled()))
                .Build();

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
                .WithBackgroundImage(ImageBackground.ColorImage)
                .WithOverlay(ImageOverlay.ColorCoordinateHandJoints)
                .WithOverlay(ImageOverlay.ColorCoordinateFaceLandmarks)
                .Create();

            BeginInvoke(new BitmapHandler(SetImage), new object[] {bitmap});
        }

        private void SetImage(Bitmap bitmap)
        {
            pictureBoxHand.Image = bitmap;
        }
    }
}