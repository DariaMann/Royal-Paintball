using System;
using System.Net.Sockets;
using System.Threading;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using GameLibrary;
using System.Collections.Generic;

namespace Server
{
    public class Producer
    {
        static public int ID = 555;
        public TcpClient client;

        private readonly ConcurrentQueue<Player> queue;
        private readonly ConcurrentQueue<string> dataForSend;

        private Thread thread;
        private volatile bool stopped;
        List<TcpClient> clients;

        public Producer(TcpClient tcpClient, ConcurrentQueue<Player> queue)
        {
            this.queue = queue;
            this.client = tcpClient;
            this.stopped = true;
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
            while (readCount < count)// while the buffer is not full
            {
                int left = count - readCount; // ask for no-more than the number of bytes left to fill our byte[]// we will ask for `left` bytes
                try
                {
                    int r = networkStream.Read(bytes, readCount, left); // but we are given `r` bytes (`r` <= `left`)
                    if (r == 0)// lost connection
                    {
                        //  throw new Exception("Lost Connection during read");
                        clients.Remove(client);
                        Stop();
                    }
                
                readCount += r; // advance by however many bytes we read
                }
                catch
                {
                    clients.Remove(client);
                    Stop();
                }
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
                catch(Exception e) { Console.WriteLine(e); }
                this.queue.Enqueue(jsonData1);
            }

        }

    }

}

