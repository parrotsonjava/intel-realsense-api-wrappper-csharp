using System;

namespace IntelRealSenseStart.Code.RealSense.Exception
{
    public class RealSenseException : System.Exception
    {
        public RealSenseException(String message) : base(message)
        {
        }
    }
}