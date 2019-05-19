using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using GameLibrary;

namespace Server
{
    class Waiting
    {
        private ConcurrentQueue<TcpClient> Wishing;
        private Thread thread;
        private volatile bool stopped;
        private int GamersCount = 2;
        private List<Game> Games;
        private Dictionary<TcpClient, Waiter> Waiters;
        private List<Waiter> waitPerson;

        public Waiting(ConcurrentQueue<TcpClient> Wishing)
        {
            this.Wishing = Wishing;
            Games = new List<Game>();
            waitPerson = new List<Waiter>();
            Waiters = new Dictionary<TcpClient, Waiter>();
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
            string waiterName = JsonConvert.DeserializeObject<string>(message);
            return waiterName;
        }

        public void SendMessage(NetworkStream stream)
        {
            byte[] data = new byte[256]; // буфер для получаемых данных
            var waiters = JsonConvert.SerializeObject(waitPerson, Formatting.Indented);
            Console.WriteLine("СЕРВЕР: " + waiters);
            data = Encoding.UTF8.GetBytes(waiters);
            stream.Write(data, 0, data.Length);
        }

        public void Process()
        {
            NetworkStream stream = null;
            
            while (!stopped)
            {
                if (Waiters.Count >= GamersCount)
                {
                    Dictionary<TcpClient, Waiter> newDic = new Dictionary<TcpClient, Waiter>();
                    newDic = Waiters;
                    Game game = new Game(newDic);
                    game.Process();
                    Games.Add(game);
                    Waiters.Clear();
                    waitPerson.Clear();
                }
                else
                {
                    foreach (TcpClient client in Wishing)
                    {
                        stream = client.GetStream();
                        string waiterName = AcceptMessage(stream);//получение сообщения

                        if (!Waiters.ContainsKey(client)) // отправляем обратно сообщение 
                        {
                            Waiter w = new Waiter(waiterName, DateTime.Now);
                            Waiters.Add(client, w);
                            waitPerson.Add(w);
                            if (waitPerson.Count >= GamersCount)
                            {
                                while (!Wishing.IsEmpty)
                                {
                                    if (this.Wishing.TryDequeue(out TcpClient clientTcp))
                                    {
                                        stream = clientTcp.GetStream();
                                        SendMessage(stream);
                                    }
                                }
                            }
                            else
                            {
                                SendMessage(stream);
                            }
                        }
                        else
                        {
                            SendMessage(stream);
                        }

                    }

                }
            }
        }
    }
}
