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
        private ConcurrentQueue<TcpClient> Waiters;
        private Thread thread;
        private volatile bool stopped;
        private int GamersCount = 2;
        private List<Game> Games;
        private List<Client> clients;
        public ConcurrentQueue<Client> ClientForSender;
        Sender2 sender;

        public Waiting(ConcurrentQueue<TcpClient> Waiters)
        {
            this.Waiters = Waiters;
            Games = new List<Game>();
            clients = new List<Client>();
            this.stopped = true;
            ClientForSender = new ConcurrentQueue<Client>();
            sender = new Sender2(ClientForSender);
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
                    //foreach (Client c in newList)
                    //{ sender.client.Remove(c); }
                    sender.client.Clear();
                    clients.Clear();
                }
                else
                {
                    if (this.Waiters.TryDequeue(out TcpClient clientTcp))
                    {
                        Client client = new Client(clientTcp);
                        client.ID = CreateID();
                        client.Start();
                        clients.Add(client);
                        ClientForSender.Enqueue(client);
                    }
                }
            }
        }
    }
}


