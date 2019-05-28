﻿using System;
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
        private int GamersCount = 2;
        private List<Game> Games;
        private List<Client> clients;
        public ConcurrentQueue<Client> ClientForSender;
        Sender sender;

        public Waiting(ConcurrentQueue<Client> Waiters)
        {
            this.Waiters = Waiters;
            Games = new List<Game>();
            clients = new List<Client>();
            this.stopped = true;
            ClientForSender = new ConcurrentQueue<Client>();
            sender = new Sender(ClientForSender);
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
                //Console.WriteLine("Games: "+Games.Count);
                if (clients.Count >= GamersCount)
                {
                    List<Client> newList = new List<Client>(clients);
                    Game game = new Game(newList);
                    game.Process();
                    Games.Add(game);
                    foreach (Client tcp in clients)
                    { sender.client.Remove(tcp); }
                    clients.Clear();
                }
                else
                {
                    if (this.Waiters.TryDequeue(out Client client))
                    {
                        client.ID = CreateID();
                        client.Start();
                        clients.Add(client);
                        ClientForSender.Enqueue(client);
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


