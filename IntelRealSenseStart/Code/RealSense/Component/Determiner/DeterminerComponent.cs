using IntelRealSenseStart.Code.RealSense.Event;

namespace IntelRealSenseStart.Code.RealSense.Component.Determiner
{
    internal interface DeterminerComponent
    {
        bool ShouldBeStarted { get; }

        void EnableFeatures();

        void Configure();

        void Process(FrameEventArgs.Builder frameEvent);
    }
}