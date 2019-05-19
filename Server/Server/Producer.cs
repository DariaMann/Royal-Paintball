using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using GameLibrary;

namespace Server
{
    public class Producer//Producer
    {
        static public int ID = 555;
        public TcpClient client;
        public Waiter waiter;

        private readonly ConcurrentQueue<Player> queue;
        private readonly ConcurrentQueue<string> dataForSend;

        private Thread thread;
        private volatile bool stopped;

        public Producer(TcpClient tcpClient, Waiter waiter, ConcurrentQueue<Player> queue, ConcurrentQueue<string> dataForSend)
        {
            this.dataForSend = dataForSend;
            this.queue = queue;
            this.client = tcpClient;
            this.stopped = true;
            this.waiter = waiter;
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

        public string AcceptMessage(NetworkStream stream)
        {
            byte[] data = new byte[256]; // буфер для получаемых данных
            StringBuilder builder = new StringBuilder(); // получаем сообщение
            int bytes = 0;
            do
            {
                bytes = stream.Read(data, 0, data.Length);
                builder.Append(Encoding.UTF8.GetString(data, 0, bytes));
            }
            while (stream.DataAvailable);
            string message = builder.ToString();
            Console.WriteLine("Клиент: " + message);
            return message;
        }

        public void SendMessage(NetworkStream stream, string mess)
        {
            byte[] data = new byte[256]; // буфер для получаемых данных
            Console.WriteLine("СЕРВЕР: " + mess);
            data = Encoding.UTF8.GetBytes(mess);
            stream.Write(data, 0, data.Length);
        }

        public void Process()
        {
            NetworkStream stream = null;
            stream = client.GetStream();
            while (true)
            {
                string message = AcceptMessage(stream);
                Player jsonData1 = new Player();
                try
                {
                    jsonData1 = JsonConvert.DeserializeObject<Player>(message);

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                if (jsonData1 == null)
                {
                    Stop();
                }
                    this.queue.Enqueue(jsonData1);
            }

        }

    }

}

