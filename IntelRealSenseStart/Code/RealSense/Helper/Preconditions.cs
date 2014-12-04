using System;
using IntelRealSenseStart.Code.RealSense.Exception;

namespace IntelRealSenseStart.Code.RealSense.Helper
{
    public static class Preconditions
    {
        public static void CheckState<T>(this T obj, Func<T, bool> precondition, String errorMessage)
        {
            if (!precondition.Invoke(obj))
            {
                throw new IllegalStateException(errorMessage);
            }
        }

        public static bool IsNotNull<T>(T obj)
        {
            // ReSharper disable once CompareNonConstrainedGenericWithNull
            return obj != null;
        }
    }
}