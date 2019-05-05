using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Pistol:Weapons
    {
        public Pistol()
        {
            this.CountBullets = 5;
            this.Power = 1;
            this.Direction = 1;//направление мыши
            this.FlightTime = 1.4;
            this.RechargeTime = 0.2;
            this.InityalCountBul = 13;
            this.Index = 1;
            this.TakenLives = 2;
            this.CountMagazine = 24;
            this.MaxCountMag = 12;
        }
        public override void Shoot()
        {
            this.CountBullets--;
        }
    }
}
