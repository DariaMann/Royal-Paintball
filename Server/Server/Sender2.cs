using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using GameLibrary;

namespace Server
{
    class Sender2
    {
        public List<Client> client;
        private Thread thread;
        private volatile bool stopped;
        private ConcurrentQueue<Client> queue;
        public bool Game;

        public Sender2(ConcurrentQueue<Client> client)
        {
            this.client = new List<Client>();
            this.stopped = true;
            queue = client;
            Game = false;
        }

        public void SendMessage(NetworkStream stream, string readyMess)
        {
            //Console.WriteLine("Sender2: " + readyMess);
            byte[] messageBytes = Encoding.ASCII.GetBytes(readyMess); 
            int length = messageBytes.Length;// определение длины сообщения
            byte[] lengthBytes = System.BitConverter.GetBytes(length);// преобразование длины в байты с помощью BitConverter (закодировать)
            if (System.BitConverter.IsLittleEndian)// переворот байтов, если это little-endian система с прямым порядком байтов: для этого обращаем байты в lengthBytes
            {
                Array.Reverse(lengthBytes);
            }
            stream.Write(lengthBytes, 0, lengthBytes.Length);//отправка длинны сообщения
            stream.Write(messageBytes, 0, length);//отправка сообщения
        }

        public void Start()
        {
            if (stopped)
            {
                thread = new Thread(Process);

                stopped = false;

                thread.Start();
            }
        }

        public void Stop()
        {
            if (!stopped)
            {
                this.stopped = true;

                this.thread.Join();
            }
        }

        public void Process()
        {
            while (!stopped)
            {
                Thread.Sleep(50);
                if (this.queue.TryDequeue(out Client clientTcp))
                {
                    client.Add(clientTcp);
                }
                for (int i = 0; i < client.Count; i++)
                {
                    NetworkStream stream = client[i].client.GetStream();
                    int countNamedClient = 0;
                    for (int j = 0; j < client.Count; j++)
                    {
                        if (client[j].Name != null)
                        {
                            countNamedClient++;
                        }
                    }
                    var mess = JsonConvert.SerializeObject(countNamedClient, Formatting.Indented);
                    string readyMess = "%" + mess + "&";
                    SendMessage(stream, readyMess);
                }
            }
        }
    }
}
