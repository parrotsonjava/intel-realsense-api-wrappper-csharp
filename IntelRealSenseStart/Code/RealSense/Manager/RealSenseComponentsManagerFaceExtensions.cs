using IntelRealSenseStart.Code.RealSense.Component.Determiner;

namespace IntelRealSenseStart.Code.RealSense.Manager
{
    public static class RealSenseComponentsManagerFaceExtensions
    {
        public static void RegisterFaces(this RealSenseComponentsManager manager)
        {
            manager.CheckIfReady();
            manager.GetComponent<FaceDeterminerComponent>().RegisterFaces();
        }

        public static void UnregisterFaces(this RealSenseComponentsManager manager)
        {
            manager.CheckIfReady();
            manager.GetComponent<FaceDeterminerComponent>().UnregisterFaces();
        }
    }
}