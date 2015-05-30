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

        public FacesBuilder.Builder FacesLandmarksBuilder()
        {
            return new FacesBuilder.Builder(realSenseFactory);
        }

        public HandsBuilder.Builder HandsJointsBuilder()
        {
            return new HandsBuilder.Builder(realSenseFactory);
        }

        public UserIdsImageCreator.Builder UserIdsImageCreator()
        {
            return new UserIdsImageCreator.Builder();
        }

        public EmotionsImageCreator.Builder EmotionsImageCreator()
        {
            return new EmotionsImageCreator.Builder();
        }
    }
}