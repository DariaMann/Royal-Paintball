using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class GameController//: IPlayController
    {
        private Field field;

        Pistol p = new Pistol();
        Shotgun s = new Shotgun();
        Gun g = new Gun();
        Bomb b = new Bomb();


        public GameController(Field field)
        {
            this.field = field;
        }
        //public void SelectWeapon(string weapon,string playerID)
        //{
        //    Player player;
        //    player = field.Players[playerID];
        //    //this.field.SelectedWeapons = weapon;
        //    //this.field.OnFieldChanged();
        //}
        public Dictionary<string, Dictionary<string, string>> Wound(string playerID, Dictionary<string, Dictionary<string, string>> dasha)//выстрел
        {
           
           
            if (Convert.ToInt32(dasha[playerID]["life"]) > 0)
            {
                dasha[playerID]["life"]= Convert.ToString(Convert.ToInt32(dasha[playerID]["life"]) - 1);
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
        public Dictionary<string, Dictionary<string, string>> LiftItem(string playerID, Dictionary<string, Dictionary<string, string>> dasha)
        {
            return dasha;
        }
        public Dictionary<string, Dictionary<string, string>> Reload(string playerID, Dictionary<string, Dictionary<string, string>> dasha)
        {
            switch(dasha[playerID]["weapon"])
            {
                case "Pistol":
                    {
                        if(Convert.ToInt32(dasha[playerID]["magazineP"])!=0)
                        {
                            if (Convert.ToInt32(dasha[playerID]["magazineP"]) >= 12)
                            {
                                if (Convert.ToInt32(dasha[playerID]["bulP"]) == 0)
                                { dasha[playerID]["bulP"] = Convert.ToString(Convert.ToInt32(dasha[playerID]["magazineP"]) - 12); }
                                else
                                {
                                    int i = Convert.ToInt32(dasha[playerID]["bulP"]);
                                    dasha[playerID]["bulP"] = dasha[playerID]["magazineP"];
                                    dasha[playerID]["magazineP"] = Convert.ToString( Convert.ToInt32(dasha[playerID]["magazineP"]) + Convert.ToInt32(dasha[playerID]["bulP"]));
                                }
                            }
                            else
                            {
                                dasha[playerID]["bulP"] = dasha[playerID]["magazineP"];
                            }
                        }
                        break;
                    }
                case "Shotgun":
                    {
                        if (Convert.ToInt32(dasha[playerID]["magazineS"]) != 0)
                        { }
                        break;
                    }
                case "Gun":
                    {
                        if (Convert.ToInt32(dasha[playerID]["magazineG"]) != 0)
                        { }
                        break;
                    }
                case "Bomb":
                    {
                        if (Convert.ToInt32(dasha[playerID]["magazineB"]) != 0)
                        { }
                        break;
                    }
            }
            return dasha;
        }
       
        public Dictionary<string, Dictionary<string, string>> FinishGame(string playerID, Dictionary<string, Dictionary<string, string>> dasha)
        {
            //  field.Players.Remove(playerID);
            dasha.Remove(playerID);
            return dasha;
        }
        public Dictionary<string, Dictionary<string, string>> MovePlayer(string playerID, Dictionary<string, Dictionary<string, string>> dasha)
        {
            //Player player;
            //player = field.Players[playerID];
            switch (dasha[playerID]["dir"])
            {
                case "W":
                    {
                        float y = Convert.ToSingle(dasha[Convert.ToString(playerID)]["pos_y"]);
                        string Y = Convert.ToString(y + 0.2);
                        dasha[Convert.ToString(playerID)]["pos_y"] = Y;//Convert.ToString(player.Position.Y++);
                        //Console.WriteLine("y=" + y);
                        //Console.WriteLine("Y=" + Y);
                        //Console.WriteLine(Convert.ToSingle(dasha[Convert.ToString(playerID)]["pos_y"]));
                        break;
                    }
                case "S":
                    {
                        float y = Convert.ToSingle(dasha[Convert.ToString(playerID)]["pos_y"]);
                        dasha[Convert.ToString(playerID)]["pos_y"] = Convert.ToString(y - 0.2); //Convert.ToString(player.Position.Y--);
                        break;
                    }
                case "A":
                    {
                        float x = Convert.ToSingle(dasha[Convert.ToString(playerID)]["pos_x"]);
                        dasha[Convert.ToString(playerID)]["pos_x"] = Convert.ToString(x - 0.2);//Convert.ToString(player.Position.X--);
                        break;
                    }
                case "D":
                    {
                        float x = Convert.ToSingle(dasha[Convert.ToString(playerID)]["pos_x"]);
                        dasha[Convert.ToString(playerID)]["pos_x"] = Convert.ToString(x+0.2); //Convert.ToString(player.Position.X++);
                        break;
                    }
            }
            return dasha;
        
        }
        public void WeaponRotation(string mousPos, int playerID)
        {

        }
        public Dictionary<string, Dictionary<string, string>> Shoot(Dictionary<string, Dictionary<string, string>> dasha, string playerID)
        {

            switch (dasha[playerID]["weapon"])
            {
                case "Pistol":
                    {
                        //Pistol p = new Pistol();
                        //p.Shoot();
                        int bul = Convert.ToInt32(dasha[playerID]["bulP"]);
                        dasha[playerID]["bulP"] = Convert.ToString(--bul);
                        break;
                    }
                case "Shotgun":
                    {
                        int bul = Convert.ToInt32(dasha[playerID]["bulS"]);
                        dasha[playerID]["bulS"] = Convert.ToString(--bul);
                        break;
                    }
                case "Gun":
                    {
                        int bul = Convert.ToInt32(dasha[playerID]["bulG"]);
                        dasha[playerID]["bulG"] = Convert.ToString(--bul);
                        break;
                    }
                case "Bomb":
                    {
                        int bul = Convert.ToInt32(dasha[playerID]["bulB"]);
                        dasha[playerID]["bulB"] = Convert.ToString(--bul);
                        break;
                    }
            }
            return dasha;

        }



        
        public Dictionary<string, Dictionary<string, string>> Shoott(Dictionary<string, Dictionary<string, string>> dasha, string playerID,Field f)
        {

           Bullet bull = new Bullet(Convert.ToSingle(dasha[playerID]["startX"]), Convert.ToSingle(dasha[playerID]["startY"]), Convert.ToSingle(dasha[playerID]["startZ"]), Convert.ToSingle(dasha[playerID]["endX"]), Convert.ToSingle(dasha[playerID]["endY"]), Convert.ToSingle(dasha[playerID]["endZ"]), dasha[playerID]["weapon"], Convert.ToInt32(playerID));

            //  f.Bullets.Add(playerID, bull); 
            f.Bull.Add(bull);
            int bul = f.Players[playerID].Weap.CountBullets--;//-1 пуля в оружии 
            switch (dasha[playerID]["weapon"])
            {
                case "Pistol":{ dasha[playerID]["bulP"] = Convert.ToString(--bul);  break; }
                case "Shotgun":{ dasha[playerID]["bulP"] = Convert.ToString(--bul); break;}
                case "Gun":{dasha[playerID]["bulP"] = Convert.ToString(--bul); break; }
                case "Bomb":{ dasha[playerID]["bulP"] = Convert.ToString(--bul); break;}
            }
            return dasha;
        }
        public Dictionary<string, Dictionary<string, string>> BulFlight(Dictionary<string, Dictionary<string, string>> dasha, string playerID)
        {
            return dasha;
        }
        public Dictionary<string, Dictionary<string, string>> MovePlayerr(string playerID, Dictionary<string, Dictionary<string, string>> dasha, Field f)
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
