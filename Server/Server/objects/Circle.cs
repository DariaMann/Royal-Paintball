using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Circle : ICloneable
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

        public void SmallCircleMove()
        {
            if (go == true)
            {
                if (Math.Abs(X) < Math.Abs(endX))
                {
                    X += a;
                    Y += b;
                    if (Size[0] > 3)
                    {
                        double i = Size[0];
                        Size[0] -= 0.01;
                        Size[1] -= 0.01;
                        Radius = Convert.ToSingle((Radius * Size[0]) / i);
                    }
                }
                else
                {
                    go = false;
                }

            }
        }

        public void SmallCircleCall()
        {
            go = true;
            Random rn = new Random(); // объявление переменной для генерации чисел
            int x = rn.Next(-40, 40); //rn.Next(-8, 8);
            int y = rn.Next(-40, 40);//rn.Next(-4, 4);
            float speed = 0.3f;
            Move(x, y, speed);
        }

        public object Clone()
        {
            return new Circle
            {
                X = this.X,
                Y = this.Y,
                Size = (double[])this.Size.Clone(),
                Radius = this.Radius,
                StartTime = this.StartTime,
                a = this.a,
                b=this.b,
                endX=this.endX,
                endY = this.endY,
                go = this.go
            };
        }
    }
}
