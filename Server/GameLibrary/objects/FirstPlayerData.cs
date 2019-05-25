using System;
using System.Net.Sockets;

namespace GameLibrary
{
    public class FirstPlayerData
    {
        public string Name { get; set; }
        public int ID { get; set; }

        public FirstPlayerData(string name, int id)
        {
            this.Name = name;
            this.ID = id;
        }
    }

}
