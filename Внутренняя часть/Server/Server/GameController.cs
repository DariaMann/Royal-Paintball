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
        //public void Wound(Weapons weapon, string playerID)//выстрел
        //{
        //    Player player;
        //    player = field.Players[playerID];
        //    player.Lifes -= weapon.TakenLives;
        //    if (player.Lifes<=0)
        //    {
        //        FinishGame(playerID);
        //    }
        //}
        public void NewPlayer(int playerID,Position position,Rotation rotation,Color color)
        {
            //Random rn = new Random();
            //playerID = rn.Next(10, 99);
            //foreach (int c in field.Players.Keys)
            //{
            //    // if(field.Players.Keys==playerID)
            //    if (c == playerID)
            //    {
            //        playerID = rn.Next(10, 99);
            //    }

            //}
          //  playerID = ;
         //   Player player = new Player(color,playerID,position,rotation);
            //field.Players.Add(playerID,player);
        }
        public void FinishGame(string playerID)
        {
            field.Players.Remove(playerID);
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

    }
}
