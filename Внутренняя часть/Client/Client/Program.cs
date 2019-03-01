//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Net.Sockets;
//using System.Net;

//namespace Client
//{
//    class Program
//    {
//        static Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
//        static void Main(string[] args)
//        {
//            socket.Connect("127.0.0.1", 904);
//            while (true)
//            {
//                string message = Console.ReadLine();
//                byte[] buffer = Encoding.ASCII.GetBytes(message);
//                socket.Send(buffer);

//                // получаем ответ
//                buffer = new byte[256]; // буфер для ответа
//                StringBuilder builder = new StringBuilder();
//                int bytes = 0; // количество полученных байт

//                do
//                {
//                    bytes = socket.Receive(buffer, buffer.Length, 0);
//                    builder.Append(Encoding.Unicode.GetString(buffer, 0, bytes));
//                }
//                while (socket.Available > 0);
//                Console.WriteLine("ответ сервера: " + builder.ToString());
//                //socket.Shutdown(SocketShutdown.Both);
//                //socket.Close();
//            }
//            Console.ReadKey();
//        }
//    }
//}
using System;
using System.Net.Sockets;
using System.Text;

namespace TcpClientApp
{
    class Program
    {
        private const int port = 904;
        private const string server = "127.0.0.1";
      //  static Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        static void Main(string[] args)
        {
            //try
            //{
                TcpClient client = new TcpClient();
                client.Connect(server, port);
            //while (true)
            //{
            //    string message = Console.ReadLine();
            //    byte[] buffer = Encoding.ASCII.GetBytes(message);
            //    NetworkStream streams = client.GetStream();
            //    streams.Write(buffer, 0, buffer.Length);
            //    //socket.Send(buffer);
            //}

                byte[] data = new byte[256];
                StringBuilder response = new StringBuilder();
                NetworkStream stream = client.GetStream();

                do
                {
                    int bytes = stream.Read(data, 0, data.Length);
                    response.Append(Encoding.UTF8.GetString(data, 0, bytes));
                }
                while (stream.DataAvailable); // пока данные есть в потоке

                Console.WriteLine(response.ToString());

                // Закрываем потоки
                stream.Close();
                client.Close();

                //}
                //catch (SocketException e)
                //{
                //    Console.WriteLine("SocketException: {0}", e);
                //}
                //catch (Exception e)
                //{
                //    Console.WriteLine("Exception: {0}", e.Message);
                //}

                Console.WriteLine("Запрос завершен...");
            //}
            Console.Read();
        }
    }
}