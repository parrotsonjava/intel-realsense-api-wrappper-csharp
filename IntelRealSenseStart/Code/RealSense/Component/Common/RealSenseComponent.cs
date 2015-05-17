namespace IntelRealSenseStart.Code.RealSense.Component.Common
{
    public interface RealSenseComponent
    {
        bool ShouldBeStarted { get; }

        void EnableFeatures();

        void Configure();

        void Stop();
    }
}