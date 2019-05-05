using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Circle
    {
        public float X { get; set; }
        public float Y { get; set; }
        public int[] Size { get; set; }

      public Circle()
        {
            X = 0;
            Y = 0;
            Size = new int[] { 30, 30 };
        }
    }
}
