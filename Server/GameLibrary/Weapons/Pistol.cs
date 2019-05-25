using System;

namespace GameLibrary
{
    public class Pistol : Weapons
    {
        public Pistol()
        {
            this.CountBullets = 6;
            this.TakenLives = 2;
            this.CountMagazine = 24;
            this.MaxCountMag = 12;
            this.CamShot = true;
            this.time = new DateTime();
            time = DateTime.Now;
            range = 4;
        }
        public override void Shoot()
        {
            this.CountBullets--;
        }
    }
}
