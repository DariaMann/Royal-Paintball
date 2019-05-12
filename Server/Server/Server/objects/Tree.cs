using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
   public class Tree
    {
        public float X { get; set; }
        public float Y { get; set; }
        public int[] Size { get; set; }
        public string Type;

        public Tree(float x,float y,string type)
        {
            this.X = x;
            this.Y = y;
            this.Type = type;
            Size = new int[2] { 4, 2 };
        }
    }
}
