//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Net.Sockets;
//using System.Net;

//namespace Server
//{
//    class Program
//    {
//        //static Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
//        //static void Main(string[] args)
//        //{

//        //    socket.Bind(new IPEndPoint(IPAddress.Any, 904));
//        //    while (true)
//        //    {
//        //    socket.Listen(5);
//        //    Socket client = socket.Accept();
//        //    Console.WriteLine("Новое подключение");

//        //        byte[] buffer = new byte[1024];
//        //    client.Receive(buffer);
//        //    Console.WriteLine(Encoding.ASCII.GetString(buffer));
//        //    }
//        //    Console.ReadKey();
//        //}
//        public static void Main()
//        {
//            TcpListener server = null;
//            try
//            {
//                // Set the TcpListener on port 13000.Установите TcpListener на порт 904.
//                Int32 port = 904;
//                IPAddress localAddr = IPAddress.Parse("127.0.0.1");

//                // TcpListener server = new TcpListener(port);
//                server = new TcpListener(localAddr, port);

//                // Start listening for client requests.Начните слушать запросы клиентов.
//                server.Start();

//                // Buffer for reading data Буфер для чтения данных
//                Byte[] bytes = new Byte[256];
//                String data = null;

//                // Enter the listening loop.Войдите в цикл прослушивания.
//                while (true)
//                {
//                    Console.Write("Waiting for a connection... ");

//                    // Perform a blocking call to accept requests.Выполните блокирующий вызов, чтобы принять запросы.
//                    // You could also user server.AcceptSocket() here.Вы также можете использовать server.AcceptSocket () здесь.
//                    TcpClient client = server.AcceptTcpClient();
//                    Console.WriteLine("Connected!");

//                    data = null;

//                    // Get a stream object for reading and writing Получить объект потока для чтения и записи
//                    NetworkStream stream = client.GetStream();

//                    int i;

//                    // Loop to receive all the data sent by the client.Цикл для получения всех данных, отправленных клиентом.
//                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
//                    {
//                        // Translate data bytes to a ASCII string.Преобразуйте байты данных в строку ASCII.
//                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
//                        Console.WriteLine("Received: {0}", data);

//                        // Process the data sent by the client.Обработка данных, отправленных клиентом.
//                        data = data.ToUpper();

//                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

//                        // Send back a response.Отправить ответ.
//                        stream.Write(msg, 0, msg.Length);
//                        Console.WriteLine("Sent: {0}", data);

//                        //byte[] data1 = new byte[256];
//                        //// отправляем ответ
//                        //string message = "ваше сообщение доставлено";
//                        //data1 = Encoding.Unicode.GetBytes(message);
//                        //socket.Send(data1);
//                        //// закрываем сокет
//                        //socket.Shutdown(SocketShutdown.Both);
//                        //socket.Close();
//                        //          string ms = Console.ReadLine();
//                        //byte[] msCl = Encoding.ASCII.GetBytes(ms);
//                        //    Console.WriteLine("Sending back : " + ms);
//                        //    stream.Write(msCl, 0, msCl.Length);
//                    }

//                    // Shutdown and end connection.Отключение и завершение соединения.
//                    client.Close();
//                }
//            }
//            catch (SocketException e)
//            {
//                Console.WriteLine("SocketException: {0}", e);
//            }
//            finally
//            {
//                // Stop listening for new clients.Хватит слушать новых клиентов.
//                server.Stop();
//            }


//            Console.WriteLine("\nHit enter to continue...");
//            Console.Read();
//        }
//    }
//}

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TcpListenerApp
{
    class Program
    {
       static public string PosPlayer()
        {
            double x = 2;
            double y = 1;
            double z = -2.77;
            string mes = "2,1,-2,77";
            return mes;

        }
        const int port = 904; // порт для прослушивания подключений
        static void Main(string[] args)
        {
            TcpListener server = null;
            //try
            //{
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                server = new TcpListener(localAddr, port);

                // запуск слушателя
                server.Start();
                // Buffer for reading data Буфер для чтения данных
                Byte[] bytes = new Byte[256];
                String data1 = null;
                while (true)
                {
                    Console.WriteLine("Ожидание подключений... ");

                    // получаем входящее подключение
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Подключен клиент. Выполнение запроса...");

                    data1 = null;

                    // получаем сетевой поток для чтения и записи
                    NetworkStream stream = client.GetStream();

                    int i;

                //Цикл для получения всех данных, отправленных клиентом.
                //while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                //{
                //    //Преобразуйте байты данных в строку ASCII.
                //    data1 = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                //    Console.WriteLine("Сообщение: {0}", data1);

                //    // Обработка данных, отправленных клиентом.
                //    data1 = data1.ToUpper();

                //    byte[] msg = System.Text.Encoding.ASCII.GetBytes(data1);

                //    // Отправить ответ.
                //    stream.Write(msg, 0, msg.Length);
                //    Console.WriteLine("Ответ: {0}", data1);
                //}




                // сообщение для отправки клиенту
                string response; response = PosPlayer();
                    // преобразуем сообщение в массив байтов
                    byte[] data = Encoding.UTF8.GetBytes(response);

                    // отправка сообщения
                    stream.Write(data, 0, data.Length);
                    Console.WriteLine("Отправлено сообщение: {0}", response);
                    // закрываем поток
                    stream.Close();
                    // закрываем подключение
                    client.Close();
                
                    }
               // }
            
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //}
            //finally
            //{
            //    if (server != null)
            //        server.Stop();
            //}
        }
    }
}