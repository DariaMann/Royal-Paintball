using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary
{
    public class Item
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public int Index { get; set; }

        public Item(string name, int count, float x, float y, int i)
        {
            this.Name = name;
            this.Count = count;
            this.X = x;
            this.Y = y;
            this.Index = i;

        }
    }
}
