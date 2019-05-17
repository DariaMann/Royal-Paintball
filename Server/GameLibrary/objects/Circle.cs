using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary
{
    public class Circle
    {
        public float X { get; set; }
        public float Y { get; set; }
        public double[] Size { get; set; }
        public float Radius { get; set; }
        public int StartTime { get; set; }
        public float a { get; set; }
        public float b { get; set; }
        public float endX { get; set; }
        public float endY { get; set; }
        public bool go { get; set; }

        public Circle()
        {
            X = 0;
            Y = 0;
            Radius = 84;
            Size = new double[] { 20, 20 };
            StartTime = 10;
            go = false;
        }

        public void Move(int endX, int endY, float speed)
        {
            float cos;
            float sin;
            this.endX = endX;
            this.endY = endY;
            if (endX >= X)
            {
                float k = (endY - Y) / (endX - X);

                cos = Convert.ToSingle(Math.Sqrt(1 / (1 + k * k)));
                sin = k * cos;

                this.a = speed * cos;
                this.b = speed * sin;
            }
            else
            {
                float k = (endY - Y) / (endX - X);

                cos = Convert.ToSingle(-Math.Sqrt(1 / (1 + k * k)));
                sin = k * cos;

                this.a = speed * cos;
                this.b = speed * sin;
            }
        }
    }
}
