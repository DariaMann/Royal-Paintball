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

        private byte[] ReadBytes(int count)
        {
            NetworkStream networkStream = client.GetStream();

            byte[] bytes = new byte[count]; // buffer to fill (and later return)
            int readCount = 0; // bytes is empty at the start

            // while the buffer is not full
            while (readCount < count)
            {
                // ask for no-more than the number of bytes left to fill our byte[]
                int left = count - readCount; // we will ask for `left` bytes
                int r = networkStream.Read(bytes, readCount, left); // but we are given `r` bytes (`r` <= `left`)

                if (r == 0)
                { // I lied, in the default configuration, a read of 0 can be taken to indicate a lost connection
                    throw new Exception("Lost Connection during read");
                }

                readCount += r; // advance by however many bytes we read
            }

            return bytes;
        }

        private string ReadMessage()
        {
            // read length bytes, and flip if necessary
            byte[] lengthBytes = ReadBytes(sizeof(int)); // int is 4 bytes
            if (System.BitConverter.IsLittleEndian)
            {
                Array.Reverse(lengthBytes);
            }

            // decode length
            int length = System.BitConverter.ToInt32(lengthBytes, 0);

            // read message bytes
            byte[] messageBytes = ReadBytes(length);

            // decode the message
            string message = System.Text.Encoding.ASCII.GetString(messageBytes);

            return message;
        }

        public void Process()
        {
            NetworkStream stream = null;
            stream = client.GetStream();
            while (true)
            {
                string message = ReadMessage();
                string command = message.Substring((message.IndexOf("%") + 1), (message.IndexOf("&") - 1));
                Player jsonData1 = new Player();
                try
                {
                    jsonData1 = JsonConvert.DeserializeObject<Player>(command);

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

