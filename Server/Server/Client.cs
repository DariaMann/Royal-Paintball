using System.Net.Sockets;
using System;
using System.Threading;
using GameLibrary;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace Server
{
    public class Client
    {
        public TcpClient client { get; set; }
        public string Name { get; set; }
        public int ID { get; set; }
        DateTime ConnectTime { get; set; }
        public bool Game = false;
        public ConcurrentQueue<Player> queue;

        private Thread thread;
        private volatile bool stopped;

        public Client(TcpClient client)
        {
            this.client = client;
            this.stopped = true;
            this.ConnectTime = DateTime.Now;
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

        private byte[] ReadBytes(int count, NetworkStream networkStream)
        {
            byte[] bytes = new byte[count]; // буфер для заполнения (и позже вернуть)
            int readCount = 0; // байты пуст в начале
            while (readCount < count)// пока буфер не заполнен
            {
                int left = count - readCount;// просить не больше, чем количество оставшихся байтов для заполнения нашего byte[]//мы будем запрашивать байты `left`
                try { 
                int r = networkStream.Read(bytes, readCount, left); // но нам даются байты` r` (`r` <=` left`)
                if (r == 0)
                {
                    //throw new Exception("Lost Connection during read");// I lied, in the default configuration, a read of 0 can be taken to indicate a lost connection
                 //   client.Remove(client);
                   // Stop();
                }
                readCount += r; // advance by however many bytes we read
                }
                catch { Stop(); }
            }
            return bytes;
        }

        public string ReadMessage(NetworkStream networkStream)
        {
            byte[] lengthBytes = ReadBytes(sizeof(int), networkStream);// чтение длины байтов и переворот, если необходимо
            if (System.BitConverter.IsLittleEndian)
            {
                Array.Reverse(lengthBytes);
            }
            int length = System.BitConverter.ToInt32(lengthBytes, 0);
            byte[] messageBytes = ReadBytes(length, networkStream);// чтение сообщения в байтах
            string message = System.Text.Encoding.ASCII.GetString(messageBytes);// расшифровка сообщения
            return message;
        }

        public void Process()
        {
            NetworkStream stream = client.GetStream();
            while (!stopped)
            {
                if (Game == false)
                {
                    string message = ReadMessage(stream);
                    
                        message = message.Substring((message.IndexOf("%") + 1), (message.IndexOf("&") - 1));
                   try
                    {
                        string msg = JsonConvert.DeserializeObject<string>(message);
                   
                    Console.WriteLine("CLIENT: " + message);
                    Name = msg;//message.Substring((message.IndexOf("%") + 1), (message.IndexOf("&") - 1));
                    }
                    catch(Newtonsoft.Json.JsonReaderException e)
                    {
                        Console.WriteLine(e);
                        Game = true;
                    }
                }
                else
                {
                    string message = ReadMessage(stream);
                    string command = message.Substring((message.IndexOf("%") + 1), (message.IndexOf("&") - 1));
                 //   Console.WriteLine("CLIENT: " + command);
                    Player player;
                    try
                    {
                        player = JsonConvert.DeserializeObject<Player>(command);

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        player = new Player();
                        player.ID = ID;
                    }
                    if (queue.Count < 1)
                    { this.queue.Enqueue(player); }
                    if(player.Death)
                    {
                        Stop();
                    }
                }
            }
        }
    }
}
