﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{

    public abstract class Weapons
    {
        public double Direction { get; set; }//направление полета пули(местонахождение мыши)
        public int CountBullets { get; set; }//количество пуль за один выстрел
        public int Power { get; set; }//сила удара пули
        public double FlightTime { get; set; }//время полета пули
        public double NextShootTime { get; set; }//время полета пули
        public double RechargeTime { get; set; }//время для перезарядки оружия
        public int InityalCountBul { get; set; }//начальное количество пуль
        public abstract void Shoot();// метод стрельбы
        public int Index { get; set; }//индекс оружия
        public int TakenLives { get; set; }//количество отнятых жизней за попадание
        public int CountMagazine { get; set; }
        public int MaxCountMag { get; set; }

        public void ChangeWeap(Field f, string weap,int ID)
        {
            if(weap == "Pistol")
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
