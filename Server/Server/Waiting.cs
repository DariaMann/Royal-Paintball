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
        private List<Client> clientPers;
        public ConcurrentQueue<Client> ClientForSender;
        Sender2 sender;

        List<Client> clientsWithName;

        public Waiting(ConcurrentQueue<TcpClient> Wishing)
        {
            clientsWithName = new List<Client>();
            this.Wishing = Wishing;
            Games = new List<Game>();
            clientPers = new List<Client>();
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
            int id = clientPers.Count;
            return id;
        }

        public void Process()
        {
            while (!stopped)
            {
                foreach(Client c in clientPers)
                {
                    if(c.Name!=null&&c.Name!="null")
                    {
                        if(!clientsWithName.Contains(c))
                        clientsWithName.Add(c);
                    }
                }
                if(clientsWithName.Count>=GamersCount)
                {
                    List<Client> newList = new List<Client>(clientsWithName);
                    Game game = new Game(newList);
                    game.Process();
                    Games.Add(game);
                    foreach (Client c in newList)
                    { sender.client.Remove(c); }
                    clientsWithName.Clear();
                    clientPers.Clear();
                }
                else
                {
                        if (this.Wishing.TryDequeue(out TcpClient clientTcp))
                        {
                            Client client = new Client(clientTcp);
                            client.ID = CreateID();
                            client.Start();
                            clientPers.Add(client);
                            ClientForSender.Enqueue(client);
                        }
                    }
                }
            }
        }
    }

