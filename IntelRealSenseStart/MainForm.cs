using System;
using System.Drawing;
using System.Windows.Forms;
using IntelRealSenseStart.Code.RealSense;
using IntelRealSenseStart.Code.RealSense.Config.Image;
using IntelRealSenseStart.Code.RealSense.Data.Status;
using IntelRealSenseStart.Code.RealSense.Event;

namespace IntelRealSenseStart
{
    public partial class MainForm : Form
    {
        public const String CAMERA_NAME = "Intel(R) RealSense(TM) 3D Camera"; // or "Lenovo EasyCamera"

        public delegate void BitmapHandler(Bitmap bitmap);

        private readonly RealSenseManager manager;

        public MainForm()
        {
            InitializeComponent();
            var builder = RealSenseManager.Create();
            manager = builder.Configure(factory => factory.Configuration()
                .UsingDeviceConfiguration(factory.DeviceConfiguration()
                    .WithVideoDeviceConfiguration(factory.VideoDeviceConfiguration()
                        .WithVideoDeviceName(CAMERA_NAME)))
                .WithHandsDetection(factory.HandsDetection().WithSegmentationImage())
                .WithFaceDetection(factory.FaceDetection().UsingLandmarks())
                .WithImage(factory.Image()
                    .WithColorStream(factory.ColorStream().From(new Size(640, 480), 30))
                    .WithDepthStream(factory.ColorStream().From(new Size(640, 480), 30))
                    .WithProjectionEnabled())).Build();

            manager.Frame += realSense_Hands_Frame;

        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (manager.Status == DeterminerStatus.STOPPED)
            {
                manager.Start();
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            if (manager.Status == DeterminerStatus.STARTED)
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