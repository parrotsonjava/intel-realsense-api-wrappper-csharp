using System.Collections.Generic;

namespace IntelRealSenseStart.Code
{
    internal class Range
    {
        private readonly List<int> values;

        public Range()
        {
            values = new List<int>();
        }

        public Range Add(int value)
        {
            values.Add(value);
            return this;
        }

        public Range Add(Range range)
        {
            values.AddRange(range.values);
            return this;
        }

        public int[] ToArray()
        {
            return values.ToArray();
        }
    }
}