using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Bomb:Weapons
    {
        public Bomb()
        {
            this.CountBullets = 4;
            this.Direction = 1;//направление мыши
            this.Power = 4;
            this.FlightTime = 2;
            this.RechargeTime = 0.1;
            this.InityalCountBul = 5;
            this.Index = 4;
            this.TakenLives = 5;
            this.CountMagazine = 3;
            this.MaxCountMag = 4;
            
        }
        public override void Shoot(Field f,string playerID, Dictionary<string, Dictionary<string, string>> dasha)
        {
            f.SelectedWeapons = f.B;
            f.Players[playerID].Weap = f.SelectedWeapons;
            int bul = f.Players[playerID].Weap.CountBullets--;
            dasha[playerID]["bulB"] = Convert.ToString(--bul);

           // this.CountBullets -= 1;
        }
        public override void LiftItem(Field f, string playerID, Dictionary<string, Dictionary<string, string>> dasha)
        {
            dasha[playerID]["magazineB"] = Convert.ToString(f.Players[playerID].Weap.CountMagazine);
        }
        public override void Reload(Field f, string playerID, Dictionary<string, Dictionary<string, string>> dasha, string bul, string mag)
        {
            dasha[playerID]["bulB"] = bul;
            dasha[playerID]["magazineB"] = mag;
        }
    }
}
