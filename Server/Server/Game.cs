using System.Net.Sockets;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System;

namespace Server
{
    class Game
    {
        private ConcurrentQueue<Player> queue;
        ConcurrentQueue<string> dataForSend;
        Field f;
        Consumer consumer;
        SenderGame sender;
        public List<Client> clients;
        public Game(List<Client> clients)
        {
            Console.WriteLine(clients.Count);
            this.clients = clients;
            this.queue = new ConcurrentQueue<Player>();
            this.dataForSend = new ConcurrentQueue<string>();

            this.f = new Field();
            for(int i=0;i<clients.Count;i++)
            {
                f.Player.Add(clients[i].ID, new Player() { ID = clients[i].ID, Color = f.ChooseColor() });
            }
            this.consumer = new Consumer(f, queue, dataForSend);
            this.sender = new SenderGame(dataForSend, clients);
        }
        public void Process()
        {
            consumer.Start();
            sender.Start();
            foreach (Client c in clients)
            {
                c.Game = true;
                c.queue = queue;
            }
        }
    }
}
