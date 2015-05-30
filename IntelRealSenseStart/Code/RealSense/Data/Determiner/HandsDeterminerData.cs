using System.Collections.Generic;
using System.Linq;

namespace IntelRealSenseStart.Code.RealSense.Data.Determiner
{
    public class HandsDeterminerData
    {
        private readonly List<HandDeterminerData> hands;

        public HandsDeterminerData()
        {
            hands = new List<HandDeterminerData>();
        }

        public List<HandDeterminerData> Hands
        {
            get { return hands; }
        }

        public class Builder
        {
            private readonly HandsDeterminerData handsDeterminerData;

            public Builder()
            {
                handsDeterminerData = new HandsDeterminerData();
            }

            public Builder WithHand(HandDeterminerData.Builder handData)
            {
                handsDeterminerData.Hands.Add(handData.Build());
                return this;
            }

            public Builder WithHands(IEnumerable<HandDeterminerData.Builder> handData)
            {
                handsDeterminerData.Hands.AddRange(handData.Select(handDataBuilder => handDataBuilder.Build()));
                return this;
            }

            public HandsDeterminerData Build()
            {
                return handsDeterminerData;
            }
        }
    }
}