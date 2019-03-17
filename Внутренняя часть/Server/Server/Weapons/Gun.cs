using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Gun:Weapons
    {
        public Gun(Color color)
        {
            this.Color = color;
            this.ColorBul = color;
            this.CountBullets = 30;
            this.Direction = 1;//направление мыши
            this.Power = 2;
            this.FlightTime = 0.8;
            this.RechargeTime = 0.5;
            this.InityalCountBul = 10;
            this.Index = 3;
            this.TakenLives = 3;
        }
        public override void Shoot()
        {
            this.CountBullets -= 1;
        }
    }
}
