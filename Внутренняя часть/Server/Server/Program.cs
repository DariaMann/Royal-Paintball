using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Server
{
    class Program
    {
      
        //public static void count()
        //  {
        //      Field fds = new Field();
        //      GameController cont = new GameController(fds);
        //      while (true)
        //      {
        //          if (dasha.Count != 0)
        //          {

        //              if (dasha[ID]["dir"] != "N")
        //              {
        //                  cont.MovePlayer(Convert.ToInt32(ID), dasha[ID]["dir"]);
        //              }
        //              if (Convert.ToInt32(dasha[ID]["life"]) <= Convert.ToInt32("0"))
        //              {
        //                  cont.FinishGame(Convert.ToInt32(ID));
        //              }
        //              if (dasha[ID]["shoot"] == "T")
        //              {
        //                  cont.Shoot(dasha[ID]["weapon"], Convert.ToInt32(ID));
        //              }
        //          }


        //          // Console.WriteLine(dasha);
        //          // Console.WriteLine(PosPlayer(ID)); 
        //          Thread.Sleep(300);
        //      }
        //  }
       
        const int port = 904; // порт для прослушивания подключений
        static TcpListener server;
        static void Main(string[] args)
        {
            server = new TcpListener(IPAddress.Parse("127.0.0.1"), port);

                // запуск слушателя
                server.Start();
            Console.WriteLine("Ожидание подключений... ");
                
            // диалог сервера с клиентами
            while (true)
            {
                // Получаем входящее подключение
                TcpClient client = server.AcceptTcpClient();
                
                Console.WriteLine("Подключен клиент. Выполнение запроса...");
                ClientObject clientObject = new ClientObject(client);


                //Thread myT3 = new Thread(clientObject.count());
                //myT3.Start();

                // создаем новый поток для обслуживания нового клиента
                Thread clientThread = new Thread(new ThreadStart(clientObject.Process));
                clientThread.Start();
                
            }
        }
        
    }
}