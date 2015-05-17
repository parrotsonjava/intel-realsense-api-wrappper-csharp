using System;

namespace IntelRealSenseStart.Code.RealSense.Exception
{
    class RealSenseInitializationException : System.Exception
    {
        public RealSenseInitializationException(String message)
            : base(message)
        {
        }
    }
}