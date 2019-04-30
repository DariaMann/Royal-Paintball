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

        public Tree()
        {
            Size = new int[2] { 4, 2 };
        }

    }
}
