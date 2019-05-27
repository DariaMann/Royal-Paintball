using System.Net.Sockets;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System;

namespace Server
{
    class Game
    {
        private ConcurrentQueue<Player> queue;
        ConcurrentQueue<Field> dataForSend;
        Field f;
        Consumer consumer;
        Sender sender;
        List<Client> clients;

        public Game(List<Client> clients)
        {
            this.clients = clients;
            this.queue = new ConcurrentQueue<Player>();
            this.dataForSend = new ConcurrentQueue<Field>();

            this.f = new Field();
            for(int i=0;i<clients.Count;i++)
            {
                f.Player.Add(clients[i].ID, new Player() { ID = clients[i].ID });
            }

            this.consumer = new Consumer(f, queue, dataForSend);
            this.sender = new Sender(dataForSend, clients);
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
