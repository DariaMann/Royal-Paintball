using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class GameController: IPlayController
    {
        private Field field;//поле игры


        public GameController(Field field)
        {
            this.field = field;
        }
       
        public Dictionary<string, Dictionary<string, string>> LiftItem(string playerID, Dictionary<string, Dictionary<string, string>> dasha)//поднятие вещей
        {
            if (field.Items.Count != 0)
            {
                for (int i = 0; i < field.Items.Count; i++) {
                    field.Players[playerID].Weap.CountMagazine += field.Items[i].Count;
                    
                    
                    switch (field.Items[i].Name)
                    {
                        case "Pistol":
                            {
                                dasha[playerID]["magazineP"] = Convert.ToString(field.Players[playerID].Weap.CountMagazine);
                                break;
                            }
                        case "Shotgun":
                            {
                                dasha[playerID]["magazineS"] = Convert.ToString(field.Players[playerID].Weap.CountMagazine);
                                break;
                            }
                        case "Gun":
                            {
                                dasha[playerID]["magazineG"] = Convert.ToString(field.Players[playerID].Weap.CountMagazine);
                                break;
                            }
                        case "Bomb":
                            {
                                dasha[playerID]["magazineB"] = Convert.ToString(field.Players[playerID].Weap.CountMagazine);
                                break;
                            }


                    }
                    field.Items.Remove(field.Items[i]);

                }

                } 
            return dasha;
        }
        public Dictionary<string, Dictionary<string, string>> Woundd(string playerID, Dictionary<string, Dictionary<string, string>> dasha,Field f)//выстрел
        {
            if (f.Players[playerID].Lifes > 0)
            {
                f.Players[playerID].Lifes -= 1;
                dasha[playerID]["life"] = Convert.ToString(f.Players[playerID].Lifes);
            }
            else
            {
                if (Convert.ToInt32(dasha[playerID]["life"]) == 0)
                {
                    dasha = FinishGame(playerID, dasha);
                }
            }
            return dasha;
        }
        public Dictionary<string, Dictionary<string, string>> ChangeWeapon(string playerID, Dictionary<string, Dictionary<string, string>> dasha, Field f)//смена оружия
        {
            switch (dasha[playerID]["weapon"])
            {
                case "Pistol": { f.SelectedWeapons = f.P; break; }
                case "Shotgun": { f.SelectedWeapons = f.S; break; }
                case "Gun": { f.SelectedWeapons = f.G; break; }
                case "Bomb": { f.SelectedWeapons = f.B; break; }
            }
            f.Players[playerID].Weap = f.SelectedWeapons;
            return dasha;
        }
        public Dictionary<string, Dictionary<string, string>> Reload(string playerID, Dictionary<string, Dictionary<string, string>> dasha, Field f)//перезарядка
        {
            string bul = "";
            string mag ="";
            if (f.Players[playerID].Weap.CountMagazine != 0)
            {

                if (f.Players[playerID].Weap.CountBullets == 0)
                {
                    if (f.Players[playerID].Weap.CountMagazine == f.Players[playerID].Weap.MaxCountMag)
                    {
                        /*dasha[playerID]["bulP"]*/
                        bul = Convert.ToString(Convert.ToInt32(f.Players[playerID].Weap.CountMagazine));
                        /* dasha[playerID]["magazineP"]*/
                        mag = Convert.ToString(Convert.ToInt32(f.Players[playerID].Weap.CountMagazine) - f.Players[playerID].Weap.MaxCountMag);
                        f.Players[playerID].Weap.CountBullets = f.Players[playerID].Weap.CountMagazine;
                        f.Players[playerID].Weap.CountMagazine = f.Players[playerID].Weap.CountMagazine - f.Players[playerID].Weap.MaxCountMag;
                    }
                    else
                    {
                        if (f.Players[playerID].Weap.CountMagazine > f.Players[playerID].Weap.MaxCountMag)
                        {
                            bul = Convert.ToString(f.Players[playerID].Weap.MaxCountMag);
                            mag = Convert.ToString(Convert.ToInt32(f.Players[playerID].Weap.CountMagazine) - f.Players[playerID].Weap.MaxCountMag);
                            f.Players[playerID].Weap.CountBullets = f.Players[playerID].Weap.MaxCountMag;
                            f.Players[playerID].Weap.CountMagazine = f.Players[playerID].Weap.CountMagazine - f.Players[playerID].Weap.MaxCountMag;
                        }
                        else
                        {
                            if (f.Players[playerID].Weap.CountMagazine < f.Players[playerID].Weap.MaxCountMag)
                            {
                                bul = Convert.ToString(Convert.ToInt32(f.Players[playerID].Weap.CountMagazine));
                                mag = "0";
                                f.Players[playerID].Weap.CountBullets = f.Players[playerID].Weap.CountMagazine;
                                f.Players[playerID].Weap.CountMagazine = 0;
                            }
                        }
                    }
                }

                else
                {
                    //if (f.Players[playerID].Weap.CountMagazine == f.Players[playerID].Weap.MaxCountMag)
                    ////while (bul != Convert.ToString(f.Players[playerID].Weap.MaxCountMag) || f.Players[playerID].Weap.CountMagazine == 0)
                    ////{
                    ////    f.Players[playerID].Weap.CountBullets += 1;
                    ////    f.Players[playerID].Weap.CountMagazine -= 1;
                    ////}
                    //bul = Convert.ToString(f.Players[playerID].Weap.CountMagazine);
                    //mag = Convert.ToString(f.Players[playerID].Weap.CountBullets);

                }
            }
            if (bul != "" && mag != "")
            {
                switch (dasha[playerID]["weapon"])
                {
                    case "Pistol":
                        {
                            dasha[playerID]["bulP"] = bul;
                            dasha[playerID]["magazineP"] = mag;
                            break;
                        }
                    case "Shotgun":
                        {
                            dasha[playerID]["bulS"] = bul;
                            dasha[playerID]["magazineS"] = mag;
                            break;
                        }
                    case "Gun":
                        {
                            dasha[playerID]["bulG"] = bul;
                            dasha[playerID]["magazineG"] = mag;
                            break;
                        }
                    case "Bomb":
                        {
                            dasha[playerID]["bulB"] = bul;
                            dasha[playerID]["magazineB"] = mag;
                            break;
                        }
                }
            }

            return dasha;
        }
        public Dictionary<string, Dictionary<string, string>> FinishGame(string playerID, Dictionary<string, Dictionary<string, string>> dasha)//конец игры
        {
            int bulP=0; int bulS=0; int bulG=0; int bulB=0;
            Item I;
            if (field.P.CountBullets!=0|| field.P.CountMagazine != 0)
            {
                bulP += field.P.CountBullets;
                bulP += field.P.CountMagazine;
                I= new Item("Pistol", bulP);
                field.Items.Add(I);
            }
            if (field.S.CountBullets != 0|| field.S.CountMagazine != 0)
            {
                bulS += field.S.CountBullets;
                bulS += field.S.CountMagazine;
                I = new Item("Shotgun", bulS);
                field.Items.Add(I);
            }
            if (field.G.CountBullets != 0|| field.G.CountMagazine != 0)
            {
                bulG += field.G.CountBullets;
                bulG += field.G.CountMagazine;
                I = new Item("Gun", bulG);
                field.Items.Add(I);

            }
           
            if (field.B.CountBullets != 0|| field.B.CountMagazine != 0)
            {
                bulB += field.B.CountBullets;
                bulB += field.B.CountMagazine;
                I = new Item("Bomb", bulB);
                field.Items.Add(I);

            }

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
        public Dictionary<string, Dictionary<string, string>> Shoott(Dictionary<string, Dictionary<string, string>> dasha, string playerID,Field f)//стрельба
        {

          Bullet bull = new Bullet(Convert.ToSingle(dasha[playerID]["startX"]), Convert.ToSingle(dasha[playerID]["startY"]), Convert.ToSingle(dasha[playerID]["startZ"]), Convert.ToSingle(dasha[playerID]["endX"]), Convert.ToSingle(dasha[playerID]["endY"]), Convert.ToSingle(dasha[playerID]["endZ"]), dasha[playerID]["weapon"], Convert.ToInt32(playerID));

             // f.Bullets.Add(playerID, bull); 
            f.Bull.Add(bull);
            //int bul = f.Players[playerID].Weap.CountBullets--;//-1 пуля в оружии 
            switch (dasha[playerID]["weapon"])
            {
                case "Pistol":
                    {
                        f.SelectedWeapons = f.P;
                        f.Players[playerID].Weap = f.SelectedWeapons;
                        int bul = f.Players[playerID].Weap.CountBullets--;
                        dasha[playerID]["bulP"] = Convert.ToString(--bul);
                        break;
                    }
                case "Shotgun":
                    {
                        f.SelectedWeapons = f.S;
                        f.Players[playerID].Weap = f.SelectedWeapons;
                        int bul = f.Players[playerID].Weap.CountBullets--;
                        dasha[playerID]["bulS"] = Convert.ToString(--bul);
                        break;
                    }
                case "Gun":
                    {
                        f.SelectedWeapons = f.G;
                        f.Players[playerID].Weap = f.SelectedWeapons;
                        int bul = f.Players[playerID].Weap.CountBullets--;
                        dasha[playerID]["bulG"] = Convert.ToString(--bul);
                        break;
                    }
                case "Bomb":
                    {
                        f.SelectedWeapons = f.B;
                        f.Players[playerID].Weap = f.SelectedWeapons;
                        int bul = f.Players[playerID].Weap.CountBullets--;
                        dasha[playerID]["bulB"] = Convert.ToString(--bul);
                        break;
                    }
            }
            return dasha;
        }
        public Dictionary<string, Dictionary<string, string>> BulFlight(Dictionary<string, Dictionary<string, string>> dasha, string playerID)//направление полета пули
        {
            for (int i = 0; i < field.Bull.Count; i++)
            {
                float xS = field.Bull[i].StartPos[0];
                float yS = field.Bull[i].StartPos[1];
                float zS = field.Bull[i].StartPos[2];
                float xE = field.Bull[i].EndPos[0];
                float yE = field.Bull[i].EndPos[1];
                float zE = field.Bull[i].EndPos[2];
                float a = xE - xS;
                float b = yE - yS;
                float c = a * a + b * b;
                c = Convert.ToSingle(Math.Sqrt(c));//гепотинуза по которой полетит пуля

            }
            return dasha;
        }
        public Dictionary<string, Dictionary<string, string>> MovePlayerr(string playerID, Dictionary<string, Dictionary<string, string>> dasha, Field f)//движение игрока
        {
            Player player;
            player = f.Players[playerID];
            switch (dasha[playerID]["dir"])
            {
                case "W":
                    {
                        player.Pos[1] = Convert.ToSingle(dasha[Convert.ToString(playerID)]["pos_y"])+0.2f;
                        dasha[playerID]["pos_y"] = Convert.ToString(player.Pos[1]);
                        break;
                    }
                case "S":
                    {
                        player.Pos[1] = Convert.ToSingle(dasha[Convert.ToString(playerID)]["pos_y"]) - 0.2f;
                        dasha[playerID]["pos_y"] = Convert.ToString(player.Pos[1]);
                        break;
                    }
                case "A":
                    {
                        player.Pos[0] = Convert.ToSingle(dasha[Convert.ToString(playerID)]["pos_x"]) - 0.2f;
                        dasha[playerID]["pos_x"] = Convert.ToString(player.Pos[0]);
                        break;
                    }
                case "D":
                    {
                        player.Pos[0] = Convert.ToSingle(dasha[Convert.ToString(playerID)]["pos_x"]) + 0.2f;
                        dasha[playerID]["pos_x"] = Convert.ToString(player.Pos[0]);
                        break;
                    }
            }
            return dasha;

        }
    }
}
