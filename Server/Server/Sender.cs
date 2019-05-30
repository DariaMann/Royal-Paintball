using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace Server
{
    public class Sender
    {
        public List<Client> clients;
        private Thread thread;
        private volatile bool stopped;
        public bool Game;
        private readonly ConcurrentQueue<Client> ForSender;

        public Sender(ConcurrentQueue<Client> ForSender)
        {
            this.clients = new List<Client>();
            this.stopped = true;
            Game = false;
            this.ForSender = ForSender;

        }

        public void SendMessage(NetworkStream stream, string readyMess)
        {
            byte[] messageBytes = Encoding.ASCII.GetBytes(readyMess);
            int length = messageBytes.Length;// определение длины сообщения
            byte[] lengthBytes = System.BitConverter.GetBytes(length);// преобразование длины в байты с помощью BitConverter (закодировать)
            if (System.BitConverter.IsLittleEndian)// переворот байтов, если это little-endian система с прямым порядком байтов: для этого обращаем байты в lengthBytes
            {
                Array.Reverse(lengthBytes);
            }
            try
            {
                stream.Write(lengthBytes, 0, lengthBytes.Length);//отправка длинны сообщения
                stream.Write(messageBytes, 0, length);//отправка сообщения
            }
            catch
            {
                Console.WriteLine("MISTACEN");
            }
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
                if (this.ForSender.TryDequeue(out Client client))
                {
                    clients.Add(client);
                }
                Thread.Sleep(50);
                for (int i = 0; i < clients.Count; i++)
                {
                    try
                    {
                        NetworkStream stream = clients[i].client.GetStream();
                        int countNamedClient = 0;
                        for (int j = 0; j < clients.Count; j++)
                        {
                            countNamedClient++;
                        }
                        var mess = JsonConvert.SerializeObject(countNamedClient, Formatting.Indented);
                        string readyMess = "%" + mess + "&";
                        //Console.WriteLine("Sender2: " + readyMess + " to " + i + " Player");
                        SendMessage(stream, readyMess);
                        if (Game)
                        {
                            clients.Clear();
                            Game = false;
                        }
                    }
                    catch (System.InvalidOperationException e)
                    {
                        Console.WriteLine(e);
                        clients[i].Stop();
                        clients.Remove(clients[i]);
                    }
                }
            }
        }
    }
}
