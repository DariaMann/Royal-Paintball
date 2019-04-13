using System;
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
            this.Direction = 1;//направление мыши
            this.Power = 2;
            this.FlightTime = 0.8;
            this.RechargeTime = 0.5;
            this.InityalCountBul = 10;
            this.Index = 3;
            this.TakenLives = 3;
            this.CountMagazine = 0;
            this.MaxCountMag = 30;
        }
        public override void Shoot(Field f, string playerID, Dictionary<string, Dictionary<string, string>> dasha)
        {
            f.SelectedWeapons = f.G;
            f.Players[playerID].Weap = f.SelectedWeapons;
            int bul = f.Players[playerID].Weap.CountBullets--;
            dasha[playerID]["bulG"] = Convert.ToString(--bul);
            //this.CountBullets -= 1;
        }
        public override void LiftItem(Field f, string playerID, Dictionary<string, Dictionary<string, string>> dasha)
        {
            dasha[playerID]["magazineG"] = Convert.ToString(f.Players[playerID].Weap.CountMagazine);
        }
        public override void Reload(Field f, string playerID, Dictionary<string, Dictionary<string, string>> dasha, string bul, string mag)
        {
            dasha[playerID]["bulG"] = bul;
            dasha[playerID]["magazineG"] = mag;
        }
    }
}
