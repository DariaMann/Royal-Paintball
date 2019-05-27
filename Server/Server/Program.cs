using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Server
{
    class Program
    {
      
        const int port = 904; // порт для прослушивания подключений
        static TcpListener server;
        static void Main(string[] args)
        {
            server = new TcpListener(IPAddress.Parse("127.0.0.1"), port);

                // запуск слушателя
                server.Start();
            Console.WriteLine("Ожидание подключений... ");

            ConcurrentQueue<TcpClient> queueTCP = new ConcurrentQueue<TcpClient>();

            Waiting waiting = new Waiting(queueTCP);

            waiting.Start();


            while (true)//поправить
            {
                
                // Получаем входящее подключение
                TcpClient client = server.AcceptTcpClient();
                Console.Clear();
                Console.WriteLine("Подключен клиент. Выполнение запроса...");

                queueTCP.Enqueue(client);
                //Client client1 = new Client(client, queue);

                //client1.ID = clients.Count;
                //client1.Game = true;
                //client1.Start();

                //f.Player.Add(client1.ID, new Player() { ID = client1.ID, Color = "blue" });
                
                //clients.Add(client1);
                //sender.Start();
                //consumer.Start();
                //Game game = new Game(clients);
                //game.Process();
            }
        }
        
        
    }
}