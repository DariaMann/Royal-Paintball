using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Shotgun:Weapons
    {
        public Shotgun(Color color)
        {
            this.Color = color;
            this.Color = color;
            this.CoutnBullets = 4;
            this.Direction = 1;//направление мыши
            this.Power = 3;
            this.FlightTime = 1;
            this.RechargeTime = 0.6;
            this.InityalCountBul = 20;
        }
        public override void Shoot()
        {

        }
    }
}
