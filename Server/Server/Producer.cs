using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using GameLibrary;
using System.Collections.Generic;

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
        List<TcpClient> clients;

        public Producer(TcpClient tcpClient, Waiter waiter, ConcurrentQueue<Player> queue, ConcurrentQueue<string> dataForSend, List<TcpClient> clients)
        {
            this.dataForSend = dataForSend;
            this.queue = queue;
            this.client = tcpClient;
            this.stopped = true;
            this.waiter = waiter;
            this.clients = clients;
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
                //if (jsonData1.ID == -1)
                //// отправляем обратно сообщение 
                //{
                //    if (this.dataForSend.TryDequeue(out string mess))
                //    {
                //        string msg = "%" + mess + "&";
                //        byte[] messageBytes = Encoding.ASCII.GetBytes(msg); // a UTF-8 encoder would be 'better', as this is the standard for network communications
                //        int length = messageBytes.Length;// determine length of message
                //        byte[] lengthBytes = System.BitConverter.GetBytes(length);// convert the length into bytes using BitConverter (encode)
                //        if (System.BitConverter.IsLittleEndian)// flip the bytes if we are a little-endian system: reverse the bytes in lengthBytes to do so
                //        {
                //            Array.Reverse(lengthBytes);
                //        }
                //        stream.Write(lengthBytes, 0, lengthBytes.Length);// send length
                //        stream.Write(messageBytes, 0, length);// send message
                //    }
                //}
               // Console.WriteLine("Сообщение от " + jsonData1.Name);
               // Console.WriteLine(command);
                this.queue.Enqueue(jsonData1);
            }

        }

    }

}

