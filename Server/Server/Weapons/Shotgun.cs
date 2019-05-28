using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Shotgun : Weapons,ICloneable
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
            TimeFly = 4;
        }
        public override void Shoot()
        {
            this.CountBullets--;
        }
        public object Clone()
        {
            return new Shotgun
            {
                CountBullets = this.CountBullets,
                TakenLives = this.TakenLives,
                CountMagazine = this.CountMagazine,
                MaxCountMag = this.MaxCountMag,
                CamShot = this.CamShot,
                time = this.time
            };
        }
    }
}

