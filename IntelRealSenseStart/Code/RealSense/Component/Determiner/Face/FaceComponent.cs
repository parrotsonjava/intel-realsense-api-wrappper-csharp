using IntelRealSenseStart.Code.RealSense.Data.Determiner;

namespace IntelRealSenseStart.Code.RealSense.Component.Determiner.Face
{
    public interface FaceComponent
    {
        void Configure(PXCMFaceConfiguration moduleConfiguration);

        void Process(PXCMFaceData.Face face, FaceDeterminerData.Builder faceDeterminerData);

        void Stop(PXCMFaceData faceData);
    }
}