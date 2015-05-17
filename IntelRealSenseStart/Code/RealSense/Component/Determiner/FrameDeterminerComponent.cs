using IntelRealSenseStart.Code.RealSense.Data.Determiner;

namespace IntelRealSenseStart.Code.RealSense.Component.Determiner
{
    internal interface FrameDeterminerComponent : DeterminerComponent
    {
        void Process(DeterminerData.Builder determinerData);
    }
}