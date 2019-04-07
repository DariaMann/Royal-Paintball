using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

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
            // устанавливаем метод обратного вызова
            Field f = new Field();
            // диалог сервера с клиентами
            while (true)
            {
                // Получаем входящее подключение
                TcpClient client = server.AcceptTcpClient();
                
                Console.WriteLine("Подключен клиент. Выполнение запроса...");
               

                ClientObject clientObject = new ClientObject(client,f);
               
                // создаем новый поток для обслуживания нового клиента
                Thread clientThread = new Thread(new ThreadStart(clientObject.Process));
                clientThread.Start();
                
            }
        }
        
        
    }
}