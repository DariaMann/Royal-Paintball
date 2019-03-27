using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Pistol:Weapons
    {
        public Pistol()
        {
          
            this.CountBullets = 12;
            this.Power = 1;
            this.Direction = 1;//направление мыши
            this.FlightTime = 1.4;
            this.RechargeTime = 0.2;
            this.InityalCountBul = 13;
            this.Index = 1;
            this.TakenLives = 2;
            this.CountMagazine = 0;
        }
        public override void Shoot()
        {
            // throw new NotImplementedException();
            this.CountBullets -= 1;
        }
    }
}
