using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Tree:ICloneable
    {
        public float X { get; set; }
        public float Y { get; set; }
        public int[] Size { get; set; }
        public string Type;

        public Tree(float x, float y, string type)
        {
            this.X = x;
            this.Y = y;
            this.Type = type;
            Size = new int[2] { 4, 2 };
        }
        public object Clone()
        {
            return new Tree(1,1,"")
            {
                X = this.X,
                Y = this.Y,
                Size = (int[])this.Size.Clone(),
                Type = this.Type
            };
        }
    }
}
