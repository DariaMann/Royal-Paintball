using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Rotation
    {
        public Rotation()
        {
            this.X =-90;
            this.Y = 0;
            this.Z = 0;

        }

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
    }
}
