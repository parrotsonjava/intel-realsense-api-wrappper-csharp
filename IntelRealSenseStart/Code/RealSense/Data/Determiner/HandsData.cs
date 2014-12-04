using System.Collections.Generic;
using System.Linq;

namespace IntelRealSenseStart.Code.RealSense.Data
{
    public class HandsData
    {
        private readonly List<HandData> hands;

        public HandsData()
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

            public Builder WithHand(HandData.Builder handData)
            {
                handsData.Hands.Add(handData.Build());
                return this;
            }

            public Builder WithHands(IEnumerable<HandData.Builder> handData)
            {
                handsData.Hands.AddRange(handData.Select(handDataBuilder => handDataBuilder.Build()));
                return this;
            }

            public HandsData Build()
            {
                return handsData;
            }
        }
    }
}