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
        private Field field;//поле игры
        TimerCallback tm = new TimerCallback(Count);
        public Timer timer { get; set; }
        static int i;
        public static void Count(object obj)
        {
            i++;
            Console.WriteLine(i);
           // BulFlight();
        }

        public GameController(Field field)
        {
            this.field = field;

            i = 0;
            this.timer = new Timer(tm, i, 0, 1000);
        }
       
        public Dictionary<string, Dictionary<string, string>> LiftItem(string playerID, Dictionary<string, Dictionary<string, string>> dasha)//поднятие вещей
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
            return dasha;
        }

        public void Woundd(string ID)//ранение
        {
            if (field.Players[ID].Lifes > 0)
            {
                field.Players[ID].Lifes -= 1;
            }
        }

        public void ChangeWeapon(string ID, string weap)//смена оружия
        {
            //field.Players[ID].Weap.ChangeWeap(field,weap,ID);
            if (weap == "Pistol")
            {
                field.Players[ID].Weap = field.Players[ID].P;
            }
            if (weap == "Shotgun")
            {
                field.Players[ID].Weap = field.Players[ID].S;
            }
            if (weap == "Gun")
            {
                field.Players[ID].Weap = field.Players[ID].G;
            }
            if (weap == "Bomb")
            {
                field.Players[ID].Weap = field.Players[ID].B;
            }
            //field.SelectedWeapons = field.Players[ID].Weap ;
            // field.Players[ID].Weap = field.SelectedWeapons;
        }

        public Dictionary<string, Dictionary<string, string>> Reload(string playerID, Dictionary<string, Dictionary<string, string>> dasha)//перезарядка
        {
            string bul = "";
            string mag ="";
            if (field.Players[playerID].Weap.CountMagazine != 0)
            {

                if (field.Players[playerID].Weap.CountBullets == 0)
                {
                    if (field.Players[playerID].Weap.CountMagazine == field.Players[playerID].Weap.MaxCountMag)
                    {
                        //bul = Convert.ToString(Convert.ToInt32(f.Players[playerID].Weap.CountMagazine));
                        //mag = Convert.ToString(Convert.ToInt32(f.Players[playerID].Weap.CountMagazine) - f.Players[playerID].Weap.MaxCountMag);
                        field.Players[playerID].Weap.CountBullets = field.Players[playerID].Weap.CountMagazine;
                        field.Players[playerID].Weap.CountMagazine = field.Players[playerID].Weap.CountMagazine - field.Players[playerID].Weap.MaxCountMag;
                    }
                    else
                    {
                        if (field.Players[playerID].Weap.CountMagazine > field.Players[playerID].Weap.MaxCountMag)
                        {
                            //bul = Convert.ToString(f.Players[playerID].Weap.MaxCountMag);
                            //mag = Convert.ToString(Convert.ToInt32(f.Players[playerID].Weap.CountMagazine) - f.Players[playerID].Weap.MaxCountMag);
                            field.Players[playerID].Weap.CountBullets = field.Players[playerID].Weap.MaxCountMag;
                            field.Players[playerID].Weap.CountMagazine = field.Players[playerID].Weap.CountMagazine - field.Players[playerID].Weap.MaxCountMag;
                        }
                        else
                        {
                            if (field.Players[playerID].Weap.CountMagazine < field.Players[playerID].Weap.MaxCountMag)
                            {
                                //bul = Convert.ToString(Convert.ToInt32(f.Players[playerID].Weap.CountMagazine));
                                //mag = "0";
                                field.Players[playerID].Weap.CountBullets = field.Players[playerID].Weap.CountMagazine;
                                field.Players[playerID].Weap.CountMagazine = 0;
                            }
                        }
                    }
                }

                else
                {
                }
            }
            //(bul != "" && mag != "")
            //{
            //    field.Players[playerID].Weap.Reload(field, playerID, dasha, bul, mag);
            //}

            return dasha;
        }

        public Dictionary<string, Dictionary<string, string>> FinishGame(string playerID, Dictionary<string, Dictionary<string, string>> dasha)//конец игры
        {
            int bulP=0; int bulS=0; int bulG=0; int bulB=0;
            Item I;
            //if (field.P.CountBullets!=0|| field.P.CountMagazine != 0)
            //{
            //    bulP += field.P.CountBullets;
            //    bulP += field.P.CountMagazine;
            //    I= new Item("Pistol", bulP);
            //    field.Items.Add(I);
            //}
            //if (field.S.CountBullets != 0|| field.S.CountMagazine != 0)
            //{
            //    bulS += field.S.CountBullets;
            //    bulS += field.S.CountMagazine;
            //    I = new Item("Shotgun", bulS);
            //    field.Items.Add(I);
            //}
            //if (field.G.CountBullets != 0|| field.G.CountMagazine != 0)
            //{
            //    bulG += field.G.CountBullets;
            //    bulG += field.G.CountMagazine;
            //    I = new Item("Gun", bulG);
            //    field.Items.Add(I);

            //}
           
            //if (field.B.CountBullets != 0|| field.B.CountMagazine != 0)
            //{
            //    bulB += field.B.CountBullets;
            //    bulB += field.B.CountMagazine;
            //    I = new Item("Bomb", bulB);
            //    field.Items.Add(I);

            //}

            //Dictionary<string, string> delPlayer = new Dictionary<string, string>();
            //delPlayer.Add("bulP", Convert.ToString(bulP));
            //delPlayer.Add("bulS", Convert.ToString(bulS));
            //delPlayer.Add("bulG", Convert.ToString(bulG));
            //delPlayer.Add("bulB", Convert.ToString(bulB));
            field.Players.Remove(playerID);
           // dasha[playerID] = delPlayer;
            dasha.Remove(playerID);
            return dasha;
        }

        public Dictionary<string, Dictionary<string, string>> Shoott(Dictionary<string, Dictionary<string, string>> dasha, string playerID)//стрельба
        {

            Bullet bull = new Bullet(Convert.ToSingle(dasha[playerID]["startX"]), Convert.ToSingle(dasha[playerID]["startY"]), Convert.ToSingle(dasha[playerID]["startZ"]), Convert.ToSingle(dasha[playerID]["endX"]), Convert.ToSingle(dasha[playerID]["endY"]), Convert.ToSingle(dasha[playerID]["endZ"]), dasha[playerID]["weapon"], Convert.ToInt32(playerID));

            
            dasha["bullets"].Add(Convert.ToString(field.Bullets.Count) + "x", Convert.ToString(bull.X));
            dasha["bullets"].Add(Convert.ToString(field.Bullets.Count) + "y", Convert.ToString(bull.Y));
            field.Bullets.Add(Convert.ToString(field.Bullets.Count), bull);
            field.Players[playerID].Weap.Shoot(field, playerID,dasha);
          return dasha;
        }
           
        public void BulFlight()//направление полета пули
        {
            
            for (int i = 0; i < field.Bullets.Count; i++)
            {
                //while (field.Bullets[Convert.ToString(i)].X != field.Bullets[Convert.ToString(i)].EndPos[0] && field.Bullets[Convert.ToString(i)].Y != field.Bullets[Convert.ToString(i)].EndPos[1])
                {
                    float speed = 0.1f;

                    float EndPos_X = field.Bullets[Convert.ToString(i)].EndPos[0];//x
                    float EndPos_Y = field.Bullets[Convert.ToString(i)].EndPos[1];//y

                    
                    float X =field.Bullets[Convert.ToString(i)].X ;
                    float Y = field.Bullets[Convert.ToString(i)].Y ;
                    
                    //float y = кх + b;

                    float k = (EndPos_Y - Y) / (EndPos_X - X);
                    float b = -(X * EndPos_Y - EndPos_X * Y) / (EndPos_X - X);

                    float counted_y = k * speed + b;

                    field.Bullets[Convert.ToString(i)].X += speed;                    
                    field.Bullets[Convert.ToString(i)].Y += counted_y;

                    //field.Bullets[Convert.ToString(i)].X += 0.1f;
                    //field.Bullets[Convert.ToString(i)].Y += 0.1f;


                    Console.WriteLine("---------------------------------------------------------------");
                    Console.WriteLine(field.Bullets[Convert.ToString(i)].X);
                    Console.WriteLine(field.Bullets[Convert.ToString(i)].Y);
                    Console.WriteLine("---------------------------------------------------------------");






                }
            }
        }

        public Dictionary<string, Dictionary<string, string>> DelBul(Dictionary<string, Dictionary<string, string>> dasha, string playerID)
        {
            for (int i = 1; i < field.Bullets.Count; i++)
            {
                if (field.Bullets[Convert.ToString(i)].X > 10 || field.Bullets[Convert.ToString(i)].X < -10 || field.Bullets[Convert.ToString(i)].Y > 10 || field.Bullets[Convert.ToString(i)].Y < -10)
                    //dasha["bullets"].Remove(Convert.ToString(i)+"x");
                    //dasha["bullets"].Remove(Convert.ToString(i) + "y");
                    dasha["bullets"][Convert.ToString(i) + "x"] = "N";
                dasha["bullets"][(Convert.ToString(i) + "y")] = "N";
               // field.Bullets.Remove(Convert.ToString(i));
            }
            return dasha;
        }

        public void MovePlayer(string playerID, string direction)//движение игрока
        {
            switch (direction)
            {
                case "W":
                    {
                        field.Players[playerID].Pos[1] = field.Players[playerID].Pos[1] + 0.2f;
                        break;
                    }
                case "S":
                    {
                        field.Players[playerID].Pos[1] = field.Players[playerID].Pos[1] - 0.2f;
                        break;
                    }
                case "A":
                    {
                        field.Players[playerID].Pos[0] = field.Players[playerID].Pos[0] - 0.2f;
                        break;
                    }
                case "D":
                    {
                        field.Players[playerID].Pos[0] = field.Players[playerID].Pos[0] + 0.2f;
                        break;
                    }
            }
        }

        public Dictionary<string, Dictionary<string, string>> PlayerData(string playerID, Dictionary<string, Dictionary<string, string>> dasha)
        {
            //wound
            dasha[playerID]["life"] = Convert.ToString(field.Players[playerID].Lifes);
            //move
            dasha[playerID]["pos_x"] = Convert.ToString(field.Players[playerID].Pos[0]);
            dasha[playerID]["pos_y"] = Convert.ToString(field.Players[playerID].Pos[1]);
            //reload
            dasha[playerID]["bulP"] = Convert.ToString(field.Players[playerID].P.CountBullets);
            dasha[playerID]["magazineP"] = Convert.ToString(field.Players[playerID].P.CountMagazine);

            dasha[playerID]["bulS"] = Convert.ToString(field.Players[playerID].S.CountBullets);
            dasha[playerID]["magazineS"] = Convert.ToString(field.Players[playerID].S.CountMagazine);

            dasha[playerID]["bulG"] = Convert.ToString(field.Players[playerID].G.CountBullets);
            dasha[playerID]["magazineG"] = Convert.ToString(field.Players[playerID].G.CountMagazine);

            dasha[playerID]["bulB"] = Convert.ToString(field.Players[playerID].B.CountBullets);
            dasha[playerID]["magazineB"] = Convert.ToString(field.Players[playerID].B.CountMagazine);

            BulFlight();

            for (int i = 0; i < field.Bullets.Count; i++)
            {
                dasha["bullets"][Convert.ToString(i) + "x"] = Convert.ToString(field.Bullets[Convert.ToString(i)].X);
                dasha["bullets"][Convert.ToString(i) + "y"] = Convert.ToString(field.Bullets[Convert.ToString(i)].Y);

            }
            return dasha;
        }
    }
}
