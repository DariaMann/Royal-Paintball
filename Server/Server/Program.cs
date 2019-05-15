using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Concurrent;
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

            ConcurrentQueue<Player> queue = new ConcurrentQueue<Player>();

            ConcurrentQueue<Field> dataForSend = new ConcurrentQueue<Field>();//data from server

            // устанавливаем метод обратного вызова
            Field f = new Field();
            // диалог сервера с клиентами

            Consumer consumer = new Consumer(f, queue, dataForSend);

            consumer.Start();

            while (true)//поправить
            {
                
                // Получаем входящее подключение
                TcpClient client = server.AcceptTcpClient();
                
                Console.WriteLine("Подключен клиент. Выполнение запроса...");
                
                Producer producer = new Producer(client,f,queue, dataForSend);//Producer

                 producer.Start();
                ////// создаем новый поток для обслуживания нового клиента
                //Thread clientThread = new Thread(new ThreadStart(producer.Process));
                //clientThread.Start();

            }
        }
        
        
    }
}