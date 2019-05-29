﻿using System.Net.Sockets;
using System;
using System.Threading;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace Server
{
    public class Client
    {
        public TcpClient client { get; set; }
        public int ID { get; set; }
        public bool Game = false;
        public ConcurrentQueue<Player> queue;
        private Thread thread;
        private volatile bool stopped;

        public Client(TcpClient client)//, ConcurrentQueue<Player> queue)
        {
            //this.queue = queue;
            this.client = client;
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

        private byte[] ReadBytes(int count, NetworkStream networkStream)
        {
            byte[] bytes = new byte[count]; // буфер для заполнения (и позже вернуть)
            int readCount = 0; // байты пуст в начале
            while (readCount < count)// пока буфер не заполнен
            {
                int left = count - readCount;// просить не больше, чем количество оставшихся байтов для заполнения нашего byte[]//мы будем запрашивать байты `left`
                try
                {
                    int r = networkStream.Read(bytes, readCount, left); // но нам даются байты` r` (`r` <=` left`)
                    if (r == 0)
                    {

                    }
                    readCount += r; // advance by however many bytes we read
                }
                catch {

                    Console.WriteLine(")))))))))))))))))))))))))))))))))))))))))))");
                    Stop();
                }
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
                Console.WriteLine("Client " + ID);
                if (Game)
                {
                    string message = ReadMessage(stream);
                    string command = message.Substring((message.IndexOf("%") + 1), (message.IndexOf("&") - 1));
                //    Console.WriteLine("CLIENT: " + command);
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

                    Console.WriteLine("Death " + ID + ":" + player.Death);
                }
            }
        }
    }
}
