using System.Net.Sockets;
using System.Collections.Generic;
using System.Collections.Concurrent;
using GameLibrary;
using System;

namespace Server
{
    class Game
    {
        private ConcurrentQueue<Player> queue;
        ConcurrentQueue<Field> dataForSend;
        Field f;
        Logic consumer;
        Sender sender;
        List<Client> clients;
        public List<FirstPlayerData> Data;

        public Game(List<Client> clients)
        {
            this.clients = clients;
            this.Data = new List<FirstPlayerData>();
            foreach(Client cl in clients)
            {
                Data.Add(new FirstPlayerData(cl.Name, cl.ID));
            }
            this.queue = new ConcurrentQueue<Player>();
            this.dataForSend = new ConcurrentQueue<Field>();
            this.f = new Field(Data);
            this.consumer = new Logic(f, queue, dataForSend);
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
