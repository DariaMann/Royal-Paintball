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
        public bool Me { get; set; }
        public bool OutCircle { get; set; }

        public Dictionary<string, string> StopIn { get; set; }

        public Player()
        {
            Me = false;
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

        public void MovePlayer(string dir)//движение игрока
        {
            float speed = 0.4f;
            switch (dir)
            {
                case "W":
                    {
                        if (!StopIn.ContainsKey("W"))
                            Y += speed;
                        break;
                    }
                case "S":
                    {
                        if (!StopIn.ContainsKey("S"))
                            Y -= speed;
                        break;
                    }
                case "A":
                    {
                        if (!StopIn.ContainsKey("A"))
                            X -= speed;
                        break;
                    }
                case "D":
                    {
                        if (!StopIn.ContainsKey("D"))
                            X += speed;
                        break;
                    }
            }
        }

        public void Wound(int takenLifes)//ранение
        {
            if (Life > 0)
            {
                Life -= takenLifes;
            }
        }

        public void ReloadCall()
        {
            Weap.CamShot = false;
            int second = DateTime.Now.Second;
            if (DateTime.Now.Second == 59)
            {
                second = 1;

            }
            else
            {
                if (DateTime.Now.Second == 58)
                {
                    second = 0;
                }
                else
                {
                    second += 2;
                }
            }
            DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, second, DateTime.Now.Millisecond);
            Weap.time = dt;
        }

        public void ReloadWeapon()//перезарядка
        {
            if (Weap.CamShot == false)
            {
                if (DateTime.Now.Second >= Weap.time.Second)
                {
                    if (Weap.CountMagazine != 0)
                    {
                        if (Weap.CountBullets == 0)
                        {
                            if (Weap.CountMagazine == Weap.MaxCountMag)
                            {
                                Weap.CountBullets = Weap.CountMagazine;
                                Weap.CountMagazine = Weap.CountMagazine - Weap.MaxCountMag;
                            }
                            else
                            {
                                if (Weap.CountMagazine > Weap.MaxCountMag)
                                {
                                    Weap.CountBullets = Weap.MaxCountMag;
                                    Weap.CountMagazine = Weap.CountMagazine - Weap.MaxCountMag;
                                }
                                else
                                {
                                    if (Weap.CountMagazine < Weap.MaxCountMag)
                                    {
                                        Weap.CountBullets = Weap.CountMagazine;
                                        Weap.CountMagazine = 0;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (!(Weap.CountBullets == Weap.MaxCountMag))
                            {
                                if (Weap.CountMagazine < (Weap.MaxCountMag - Weap.CountBullets))
                                {
                                    Weap.CountBullets += Weap.CountMagazine;
                                    Weap.CountMagazine = 0;
                                }
                                else
                                {
                                    int i = Weap.MaxCountMag - Weap.CountBullets;
                                    Weap.CountBullets = Weap.MaxCountMag;
                                    Weap.CountMagazine -= i;
                                }
                            }
                        }
                    }
                    Weap.CamShot = true;
                }

            }
        }

        public bool LiftItemInGame(Item item)//поднятие вещей
        {
            float aX = X - (Size[0] + 2);
            float aY = Y + (Size[1] + 2);
            float bX = X + (Size[0] + 2);
            float bY = Y + (Size[1] + 2);
            float cX = X + (Size[0] + 2);
            float cY = Y - (Size[1] + 2);
            float dX = X - (Size[0] + 2);
            float dY = Y - (Size[1] + 2);
            if (item.X > aX && item.X < bX)
            {
                if (item.Y > dY && item.Y < aY)
                {
                    switch (item.Name)
                    {
                        case "Pistol":
                            {
                                P.CountMagazine += item.Count;

                                break;
                            }
                        case "Shotgun":
                            {
                                S.CountMagazine += item.Count;
                                break;
                            }
                        case "Gun":
                            {

                                G.CountMagazine += item.Count;
                                break;
                            }
                        case "Bomb":
                            {
                                B.CountMagazine += item.Count;
                                break;
                            }
                        case "Kit":
                            {
                                Life += item.Count;
                                break;
                            }
                    }
                    return true;
                }
            }
            return false;
        }
        
        public void ChangeWeapon(string weapon)//смена оружия
        {
            if (weapon == "Pistol")
            {
                Weap = P;
            }
            if (weapon == "Shotgun")
            {
                Weap = S;
            }
            if (weapon == "Gun")
            {
                Weap = G;
            }
            if (weapon == "Bomb")
            {
               Weap = B;
            }
            Weapon = weapon;

        }
    }
}
