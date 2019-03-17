using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Data
    {
          
            public Action[] Actions { get; set; }
        public ASD Asd { get; set; }
    }
    class ASD
    {
        public string X { get; set; }
        public string Y { get; set; }
        public string Z { get; set; }
        public string Xw { get; set; }
        public string Yw { get; set; }
        public string Zw { get; set; }
        public string Xr { get; set; }
        public string Yr { get; set; }
        public string Zr { get; set; }
        public string IP { get; set; }
        public string Life { get; set; }

    }
    class Action
    {
        public string NewPlayer { get; set; }
        public string MovePlayer { get; set; }
        public string FinishGame { get; set; }
        public string WeaponRotation { get; set; }
        public string Shoot { get; set; }
        public string Wound { get; set; }
        public string SelectWeapon { get; set; }
        //public string Wound { get; set; }
        //public string Wound { get; set; }
    }
}

