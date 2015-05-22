using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using IntelRealSenseStart.Code.RealSense;
using IntelRealSenseStart.Code.RealSense.Config.Image;
using IntelRealSenseStart.Code.RealSense.Data.Status;
using IntelRealSenseStart.Code.RealSense.Event;
using IntelRealSenseStart.Code.RealSense.Exception;

namespace RealSenseExample
{
    public partial class MainForm : Form
    {
        public const String CAMERA_NAME = "Intel(R) RealSense(TM) 3D Camera"; // or "Lenovo EasyCamera"
        public const String AUDIO_DEVICE_NAME = "VF0800";

        public delegate void BitmapHandler(Bitmap bitmap);

        private readonly RealSenseManager manager;

        public MainForm()
        {
            var grammar = File.ReadAllText(@"Resources\control.jsgf");

            InitializeComponent();
            var builder = RealSenseManager.Create();
            manager = builder.Configure(factory => factory.Configuration()
                .UsingBaseConfiguration(factory.BaseConfiguration()
                    .WithAudioConfiguration(factory.AudioConfiguration()
                        .UsingAudioInputDevice(audioDeviceProperties => audioDeviceProperties.DeviceName.Contains(AUDIO_DEVICE_NAME)))
                    .WithVideoConfiguration(factory.VideoConfiguration()
                        .WithVideoDeviceName(CAMERA_NAME)))
                .WithSpeechRecognition(factory.SpeechRecognition().UsingGrammmar(grammar))
                .WithSpeechSynthesis(factory.SpeechSynthesis())
                .WithHandsDetection(factory.HandsDetection().WithSegmentationImage())
                .WithFaceDetection(factory.FaceDetection().UsingLandmarks())
                .WithImage(factory.Image()
                    .WithColorStream(factory.ColorStream().From(new Size(640, 480), 30))
                    .WithDepthStream(factory.ColorStream().From(new Size(640, 480), 30))
                    .WithProjectionEnabled())).Build();

            manager.Ready += realSense_Ready;
            manager.Frame += realSense_Frame;
            manager.SpeechRecognized += realSense_Speech;
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

        private void realSense_Ready()
        {
            manager.StartRecognition();
        }

        private void realSense_Frame(FrameEventArgs frameEventArgs)
        {
            try
            {
                Bitmap bitmap = frameEventArgs.CreateImage()
                    .WithResolution(new Size(640, 480))
                    .WithBackgroundImage(ImageBackground.ColorImage)
                    .WithOverlay(ImageOverlay.ColorCoordinateHandJoints)
                    .WithOverlay(ImageOverlay.ColorCoordinateFaceLandmarks)
                    .Create();
                BeginInvoke(new BitmapHandler(SetImage), new object[] { bitmap });
            }
            catch (RealSenseException e)
            {
                Console.WriteLine(@"Error creating the image: {0}", e.Message);
            }

            var facesLandmarksData = frameEventArgs.FaceLandmarks;
            var handsJoints = frameEventArgs.HandsJoints;
        }

        private void realSense_Speech(SpeechRecognitionEventArgs speechRecognitionEventArgs)
        {
            var sentence = speechRecognitionEventArgs.Sentence;
            manager.Speak(String.Format("Did you say {0}?", sentence));
        }

        private void SetImage(Bitmap bitmap)
        {
            pictureBoxHand.Image = bitmap;
        }
    }
}