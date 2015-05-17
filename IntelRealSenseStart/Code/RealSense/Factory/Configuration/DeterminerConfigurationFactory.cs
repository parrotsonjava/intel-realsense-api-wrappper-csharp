using IntelRealSenseStart.Code.RealSense.Config.RealSense;

namespace IntelRealSenseStart.Code.RealSense.Factory.Configuration
{
    public class DeterminerConfigurationFactory
    {
        public RealSenseConfiguration.Builder Configuration()
        {
            return new RealSenseConfiguration.Builder();
        }

        public DeviceConfiguration.Builder DeviceConfiguration()
        {
            return new DeviceConfiguration.Builder();
        }

        public VideoDeviceConfiguration.Builder VideoDeviceConfiguration()
        {
            return new VideoDeviceConfiguration.Builder();
        }

        public AudioDeviceConfiguration.Builder AudioDeviceConfiguration()
        {
            return new AudioDeviceConfiguration.Builder();
        }

        public HandsConfiguration.Builder HandsDetection()
        {
            return new HandsConfiguration.Builder();
        }

        public FaceConfiguration.Builder FaceDetection()
        {
            return new FaceConfiguration.Builder();
        }

        public SpeechConfiguration.Builder SpeechRecognition()
        {
            return new SpeechConfiguration.Builder();
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
