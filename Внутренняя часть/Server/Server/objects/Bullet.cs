using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Bullet
    {
        public Color Color { get; set; }
        public int ID { get; set; }
        public string Weapon { get; set; }
        public float[] EndPos { get; set; }
        public float[] StartPos { get; set; }

        public Bullet(float startX, float startY, float startZ, float endX, float endY, float endZ, string weapon,int id)
        {
            this.Weapon = weapon;
            this.StartPos = new float[3];
            StartPos[0] = startX;
            StartPos[1] = startY;
            StartPos[2] = startZ;
            this.EndPos = new float[3];
            EndPos[0] = endX;
            EndPos[1] = endY;
            EndPos[2] = endZ;
            this.ID = id;
        }
    }
}
