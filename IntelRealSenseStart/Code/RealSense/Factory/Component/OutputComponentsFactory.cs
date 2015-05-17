using IntelRealSenseStart.Code.RealSense.Component.Output;

namespace IntelRealSenseStart.Code.RealSense.Factory.Component
{
    public class OutputComponentsFactory
    {
        public RealSenseSpeechSynthesisOutputComponent.Builder SpeechSynthesis()
        {
            return new RealSenseSpeechSynthesisOutputComponent.Builder();
        }
    }
}
