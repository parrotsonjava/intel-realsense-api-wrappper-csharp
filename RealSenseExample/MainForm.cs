using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using IntelRealSenseStart.Code.RealSense;
using IntelRealSenseStart.Code.RealSense.Config.Image;
using IntelRealSenseStart.Code.RealSense.Config.RealSense.Data;
using IntelRealSenseStart.Code.RealSense.Data.Event;
using IntelRealSenseStart.Code.RealSense.Data.Status;
using IntelRealSenseStart.Code.RealSense.Event;
using IntelRealSenseStart.Code.RealSense.Event.Data;
using IntelRealSenseStart.Code.RealSense.Exception;

namespace RealSenseExample
{
    public partial class MainForm : Form
    {
        private enum Status
        {
            IDLE,
            WAITING_FOR_NAME,
            WAITING_FOR_CONFIRMATION
        }

        public const String CAMERA_NAME = "Intel(R) RealSense(TM) 3D Camera"; // or "Lenovo EasyCamera"
        public const String AUDIO_DEVICE_NAME = "VF0800";
        public const String FACE_DATABASE_PATH = "faces.db";

        public delegate void BitmapHandler(Bitmap bitmap);

        private readonly RealSenseManager realSenseManager;
        private readonly String grammarIdle, grammarConfirm;

        private Status status = Status.IDLE;
        private String detectedName;

        private EmotionType currentEmotionType;
        private DateTime emotionStartTime;
        private bool emotionTriggered;
        private readonly TimeSpan timeSpanUntilEmotionTrigger = new TimeSpan(0, 0, 3);

        public MainForm()
        {
            InitializeComponent();

            grammarIdle = File.ReadAllText(@"Resources\idle.jsgf");
            grammarConfirm = File.ReadAllText(@"Resources\confirm.jsgf");

            realSenseManager = CreateRealSenseManager();
        }

