using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Pistol:Weapons
    {
        public Pistol(Color color)
        {
            this.Color = color;
            this.Color = color;
            this.CoutnBullets = 1;
            this.Power = 1;
            this.Direction = 1;//направление мыши
            this.FlightTime = 1.4;
            this.RechargeTime = 0.2;
            this.InityalCountBul = 13;
        }
    }
}
