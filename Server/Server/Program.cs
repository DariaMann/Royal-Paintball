using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using System.Collections.Generic;
using GameLibrary;

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

            ConcurrentQueue<Player> queue = new ConcurrentQueue<Player>();

            ConcurrentQueue<string> dataForSend = new ConcurrentQueue<string>();//data from server

            List<TcpClient> clientTSP = new List<TcpClient>();

            ConcurrentQueue<TcpClient> Wishing = new ConcurrentQueue<TcpClient>();//data from server

            // устанавливаем метод обратного вызова
            Field f = new Field();
            // диалог сервера с клиентами

            Consumer consumer = new Consumer(f, queue, dataForSend);

            Sender sender = new Sender(dataForSend, clientTSP);

        //    sender.Start();

            consumer.Start();

            while (true)//поправить
            {
                
                // Получаем входящее подключение
                TcpClient client = server.AcceptTcpClient();
                
                Console.WriteLine("Подключен клиент. Выполнение запроса...");
                
                Producer producer = new Producer(client,f,queue, dataForSend);//Producer

                clientTSP.Add(client);

                Wishing.Enqueue(client);

                producer.Start();
            }
        }
        
        
    }
}