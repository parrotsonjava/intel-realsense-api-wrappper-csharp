using System.Drawing;
using IntelRealSenseStart.Code.RealSense.Config.Image;
using IntelRealSenseStart.Code.RealSense.Data.Determiner;

namespace IntelRealSenseStart.Code.RealSense.Component.Creator
{
    public interface ImageCreator
    {
        Bitmap Create(Bitmap bitmap, DeterminerData determinerData, ImageCreatorConfiguration imageCreatorConfiguration);
    }
}