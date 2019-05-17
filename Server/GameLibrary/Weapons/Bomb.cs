using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary
{
    public class Bomb : Weapons
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

        }
        public override void Shoot()
        {
            this.CountBullets--;
        }
    }
}
