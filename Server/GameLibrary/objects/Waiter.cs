using System;
using System.Net.Sockets;

namespace GameLibrary
{
    public class Waiter
    {
        public string Name { get; set; }
        public DateTime ConnectTime { get; set; }
        public bool InGame { get; set; }

        public Waiter(string name, DateTime ConnectTime)
        {
            this.Name = name;
            this.ConnectTime = ConnectTime;
            InGame = false;
        }
    }
}
