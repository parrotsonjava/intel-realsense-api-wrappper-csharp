using System;
using System.Threading;
using DetectionServer.Server.Udp;

namespace DetectionServer.Server
{
    public class RealSenseDetectionServer
    {
        private const String MESSAGE = "foo";
        private const char DELIMITER = '\n';

        private readonly UdpServer udpServer;

        public static RealSenseDetectionServer Create()
        {
            return new RealSenseDetectionServer();
        }

        private RealSenseDetectionServer()
        {
            udpServer = new UdpServer(DELIMITER);
        }
        
        public void Start()
        {
            Console.WriteLine(@"Starting");

            udpServer.Start(9050);
            udpServer.OnText += TextFromClient;

            StartSending();
        }

        private void StartSending()
        {
            while (true)
            {
                Thread.Sleep(500);
                
                Console.WriteLine(@"Sending message: " + MESSAGE);
                udpServer.SendText(MESSAGE);
            }
        }

        private void TextFromClient(object sender, TextEventArgs eventArgs)
        {
            Console.WriteLine(@"Received message: " + eventArgs.Text);
        }
    }
}