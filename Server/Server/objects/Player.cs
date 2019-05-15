using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Player
    {
        public int Life { get; set; }
        public int ID { get; set; }
        public string Direction { get; set; }

        public float X { get; set; }
        public float Y { get; set; }
        public float XRot { get; set; }
        public float YRot { get; set; }
        public bool Shoot { get; set; }
        public bool Reload { get; set; }
        public bool LiftItem { get; set; }
        public float[] MousePos { get; set; }
        public int[] Size { get; set; }

        public float[] Start { get; set; }
        public float[] End { get; set; }

        public string Weapon { get; set; }
        public Weapons Weap { get; set; }
        public Pistol P { get; set; }
        public Shotgun S { get; set; }
        public Gun G { get; set; }
        public Bomb B { get; set; }

        public string Color { get; set; }

        public bool OutCircle { get; set; }

        public Dictionary<string, string> StopIn { get; set; }

        public Player()
        {
            P = new Pistol();
            S = new Shotgun();
            G = new Gun();
            B = new Bomb();
            Weap = P;
            Random rn = new Random(); // объявление переменной для генерации чисел
            this.X = rn.Next(-2, 5);
            this.Y = rn.Next(-2, 5);
            XRot = -90;
            YRot = 0;
            Life = 50;
            Direction = "N";
            Shoot = false;
            Weapon = "Pistol";
            LiftItem = false;
            Start = new float[2];
            End = new float[2];
            MousePos = new float[3];
            Size = new int[] { 1, 1 };
            OutCircle = false;
            StopIn = new Dictionary<string, string>();

        }

    }
}
