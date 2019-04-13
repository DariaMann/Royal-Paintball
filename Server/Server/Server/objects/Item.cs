using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Item
    {
        public string Name { get; set; }
        public int Count { get; set; }

        public Item(string name,int count)
        {
            this.Name = name;
            this.Count = count;

        }
    }
}
