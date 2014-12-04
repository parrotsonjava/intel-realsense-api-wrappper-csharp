namespace IntelRealSenseStart.Code.RealSense.Helper
{
    internal static class RangeExtensions
    {
        public static Range To(this int start, int end)
        {
            var range = new Range();
            for (int i = start; i <= end; i++)
            {
                range.Add(i);
            }
            return range;
        }

        public static Range AsRange(this int value)
        {
            Range range = new Range();
            range.Add(value);
            return range;
        }
    }
}