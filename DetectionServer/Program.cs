using DetectionServer.Server;

namespace DetectionServer
{
    class Program
    {
        static void Main(string[] args)
        {
            RealSenseDetectionServer.Create().Start();
        }
    }
}