using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary
{
    public class Shotgun : Weapons
    {
        public Shotgun()
        {

            this.CountBullets = 7;
            this.TakenLives = 4;
            this.CountMagazine = 0;
            this.MaxCountMag = 7;
            this.CamShot = true;
            this.time = new DateTime();
            time = DateTime.Now;
            range = 3;
        }
        public override void Shoot()
        {
            this.CountBullets--;
        }
    }
}

