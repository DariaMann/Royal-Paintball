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

        public GameController(Field field)
        {
            this.field = field;
        }
       
        public void LiftItem(int ID)//поднятие вещей
        {
            foreach (int c in field.Player.Keys)
            {
                for (int i = 0; i < field.Item.Count; i++)
                {
                    float aX = field.Player[c].X - (field.Player[c].Size[0] + 5);
                    float aY = field.Player[c].Y  + (field.Player[c].Size[1] + 5);
                    float bX = field.Player[c].X  + (field.Player[c].Size[0] + 5);
                    float bY = field.Player[c].Y  + (field.Player[c].Size[1] + 5);
                    float cX = field.Player[c].X  + (field.Player[c].Size[0] + 5);
                    float cY = field.Player[c].Y  - (field.Player[c].Size[1] + 5);
                    float dX = field.Player[c].X - (field.Player[c].Size[0] + 5);
                    float dY = field.Player[c].Y  - (field.Player[c].Size[1] + 5);
                    if (field.Item[i].X > aX && field.Item[i].X < bX)
                    {
                        if (field.Item[i].Y > dY && field.Item[i].Y < aY)
                        {
                            switch(field.Item[i].Name)
                            {
                                case "Pistol":
                                    {
                                        field.Player[c].P.CountMagazine += field.Item[i].Count;
                                      
                                        break;
                                    }
                                case "Shotgun":
                                    {
                                        field.Player[c].S.CountMagazine += field.Item[i].Count;
                                        break;
                                    }
                                case "Gun":
                                    {

                                        field.Player[c].G.CountMagazine += field.Item[i].Count;
                                        break;
                                    }
                                case "Bomb":
                                    {
                                        field.Player[c].B.CountMagazine += field.Item[i].Count;
                                        break;
                                    }
                            }
                            field.Item.Remove(field.Item[i]);
                        }
                    }
                }

            }
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

        public int WeapWound(string weapon, int ID)
        {
            int countLife = 0;
            switch (weapon)
            {
                case "Pistol":
                    {
                        countLife = field.Player[ID].P.TakenLives;
                        break;
                    }
                case "Shotgun":
                    {
                        countLife = field.Player[ID].S.TakenLives;
                        break;
                    }
                case "Gun":
                    {
                        countLife = field.Player[ID].G.TakenLives;
                        break;
                    }
                case "Bomb":
                    {
                        countLife = field.Player[ID].B.TakenLives;
                        break;
                    }
            }
            return countLife;
        }

        public void Woundd(int ID,int takenLifes)//ранение
        {
            

            if (field.Player[ID].Life > 0)
            {
                field.Player[ID].Life += takenLifes;
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
                            if (field.Player[c].ID != field.Bullet[i].ID)
                            {
                                int countTakenLife = WeapWound(field.Bullet[i].Weapon, ID);
                                Woundd(c, countTakenLife);
                                DelBul(field.Bullet[i]);
                                // field.Bullet[i].ID = 0;
                                break;
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < field.Bullet.Count; i++)
            {
                    float aX = field.X - field.Size[0];
                    float aY = field.Y + field.Size[1];
                    float bX = field.X + field.Size[0];
                    float bY = field.Y + field.Size[1];
                    float cX = field.X + field.Size[0];
                    float cY = field.Y - field.Size[1];
                    float dX = field.X - field.Size[0];
                    float dY = field.Y - field.Size[1];
                    if (field.Bullet[i].X < aX || field.Bullet[i].X > bX|| field.Bullet[i].Y < dY || field.Bullet[i].Y > aY)
                    {
                                DelBul(field.Bullet[i]);
                            
                    }
            }
            foreach (int c in field.Player.Keys)
            {
                float aX = field.circle.X - field.circle.Size[0];
                float aY = field.circle.Y + field.circle.Size[1];
                float bX = field.circle.X + field.circle.Size[0];
                float bY = field.circle.Y + field.circle.Size[1];
                float cX = field.circle.X + field.circle.Size[0];
                float cY = field.circle.Y - field.circle.Size[1];
                float dX = field.circle.X - field.circle.Size[0];
                float dY = field.circle.Y - field.circle.Size[1];
                if (field.Player[c].X < aX || field.Player[c].X > bX || field.Player[c].Y < dY || field.Player[c].Y > aY)
                {
                    Woundd(c,1);
                }
            }
            //foreach (int c in field.Player.Keys)
            //{
            //    float aX = field.Player[c].X - field.Player[c].Size[0];
            //    float aY = field.Player[c].Y + field.Player[c].Size[1];
            //    float bX = field.Player[c].X + field.Player[c].Size[0];
            //    float bY = field.Player[c].Y + field.Player[c].Size[1];
            //    float cX = field.Player[c].X + field.Player[c].Size[0];
            //    float cY = field.Player[c].Y - field.Player[c].Size[1];
            //    float dX = field.Player[c].X - field.Player[c].Size[0];
            //    float dY = field.Player[c].Y - field.Player[c].Size[1];
            //    if (field.Wall[i].X > aX && field.Wall[i].X < bX)
            //    {
            //        if (field.Wall[i].Y > dY && field.Wall[i].Y < aY)
            //        {
            //            string badSide = "";
            //            if(field.Player[c].X<field.Wall[i].X)
            //            {

            //            }
            //        }
            //    }
            //}
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
            { field.Item.Add(new Item("Pistol", pistol, field.Player[ID].X, field.Player[ID].Y + 1.5f,field.Item.Count)); }
            int shotgun = field.Player[ID].S.CountBullets + field.Player[ID].S.CountMagazine;
            if (shotgun != 0)
            { field.Item.Add(new Item("Shotgun", shotgun, field.Player[ID].X, field.Player[ID].Y - 1.5f, field.Item.Count)); }
            int gun = field.Player[ID].G.CountBullets + field.Player[ID].G.CountMagazine;
            if (gun != 0)
            { field.Item.Add(new Item("Gun", gun, field.Player[ID].X - 1.5f, field.Player[ID].Y, field.Item.Count)); }
            int bomb = field.Player[ID].B.CountBullets + field.Player[ID].B.CountMagazine;
            if (bomb != 0)
            { field.Item.Add(new Item("Bomb", bomb, field.Player[ID].X + 1.5f, field.Player[ID].Y, field.Item.Count)); }
            field.Player[ID].P.CountBullets = 0; field.Player[ID].P.CountMagazine = 0;
            field.Player[ID].S.CountBullets = 0; field.Player[ID].S.CountMagazine = 0;
            field.Player[ID].G.CountBullets = 0; field.Player[ID].G.CountMagazine = 0;
            field.Player[ID].B.CountBullets = 0; field.Player[ID].B.CountMagazine = 0;
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
