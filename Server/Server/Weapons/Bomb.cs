using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Bomb : Weapons,ICloneable
    {
        public Bomb()
        {
            this.CountBullets = 4;
            this.TakenLives = 5;
            this.CountMagazine = 3;
            this.MaxCountMag = 4;
            this.CamShot = true;
            this.time = new DateTime();
            time = DateTime.Now;
            TimeFly = 2;

        }
        public override void Shoot()
        {
            this.CountBullets--;
        }
        public object Clone()
        {
            return new Bomb
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
