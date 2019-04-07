using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Position
    {
        public Position()//(float x,float y, float z)
        {
            Random rn = new Random(); // объявление переменной для генерации чисел
            this.X = rn.Next(-2, 5); //rn.Next(-8, 8);
            this.Y = rn.Next(-2, 5);//rn.Next(-4, 4);
         //   this.Z = -2.77f;//-1;
        }
       
    
        public float X { get; set; }
        public float Y { get; set; }
    //    public float Z { get; set; }
    }
}
