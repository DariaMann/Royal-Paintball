﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Gun:Weapons
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
        }
        public override void Shoot()
        {
            this.CountBullets--;
        }
       
    }
}