        public RealSenseManager CreateRealSenseManager()
        {
            var builder = RealSenseManager.Create();
            var manager = builder.Configure(factory => factory.Configuration()
                .UsingBaseConfiguration(factory.BaseConfiguration()
                    .WithAudioConfiguration(factory.AudioConfiguration()
                        .UsingAudioInputDevice(
                            audioDeviceProperties => audioDeviceProperties.DeviceName.Contains(AUDIO_DEVICE_NAME)))
                    .WithVideoConfiguration(factory.VideoConfiguration()
                        .WithVideoDeviceName(CAMERA_NAME)))
                .WithSpeechRecognition(factory.SpeechRecognition().UsingGrammmar(grammarIdle))
                .WithSpeechSynthesis(factory.SpeechSynthesis())
                .WithHandsDetection(factory.HandsDetection().WithSegmentationImage())
                .WithFaceDetection(factory.FaceDetection()
                    .UsingLandmarks()
                    .UsingEmotions()
                    .UsingFaceIdentification(factory.FaceIdentification()
                        .WithFaceIdentificationMode(FaceIdentificationMode.ON_DEMAND)
                        .WithDataBasePath(FACE_DATABASE_PATH)
                        .UsingExistingDatabase(true)))
                .WithImage(factory.Image()
                    .WithColorStream(factory.ColorStream().From(new Size(640, 480), 30))
                    .WithDepthStream(factory.ColorStream().From(new Size(640, 480), 30))
                    .WithProjectionEnabled())).Build();

            manager.Ready += realSense_Ready;
            manager.Frame += realSense_Frame;
            manager.SpeechRecognized += realSense_SpeechRecognized;
            manager.SpeechOutput += realSense_SpeechOutput;

            return manager;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (realSenseManager.Status == DeterminerStatus.STOPPED)
            {
                realSenseManager.Start();
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            if (realSenseManager.Status == DeterminerStatus.STARTED)
            {
                realSenseManager.Stop();
            }
        }

        private void buttonRegisterFaces_Click(object sender, EventArgs e)
        {
            realSenseManager.RegisterFaces();
        }

        private void buttonUnregisterFaces_Click(object sender, EventArgs e)
        {
            realSenseManager.UnregisterFaces();
        }

        private void realSense_Ready()
        {
            realSenseManager.Speak("Hello");
            realSenseManager.StartRecognition();

            BeginInvoke((Action) (() => {
                buttonRegisterFaces.Enabled = true;
                buttonUnregisterFaces.Enabled = true;
            }));
        }

        private void realSense_Frame(FrameEventArgs frameEventArgs)
        {
            DrawBitmap(frameEventArgs);
            ProcessEventArgs(frameEventArgs);
        }

        private void DrawBitmap(FrameEventArgs frameEventArgs)
        {
            try
            {
                Bitmap bitmap = frameEventArgs.CreateImage()
                    .WithResolution(new Size(640, 480))
                    .WithBackgroundImage(ImageBackground.ColorImage)
                    .WithOverlay(ImageOverlay.ColorCoordinateHandJoints)
                    .WithOverlay(ImageOverlay.ColorCoordinateFaceLandmarks)
                    .WithOverlay(ImageOverlay.UserIds)
                    .Create();
                BeginInvoke(new BitmapHandler(SetImage), new object[] {bitmap});
            }
            catch (RealSenseException e)
            {
                Console.WriteLine(@"Error creating the image: {0}", e.Message);
            }
        }

        private void ProcessEventArgs(FrameEventArgs frameEventArgs)
        {
            var faces = frameEventArgs.Faces;
            var hands = frameEventArgs.Hands;

            if (faces.Faces.Count > 0)
            {
                var firstFace = faces.Faces[0];
                var primaryEmotion = firstFace.Emotions.PrimaryEmotion;

                ProcessEmotion(primaryEmotion);
            }
        }

        private void ProcessEmotion(EmotionData emotionData)
        {
            var emotionType = emotionData.Present ? emotionData.Type : EmotionType.NONE;

            if (currentEmotionType != emotionType)
            {
                emotionStartTime = DateTime.Now;
                emotionTriggered = false;
                currentEmotionType = emotionType;
            }
            else if(!emotionTriggered)
            {
                if (DateTime.Now - emotionStartTime > timeSpanUntilEmotionTrigger)
                {
                    emotionTriggered = true;
                    if (currentEmotionType == EmotionType.SADNESS)
                    {
                        realSenseManager.Speak("Why are you so sad?");
                    }
                    else if (currentEmotionType == EmotionType.DISGUST)
                    {
                        realSenseManager.Speak("Am I disgusting you?");
                    }
                }
            }
        }

        private void realSense_SpeechRecognized(SpeechRecognitionEventArgs eventArgs)
        {
            var sentence = eventArgs.Matches[0].Sentence;
            if (status == Status.IDLE && sentence == "Okay real sense")
            {
                SetRealSenseCalledMode();
            }
            else if (status == Status.WAITING_FOR_NAME)
            {
                SetRealSenseName(sentence);
            }
            else if (status == Status.WAITING_FOR_CONFIRMATION)
            {
                SetRealSenseConfirmation(sentence);
            }
        }
        
        private void SetRealSenseCalledMode()
        {
            status = Status.WAITING_FOR_NAME;
            realSenseManager.ConfigureRecognition(factory => factory.SpeechRecognition().UsingDictation());
            realSenseManager.Speak("What up?");
        }

        private void SetRealSenseName(String sentence)
        {
            if (sentence.StartsWith("This is ", StringComparison.OrdinalIgnoreCase))
            {
                detectedName = sentence.Replace("This is", "");
                status = Status.WAITING_FOR_CONFIRMATION;
                realSenseManager.ConfigureRecognition(factory => factory.SpeechRecognition().UsingGrammmar(grammarConfirm));
                realSenseManager.Speak(String.Format("Is this really {0}?", detectedName));
            }
        }
        private void SetRealSenseConfirmation(string sentence)
        {
            if (sentence.Equals("yes", StringComparison.OrdinalIgnoreCase))
            {
                status = Status.IDLE;
                realSenseManager.ConfigureRecognition(factory => factory.SpeechRecognition().UsingGrammmar(grammarIdle));
                realSenseManager.Speak(String.Format("Confirmed.", detectedName));
            } else if (sentence.Equals("no", StringComparison.OrdinalIgnoreCase))
            {
                status = Status.WAITING_FOR_NAME;
                realSenseManager.ConfigureRecognition(factory => factory.SpeechRecognition().UsingDictation());
                realSenseManager.Speak("In this case, you should speak more clearly.");
            }
        }

        private void realSense_SpeechOutput(SpeechOutputStatusEventArgs eventArgs)
        {
            if (eventArgs.Status == SpeechOutputStatus.STARTED_SPEAKING)
            {
                realSenseManager.StopRecognition();
            }
            else if (eventArgs.Status == SpeechOutputStatus.ENDED_SPEAKING)
            {
                realSenseManager.StartRecognition();
            }
        }

        private void SetImage(Bitmap bitmap)
        {
            pictureBoxHand.Image = bitmap;
        }
    }
}