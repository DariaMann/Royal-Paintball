using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Pistol:Weapons
    {
        public Pistol()
        {
            this.CountBullets = 5;
            this.Power = 1;
            this.Direction = 1;//направление мыши
            this.FlightTime = 1.4;
            this.RechargeTime = 0.2;
            this.InityalCountBul = 13;
            this.Index = 1;
            this.TakenLives = 2;
            this.CountMagazine = 24;
            this.MaxCountMag = 12;
        }
        public override void Shoot(Field f, string playerID, Dictionary<string, Dictionary<string, string>> dasha)
        {
            int bul = f.Players[playerID].Weap.CountBullets--;
            dasha[playerID]["bulP"] = Convert.ToString(--bul);
        }
        public override void LiftItem(Field f, string playerID, Dictionary<string, Dictionary<string, string>> dasha)
        {
            dasha[playerID]["magazineP"] = Convert.ToString(f.Players[playerID].Weap.CountMagazine);
        }
        public override void Reload(Field f, string playerID, Dictionary<string, Dictionary<string, string>> dasha,string bul,string mag)
        {
            dasha[playerID]["bulP"] = bul;
            dasha[playerID]["magazineP"] = mag;
        }
    }
}
