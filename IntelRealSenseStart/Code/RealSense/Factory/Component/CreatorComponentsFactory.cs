using IntelRealSenseStart.Code.RealSense.Component.Creator;

namespace IntelRealSenseStart.Code.RealSense.Factory.Component
{
    public class CreatorComponentsFactory
    {
        private readonly RealSenseFactory realSenseFactory;

        public CreatorComponentsFactory(RealSenseFactory realSenseFactory)
        {
            this.realSenseFactory = realSenseFactory;
        }

        public ImageBuilder.Builder HandsImageBuilder()
        {
            return new ImageBuilder.Builder(realSenseFactory);
        }

        public BasicImageCreator.Builder BasicImageCreator()
        {
            return new BasicImageCreator.Builder();
        }

        public HandsImageCreator.Builder HandsImageCreator()
        {
            return new HandsImageCreator.Builder();
        }

        public FaceImageCreator.Builder FaceImageCreator()
        {
            return new FaceImageCreator.Builder();
        }

        public OverallImageCreator.Builder OverallImageCreator()
        {
            return new OverallImageCreator.Builder();
        }

        public FacesLandmarksBuilder.Builder FacesLandmarksBuilder()
        {
            return new FacesLandmarksBuilder.Builder(realSenseFactory);
        }

        public HandsJointsBuilder.Builder HandsJointsBuilder()
        {
            return new HandsJointsBuilder.Builder(realSenseFactory);
        }
    }
}
