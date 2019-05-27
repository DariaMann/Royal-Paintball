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

            //ConcurrentQueue<Player> queue = new ConcurrentQueue<Player>();

            //ConcurrentQueue<Field> dataForSend = new ConcurrentQueue<Field>();//data from server

            //List<Client> clients = new List<Client>();
            //// устанавливаем метод обратного вызова
            //Field f = new Field();
            //// диалог сервера с клиентами

            //Sender sender = new Sender(dataForSend, clients);

            //sender.Start();

            //Consumer consumer = new Consumer(f, queue, dataForSend);

            //consumer.Start();

            while (true)//поправить
            {
                
                // Получаем входящее подключение
                TcpClient client = server.AcceptTcpClient();
                
                Console.WriteLine("Подключен клиент. Выполнение запроса...");

                 queueTCP.Enqueue(client);
                //Client client1 = new Client(client, queue);
                
                //client1.ID = clients.Count;
                //client1.Game = true;
                //client1.Start();

                //f.Player.Add(client1.ID, new Player() { ID = client1.ID, Color = "blue" });

                //clients.Add(client1);
            }
        }
        
        
    }
}