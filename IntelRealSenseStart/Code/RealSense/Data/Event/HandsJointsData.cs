using System.Collections.Generic;

namespace IntelRealSenseStart.Code.RealSense.Data.Event
{
    public class HandsJointsData
    {
        private readonly List<HandJointsData> hands;

        private HandsJointsData()
        {
            hands = new List<HandJointsData>();
        }

        public List<HandJointsData> Hands
        {
            get { return hands; }
        }

        public class Builder
        {
            private readonly HandsJointsData handsJointsData;

            public Builder()
            {
                handsJointsData = new HandsJointsData();
            }

            public Builder WithFaceLandmarks(HandJointsData.Builder detectionsPoints)
            {
                handsJointsData.hands.Add(detectionsPoints.Build());
                return this;
            }

            public HandsJointsData Build()
            {
                return handsJointsData;
            }
        }
    }
}