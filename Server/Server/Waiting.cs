using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Waiting
    {
        private ConcurrentQueue<Client> Waiters;
        private Thread thread;
        private volatile bool stopped;
        private ConcurrentQueue<Client> ForSender;
        private int GamersCount = 2;
        private List<Game> Games;
        private List<Client> clients;
        Sender sender;

        public Waiting(ConcurrentQueue<Client> Waiters)
        {
            Console.Write("Введие количество игроков: ");
            GamersCount = Convert.ToInt32(Console.ReadLine());
            this.Waiters = Waiters;
            Games = new List<Game>();
            clients = new List<Client>();
            this.stopped = true;
            ForSender = new ConcurrentQueue<Client>();
            sender = new Sender(ForSender);
           
            sender.Start();
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

        public int CreateID()
        {
            int id = clients.Count;
            return id;
        }

        public void Process()
        {
            while (!stopped)
            {
                if (clients.Count >= GamersCount)
                {
                    List<Client> newList = new List<Client>(clients);
                    Game game = new Game(newList);
                    game.Process();
                    Games.Add(game);
                    //foreach (Client tcp in clients)
                    //{ sender.clients.Remove(tcp); }
                    clients.Clear();
                    Thread.Sleep(100);
                    sender.Game = true;
                    
                }
                else
                {
                    if (this.Waiters.TryDequeue(out Client client))
                    {
                        client.ID = CreateID();
                        client.Start();
                        clients.Add(client);
                        ForSender.Enqueue(client);
                    }
                }
                for(int i = 0;i<Games.Count;i++)
                {
                    if(Games[i].clients.Count<=0)
                    {
                        Games.Remove(Games[i]);
                    }
                }
            }
        }
    }
}


