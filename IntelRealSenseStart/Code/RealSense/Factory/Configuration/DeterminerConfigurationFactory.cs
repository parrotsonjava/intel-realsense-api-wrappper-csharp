using IntelRealSenseStart.Code.RealSense.Config.RealSense;

namespace IntelRealSenseStart.Code.RealSense.Factory.Configuration
{
    public class DeterminerConfigurationFactory
    {
        public RealSenseConfiguration.Builder Configuration()
        {
            return new RealSenseConfiguration.Builder();
        }

        public BaseConfiguration.Builder BaseConfiguration()
        {
            return new BaseConfiguration.Builder();
        }

        public VideoConfiguration.Builder VideoConfiguration()
        {
            return new VideoConfiguration.Builder();
        }

        public AudioConfiguration.Builder AudioConfiguration()
        {
            return new AudioConfiguration.Builder();
        }

        public HandsConfiguration.Builder HandsDetection()
        {
            return new HandsConfiguration.Builder();
        }

        public FaceConfiguration.Builder FaceDetection()
        {
            return new FaceConfiguration.Builder();
        }

        public SpeechRecognitionConfiguration.Builder SpeechRecognition()
        {
            return new SpeechRecognitionConfiguration.Builder();
        }
        public SpeechSynthesisConfiguration.Builder SpeechSynthesis()
        {
            return new SpeechSynthesisConfiguration.Builder();
        }

        public ImageConfiguration.Builder Image()
        {
            return new ImageConfiguration.Builder();
        }

        public StreamConfiguration.Builder ColorStream()
        {
            return new StreamConfiguration.Builder();
        }
    }
}
