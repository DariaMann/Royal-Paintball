using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary
{
    public abstract class Weapons
    {
        public int CountBullets { get; set; }//количество пуль за один выстрел
        public abstract void Shoot();// метод стрельбы
        public int TakenLives { get; set; }//количество отнятых жизней за попадание
        public int CountMagazine { get; set; }
        public int MaxCountMag { get; set; }
        public bool CamShot { get; set; }
        public DateTime time { get; set; }

        public void ChangeWeap(Field f, string weap, int ID)
        {
            if (weap == "Pistol")
            {
                f.Player[ID].Weap = f.Player[ID].P;
            }
            if (weap == "Shotgun")
            {
                f.Player[ID].Weap = f.Player[ID].S;
            }
            if (weap == "Gun")
            {
                f.Player[ID].Weap = f.Player[ID].G;
            }
            if (weap == "Bumb")
            {
                f.Player[ID].Weap = f.Player[ID].B;
            }
        }

    }

}

