using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Gun : Weapons,ICloneable
    {
        public Gun()
        {
            this.CountBullets = 30;
            this.TakenLives = 3;
            this.CountMagazine = 0;
            this.MaxCountMag = 30;
            this.CamShot = true;
            this.time = new DateTime();
            time = DateTime.Now;
            TimeFly = 6;
        }
        public override void Shoot()
        {
            this.CountBullets--;
        }
        public object Clone()
        {
            return new Gun
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
