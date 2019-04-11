using System;
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
        public abstract void Shoot(Field f, string playerID, Dictionary<string, Dictionary<string, string>> dasha);// метод стрельбы
        public abstract void LiftItem(Field f, string playerID, Dictionary<string, Dictionary<string, string>> dasha);
        public abstract void Reload(Field f, string playerID, Dictionary<string, Dictionary<string, string>> dasha, string bul, string mag);
        public int Index { get; set; }//индекс оружия
        public int TakenLives { get; set; }//количество отнятых жизней за попадание
        public int CountMagazine { get; set; }
        public int MaxCountMag { get; set; }

        public void ChangeWeap(Field f, string playerID, Dictionary<string, Dictionary<string, string>> dasha)
        {
            if(dasha[playerID]["weapon"] == "Pistol")
            {
                f.SelectedWeapons = f.P;
            }
            if (dasha[playerID]["weapon"] == "Shotgun")
            {
                f.SelectedWeapons = f.S;
            }
            if (dasha[playerID]["weapon"] == "Gun")
            {
                f.SelectedWeapons = f.G;
            }
            if (dasha[playerID]["weapon"] == "Bumb")
            {
                f.SelectedWeapons = f.B;
            }
        }

    }
  
}
