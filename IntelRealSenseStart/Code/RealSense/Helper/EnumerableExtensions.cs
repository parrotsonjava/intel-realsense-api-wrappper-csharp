using System;
using System.Collections.Generic;

namespace IntelRealSenseStart.Code
{
    internal static class EnumerableExtensions
    {
        public static void Do<TSource>(this IEnumerable<TSource> values, Action<TSource> action)
        {
            foreach (TSource value in values)
            {
                action.Invoke(value);
            }
        }
    }
}