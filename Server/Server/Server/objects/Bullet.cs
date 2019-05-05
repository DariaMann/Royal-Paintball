using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Bullet
    {
        public int ID { get; set; }
        public string Weapon { get; set; }
 
        public float X { get; set; }
        public float Y { get; set; }
        public float a { get; set; }
        public float b { get; set; }

        public Bullet(float endX, float endY,float x, float y, string weapon,int id,float speed)
        {
            float cos;
            float sin;
            if (endX >= x)
            {
                float k = (endY - y) / (endX - x);

                cos = Convert.ToSingle(Math.Sqrt(1 / (1 + k * k)));
                sin = k * cos;

                this.a = speed * cos;
                this.b = speed * sin;
            }
            else
            {
                float k = (endY - y) / (endX - x);

                cos = Convert.ToSingle(-Math.Sqrt(1 / (1 + k * k)));
                sin = k * cos;

                this.a = speed * cos;
                this.b = speed * sin;
            }
            this.X = x;
            this.Y = y;
            this.Weapon = weapon;
            this.ID = id;
        }
    }
}
