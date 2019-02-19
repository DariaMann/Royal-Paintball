using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace Client
{
    class Program
    {
        static Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        static void Main(string[] args)
        {
            socket.Connect("127.0.0.1", 904);
            while (true)
            {
                string message = Console.ReadLine();
                byte[] buffer = Encoding.ASCII.GetBytes(message);
                socket.Send(buffer);

                // получаем ответ
                buffer = new byte[256]; // буфер для ответа
                StringBuilder builder = new StringBuilder();
                int bytes = 0; // количество полученных байт

                do
                {
                    bytes = socket.Receive(buffer, buffer.Length, 0);
                    builder.Append(Encoding.Unicode.GetString(buffer, 0, bytes));
                }
                while (socket.Available > 0);
                Console.WriteLine("ответ сервера: " + builder.ToString());
                //socket.Shutdown(SocketShutdown.Both);
                //socket.Close();
            }
            Console.ReadKey();
        }
    }
}
