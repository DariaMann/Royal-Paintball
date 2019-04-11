using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Wall
    {
        public float[] Pos { get; set; }
        public int Count { get; set; }

        public Wall(float x, float y, int count)
        {

            this.Count = count;
            this.Pos = new float[2] { x, y };
        }

    }
}
