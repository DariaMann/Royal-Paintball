using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Bomb:Weapons
    {
        public Bomb()
        {
            this.CountBullets = 4;
            this.Direction = 1;//направление мыши
            this.Power = 4;
            this.FlightTime = 2;
            this.RechargeTime = 0.1;
            this.InityalCountBul = 5;
            this.Index = 4;
            this.TakenLives = 5;
        }
        public override void Shoot()
        {
            this.CountBullets -= 1;
        }
    }
}
