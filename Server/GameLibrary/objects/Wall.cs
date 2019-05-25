using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary
{
    public class Wall:Objects
    {
        //public float X { get; set; }
        //public float Y { get; set; }
        //public int[] Size { get; set; }

        public Wall(float x, float y)
        {
            this.X = x;
            this.Y = y;
            Size = new int[2] { 2, 3 };
        }

    }
}
