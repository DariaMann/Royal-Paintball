using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Server
{
    public class GameController: IPlayController//Consumer
    {
        public Field field;//поле игры
        TimerCallback tm = new TimerCallback(Count);
        public Timer timer { get; set; }
        static int i;
        public static void Count(object obj)
        {
            i++;
           // Console.WriteLine(i);
           // BulFlight();
        }

        public GameController(Field field)
        {
            this.field = field;

            i = 0;
            this.timer = new Timer(tm, i, 0, 1000);
        }
       
        public void LiftItem(int ID)//поднятие вещей
        {
           //float x = field.Players[playerID].Pos[0];
           // float y = field.Players[playerID].Pos[1];
           //// float radius = 5;
           // if (field.Items.Count != 0)
           // {
           //     for (int i = 0; i < field.Items.Count; i++) {
           //         field.Players[playerID].Weap.CountMagazine += field.Items[i].Count;
           //         field.Items[i].LiftItem(field, playerID, dasha, field.Items[i].Name);
           //         field.Items.Remove(field.Items[i]);
           //     }
           // } 
        }

        public void Woundd(int ID)//ранение
        {
            if (field.Player[ID].Life > 0)
            {
                field.Player[ID].Life -= 1;
            }
        }

        public void Hit(int ID)//ранение
        {

            for (int i = 0; i < field.Bullet.Count; i++)
            {
                foreach (Wall c in field.Wall)
                {
                    float aX = c.X - c.Size[0];
                    float aY = c.Y + c.Size[1];
                    float bX = c.X + c.Size[0];
                    float bY = c.Y + c.Size[1];
                    float cX = c.X + c.Size[0];
                    float cY = c.Y - c.Size[1];
                    float dX = c.X - c.Size[0];
                    float dY = c.Y - c.Size[1];
                    if (field.Bullet[i].X > aX && field.Bullet[i].X < bX)
                    {
                        if (field.Bullet[i].Y > dY && field.Bullet[i].Y < aY)
                        {
                            //Woundd(ID);
                            DelBul(field.Bullet[i]);
                            // field.Bullet[i].ID = 0;
                            break;
                        }
                    }
                }
            }
            for (int i = 0; i < field.Bullet.Count; i++)
            {
                foreach (Tree c in field.Tree)
            {
                float aX = c.X - c.Size[0];
                float aY = c.Y + c.Size[1];
                float bX = c.X + c.Size[0];
                float bY = c.Y + c.Size[1];
                float cX = c.X + c.Size[0];
                float cY = c.Y - c.Size[1];
                float dX = c.X - c.Size[0];
                float dY = c.Y - c.Size[1];
                if (field.Bullet[i].X > aX && field.Bullet[i].X < bX)
                {
                    if (field.Bullet[i].Y > dY && field.Bullet[i].Y < aY)
                    {
                        // Woundd(ID);
                        DelBul(field.Bullet[i]);
                        // field.Bullet[i].ID = 0;
                        break;
                    }
                }
            }
            }
            for (int i = 0; i < field.Bullet.Count; i++)
            {
                foreach (int c in field.Player.Keys)
                {
                    float aX = field.Player[c].X - field.Player[c].Size[0];
                    float aY = field.Player[c].Y + field.Player[c].Size[1];
                    float bX = field.Player[c].X + field.Player[c].Size[0];
                    float bY = field.Player[c].Y + field.Player[c].Size[1];
                    float cX = field.Player[c].X + field.Player[c].Size[0];
                    float cY = field.Player[c].Y - field.Player[c].Size[1];
                    float dX = field.Player[c].X - field.Player[c].Size[0];
                    float dY = field.Player[c].Y - field.Player[c].Size[1];
                    if (field.Bullet[i].X > aX && field.Bullet[i].X < bX)
                    {
                        if (field.Bullet[i].Y > dY && field.Bullet[i].Y < aY)
                        {
                            Woundd(c);
                            DelBul(field.Bullet[i]);
                            // field.Bullet[i].ID = 0;
                            break;
                        }
                    }
                }
            }

        }

        public void ChangeWeapon(int ID,string weapon)//смена оружия
        {
            if (weapon == "Pistol")
            {
                field.Player[ID].Weap = field.Player[ID].P;
            }
            if (weapon == "Shotgun")
            {
                field.Player[ID].Weap = field.Player[ID].S;
            }
            if (weapon == "Gun")
            {
                field.Player[ID].Weap = field.Player[ID].G;
            }
            if (weapon == "Bomb")
            {
                field.Player[ID].Weap = field.Player[ID].B;
            }
            field.Player[ID].Weapon =weapon;
           
        }

        public void Reload(int ID,Player player)//перезарядка
        {
            if (field.Player[ID].Weap.CountMagazine != 0)
            {
                if (field.Player[ID].Weap.CountBullets == 0)
                {
                    if (field.Player[ID].Weap.CountMagazine == field.Player[ID].Weap.MaxCountMag)
                    {
                        field.Player[ID].Weap.CountBullets = field.Player[ID].Weap.CountMagazine;
                        field.Player[ID].Weap.CountMagazine = field.Player[ID].Weap.CountMagazine - field.Player[ID].Weap.MaxCountMag;
                    }
                    else
                    {
                        if (field.Player[ID].Weap.CountMagazine > field.Player[ID].Weap.MaxCountMag)
                        {
                            field.Player[ID].Weap.CountBullets = field.Player[ID].Weap.MaxCountMag;
                            field.Player[ID].Weap.CountMagazine = field.Player[ID].Weap.CountMagazine - field.Player[ID].Weap.MaxCountMag;
                        }
                        else
                        {
                            if (field.Player[ID].Weap.CountMagazine < field.Player[ID].Weap.MaxCountMag)
                            {
                                field.Player[ID].Weap.CountBullets = field.Player[ID].Weap.CountMagazine;
                                field.Player[ID].Weap.CountMagazine = 0;
                            }
                        }
                    }
                }
            }
        }

        public void FinishGame(int ID)//конец игры
        {
            int pistol = field.Player[ID].P.CountBullets + field.Player[ID].P.CountMagazine;
            if (pistol != 0)
            { field.Item.Add(new Item("Pistol", pistol)); }
            int shotgun = field.Player[ID].S.CountBullets + field.Player[ID].S.CountMagazine;
            if (shotgun != 0)
            { field.Item.Add(new Item("Shotgun", shotgun)); }
            int gun = field.Player[ID].G.CountBullets + field.Player[ID].G.CountMagazine;
            if (gun != 0)
            { field.Item.Add(new Item("Gun", gun)); }
            int bomb = field.Player[ID].B.CountBullets + field.Player[ID].B.CountMagazine;
            if (bomb != 0)
            { field.Item.Add(new Item("Bomb", bomb)); }

            field.Player.Remove(ID);
        }

        public void Shoott(int ID,Player player)//стрельба
        {
            float speed = 0.1f;
            field.Bullet.Add(new Bullet(player.End[0], player.End[1], player.Start[0], player.Start[1], player.Weapon, ID,speed));
            field.Player[ID].Weap.Shoot();
            SmallCircle();
        }
         
        public void BulFlight()//направление полета пули
        {

            for (int i = 0; i < field.Bullet.Count; i++)
            {
                field.Bullet[i].X += field.Bullet[i].a;
                field.Bullet[i].Y += field.Bullet[i].b;
            }
        }

        void DelBul(Bullet bul)
        {
            if(field.Bullet.Contains(bul))
            field.Bullet.Remove(bul);
        }
       public void SmallCircle()
        {
            field.circle.Size[0] -= 3;
            field.circle.Size[1] -= 3;
        }

       public void MovePlayer(int ID,string dir)//движение игрока
        {
            switch (dir)
            {
                case "W":
                    {
                        field.Player[ID].Y += 0.2f;
                        break;
                    }
                case "S":
                    {
                        field.Player[ID].Y -= 0.2f;
                        break;
                    }
                case "A":
                    {
                        field.Player[ID].X -= 0.2f;
                        break;
                    }
                case "D":
                    {
                        field.Player[ID].X += 0.2f;
                        break;
                    }
            }
        }
        
    }
}
