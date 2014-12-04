using System;

namespace IntelRealSenseStart.Code.RealSense.Exception
{
    public class IllegalStateException : System.Exception
    {
        public IllegalStateException(String message) : base(message)
        {
        }
    }
}