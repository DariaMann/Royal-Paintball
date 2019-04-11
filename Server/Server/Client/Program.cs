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
            socket.Connect("127.0.0.1", 13000);
            while (true)
            {
                string message = Console.ReadLine();
                byte[] buffer = Encoding.ASCII.GetBytes(message);
                socket.Send(buffer);
            }
            Console.ReadKey();
        }
    }
}
