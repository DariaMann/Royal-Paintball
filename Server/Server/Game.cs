using System.Net.Sockets;
using System.Collections.Generic;
using System.Collections.Concurrent;
using GameLibrary;

namespace Server
{
    class Game
    {
        private readonly Dictionary<TcpClient, Waiter> Waiters;
        private ConcurrentQueue<Player> queue;
        ConcurrentQueue<string> dataForSend;
        Field f;
        Consumer consumer;
        Sender sender;
        List<TcpClient> clients;

        public Game(Dictionary<TcpClient, Waiter> waiters)
        {
            this.Waiters = waiters;
            this.clients = new List<TcpClient>();
            foreach (TcpClient tcp in Waiters.Keys)
            {
                clients.Add(tcp);
            }
            this.queue = new ConcurrentQueue<Player>();
            this.dataForSend = new ConcurrentQueue<string>();
            this.f = new Field();
            this.consumer = new Consumer(f, queue, dataForSend);
            this.sender = new Sender(dataForSend, clients);

            //foreach (TcpClient c in Waiters.Keys)
            //{
            //    Producer producer = new Producer(c, Waiters[c], queue, dataForSend);//Producer
            //    producer.Start();

            //}
        }

        public void Process()
        {
            consumer.Start();
            sender.Start();
            foreach (TcpClient c in Waiters.Keys)
            {
                Producer producer = new Producer(c, Waiters[c], queue, dataForSend);//Producer
                producer.Start();

            }
        }
    }
}
