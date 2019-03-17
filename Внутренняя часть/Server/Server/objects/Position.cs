using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Position
    {
        public Position()
        {
            Random rn = new Random(); // объявление переменной для генерации чисел
            this.X = rn.Next(-2, 5);
            this.Y = rn.Next(-2, 5);
            this.Z = -2.77;
        }

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
    }
}
