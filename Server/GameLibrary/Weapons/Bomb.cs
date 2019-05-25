using System;

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
            range = 2;

        }
        public override void Shoot()
        {
            this.CountBullets--;
        }
    }
}
