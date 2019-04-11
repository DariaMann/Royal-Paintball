using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Shotgun:Weapons
    {
        public Shotgun()
        {
            
            this.CountBullets = 7;
            this.Direction = 1;//направление мыши
            this.Power = 3;
            this.FlightTime = 1;
            this.RechargeTime = 0.6;
            this.InityalCountBul = 20;
            this.Index = 2;
            this.TakenLives = 4;
            this.CountMagazine = 0;
            this.MaxCountMag = 7;
        }
        public override void Shoot(Field f, string playerID, Dictionary<string, Dictionary<string, string>> dasha)
        {
            f.SelectedWeapons = f.S;
            f.Players[playerID].Weap = f.SelectedWeapons;
            int bul = f.Players[playerID].Weap.CountBullets--;
            dasha[playerID]["bulS"] = Convert.ToString(--bul);
            //this.CountBullets -= 1;
        }
        public override void LiftItem(Field f, string playerID, Dictionary<string, Dictionary<string, string>> dasha)
        {
            dasha[playerID]["magazineS"] = Convert.ToString(f.Players[playerID].Weap.CountMagazine);
        }
        public override void Reload(Field f, string playerID, Dictionary<string, Dictionary<string, string>> dasha, string bul, string mag)
        {
            dasha[playerID]["bulS"] = bul;
            dasha[playerID]["magazineS"] = mag;
        }
    }
}
