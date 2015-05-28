using System.Collections.Generic;

namespace IntelRealSenseStart.Code.RealSense.Data.Event
{
    public class HandsData
    {
        private readonly List<HandData> hands;

        private HandsData()
        {
            hands = new List<HandData>();
        }

        public List<HandData> Hands
        {
            get { return hands; }
        }

        public class Builder
        {
            private readonly HandsData handsData;

            public Builder()
            {
                handsData = new HandsData();
            }

            public Builder WithFaceLandmarks(HandData.Builder detectionsPoints)
            {
                handsData.hands.Add(detectionsPoints.Build());
                return this;
            }

            public HandsData Build()
            {
                return handsData;
            }
        }
    }
}