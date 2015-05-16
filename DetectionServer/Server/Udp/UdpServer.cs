using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace DetectionServer.Server.Udp
{
    internal class UdpServer
    {
        private readonly char delimiter;

        public event TextEventListener OnText;

        private UdpClient socketServer;
        private volatile bool started;

        private readonly Thread senderThread;

        private IPEndPoint senderEndPoint;
        private readonly IPEndPoint receiverEndPoint;

        private byte[] receivingData;
        private readonly Dictionary<String, String> incomingTexts;

        private byte[] sendingData;
        private readonly Queue<String> sendTextQueue;

        public UdpServer(char delimiter)
        {
            started = false;
            this.delimiter = delimiter;

            receivingData = new byte[1024];
            receiverEndPoint = new IPEndPoint(IPAddress.Any, 0);

            sendTextQueue = new Queue<String>();
            incomingTexts = new Dictionary<string, string>();

            senderThread = new Thread(Send);
        }

        public void Start(int port)
        {
            started = true;
            CreateServer(port);
            StartSendingAndReceiving();
        }

        private void CreateServer(int port)
        {
            senderEndPoint = new IPEndPoint(IPAddress.Any, port);
            socketServer = new UdpClient(senderEndPoint);
        }

        private void StartSendingAndReceiving()
        {
            StartReceiving();
            senderThread.Start();
        }

        public void Stop()
        {
            started = false;

            senderThread.Join();
        }

        private void StartReceiving()
        {
            if (started)
            {
                socketServer.BeginReceive(ReceiveCallback, new object());
            }
        }

        public void ReceiveCallback(IAsyncResult ar)
        {
            GetDataFromSocket(ar);
            ProcessData();

            StartReceiving();
        }
        
        private void GetDataFromSocket(IAsyncResult ar)
        {
            IPEndPoint receiver = new IPEndPoint(IPAddress.Any, 0);
            receivingData = socketServer.EndReceive(ar, ref receiver);
        }

        private void ProcessData()
        {
            String incomingText = Encoding.ASCII.GetString(receivingData, 0, receivingData.Length);
            UpdateTexts(receiverEndPoint, incomingText);
            List<String> linesToSendForSender = ExtractCompleteLinesFor(receiverEndPoint);

            InvokeIncomingText(receiverEndPoint, linesToSendForSender);
        }

        private void UpdateTexts(IPEndPoint sender, String text)
        {
            String key = sender.Address.ToString();

            if (incomingTexts.ContainsKey(key))
            {
                incomingTexts[key] += text;
            }
            else
            {
                incomingTexts[key] = text;
            }
        }

        private List<String> ExtractCompleteLinesFor(IPEndPoint sender)
        {
            String key = sender.Address.ToString();

            String text = incomingTexts[key];
            var lines = text.Split(delimiter);
            incomingTexts[key] = lines[lines.Length - 1];

            List<String> completeLines = new List<String>();
            completeLines.AddRange(lines);
            completeLines.RemoveAt(completeLines.Count - 1);

            return completeLines;
        }

        private void InvokeIncomingText(IPEndPoint sender, List<String> texts)
        {
            if (OnText != null)
            {
                return;
            }

            foreach (var text in texts)
            {
                var eventArgs = TextEventArgs.Create().WithText(text).Build();
                OnText.Invoke(sender, eventArgs);
            }
        }

        private void Send()
        {
            while (started)
            {
                if (sendTextQueue.Count == 0)
                {
                    Thread.Sleep(100);
                    continue;
                }

                SendFirstTextFromQueue();
            }
        }

        private void SendFirstTextFromQueue()
        {
            String text = sendTextQueue.Dequeue();
            sendingData = Encoding.ASCII.GetBytes(text + delimiter);
            socketServer.Send(sendingData, sendingData.Length, "localhost", 9050);
        }

        public void SendText(String text)
        {
            sendTextQueue.Enqueue(text);
        }
    }
}