using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Concurrent;

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
            ConcurrentQueue<TcpClient> Wishing = new ConcurrentQueue<TcpClient>();//data from server
            Waiting wait = new Waiting(Wishing);
            wait.Start();

            while (true)//поправить
            {
                // Получаем входящее подключение
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Подключен клиент. Выполнение запроса...");
                Wishing.Enqueue(client);
            }
        }
    }
}