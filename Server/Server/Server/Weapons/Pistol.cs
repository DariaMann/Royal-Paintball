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
            this.TakenLives = 2;
            this.CountMagazine = 24;
            this.MaxCountMag = 12;
            this.CamShot = true;
            this.time = new DateTime();
            time = DateTime.Now;
        }
        public override void Shoot()
        {
            this.CountBullets--;
        }
    }
}
