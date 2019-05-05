using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public  class Item
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public int Index { get; set; }

        public Item(string name, int count,float x,float y,int i)
        {
            this.Name = name;
            this.Count = count;
            this.X = x;
            this.Y = y;
            this.Index = i;

        }
        //public void LiftItem(Field field, string playerID, Dictionary<string, Dictionary<string, string>> dasha,string ItemName)
        //{
        //    if(ItemName == "Pistol")
        //    {
        //        dasha[playerID]["magazineP"] = Convert.ToString(field.Players[playerID].Weap.CountMagazine);
        //    }
        //    if (ItemName == "Shotgun")
        //    {
        //        dasha[playerID]["magazineS"] = Convert.ToString(field.Players[playerID].Weap.CountMagazine);
        //    }
        //    if (ItemName == "Gun")
        //    {
        //        dasha[playerID]["magazineG"] = Convert.ToString(field.Players[playerID].Weap.CountMagazine);
        //    }
        //    if (ItemName == "Bomb")
        //    {
        //        dasha[playerID]["magazineB"] = Convert.ToString(field.Players[playerID].Weap.CountMagazine);
        //    }
        //}
    }
}
