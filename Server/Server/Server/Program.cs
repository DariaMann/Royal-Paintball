using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Newtonsoft.Json;

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

            ConcurrentQueue<Player> queue = new ConcurrentQueue<Player>();//data from client
            ConcurrentQueue<Field> dataForSend = new ConcurrentQueue<Field>();//data from server
            List<Producer> producers = new List<Producer>();
            List<TcpClient> clientTSP = new List<TcpClient>();
            // устанавливаем метод обратного вызова
            Field f = new Field();
            // диалог сервера с клиентами

            Consumer consumer = new Consumer(f, queue,dataForSend);

            Sender sender = new Sender( dataForSend, clientTSP);

            sender.Start();

            consumer.Start();

            while (true)//поправить
            {
                
                // Получаем входящее подключение
                TcpClient client = server.AcceptTcpClient();
                
                Console.WriteLine("Подключен клиент. Выполнение запроса...");
                
                Producer producer = new Producer(client,f,queue,dataForSend);

                clientTSP.Add(client);

                producers.Add(producer);

                 producer.Start();
            }
        }
    }
}