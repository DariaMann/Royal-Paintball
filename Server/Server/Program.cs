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

            //server = new TcpListener(IPAddress.Parse("127.0.0.1"), port);

            //// запуск слушателя
            //server.Start();
            //Console.WriteLine("Ожидание подключений... ");

            //List<Client> clients = new List<Client>();//data from server

            //while /*(true)//*/(clients.Count != 3)//поправить
            //{
            //    TcpClient client = server.AcceptTcpClient();

            //    Console.WriteLine("Подключен клиент. Выполнение запроса...");

            //    Client cl = new Client(client);
            //    cl.ID = clients.Count;

            //    cl.Game = true;
            //    cl.Start();

            //    clients.Add(cl);

            //}
            //Game game = new Game(clients);



            // server = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
           
                server = new TcpListener(IPAddress.Parse("192.168.31.163"), port);

            // запуск слушателя
            server.Start();
            Console.WriteLine("Ожидание подключений... ");

            ConcurrentQueue<Client> queueTCP = new ConcurrentQueue<Client>();//очередь подключений клиентов

            Waiting waiting = new Waiting(queueTCP);

            waiting.Start();


            while (true)//поправить
            {
                // Получаем входящее подключение
                TcpClient client = server.AcceptTcpClient();

                Console.WriteLine("Подключен клиент. Выполнение запроса...");

                Client cl = new Client(client);

                queueTCP.Enqueue(cl);
            }
        }
    }
}