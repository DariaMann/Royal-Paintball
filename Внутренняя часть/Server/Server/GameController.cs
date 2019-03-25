using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class GameController: IPlayController
    {
        private Field field;

        public GameController(Field field)
        {
            this.field = field;
        }
        public void SelectWeapon(string weapon,int playerID)
        {
            Player player;
            player = field.Players[playerID];
            //this.field.SelectedWeapons = weapon;
            //this.field.OnFieldChanged();
        }
        public void Wound(Weapons weapon, int playerID)//выстрел
        {
            Player player;
            player = field.Players[playerID];
            player.Lifes -= weapon.TakenLives;
            if (player.Lifes<=0)
            {
                FinishGame(playerID);
            }
        }
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
        public void FinishGame(int playerID)
        {
            field.Players.Remove(playerID);
        }
        public Dictionary<string, Dictionary<string, string>> MovePlayer(int playerID, Dictionary<string, Dictionary<string, string>> str)
        {
            Player player;
            player = field.Players[playerID];
            switch (str[Convert.ToString(playerID)]["dir"])
            {
                case "W":
                    {

                        str[Convert.ToString(playerID)]["pos_y"] = Convert.ToString(player.Position.Y++);
                        break;
                    }
                case "S":
                    {
                        str[Convert.ToString(playerID)]["pos_y"] = Convert.ToString(player.Position.Y--);
                        break;
                    }
                case "A":
                    {
                        str[Convert.ToString(playerID)]["pos_x"] = Convert.ToString(player.Position.X--);
                        break;
                    }
                case "D":
                    {
                        str[Convert.ToString(playerID)]["pos_x"] = Convert.ToString(player.Position.X++);
                         break;
                    }
            }
            return str;
        
        }
        public void WeaponRotation(string mousPos, int playerID)
        {

        }
        public void Shoot(string weapon, int playerID)
        {
            Player player;
            player = field.Players[playerID];
            player.Lifes--;
            switch (weapon)
            {
                case "Pistol":
                    {
                        Pistol p = new Pistol();
                        p.Shoot();
                        break;
                    }
                case "Shotgun":
                    {
                        Shotgun s = new Shotgun();
                        s.Shoot();
                        break;
                    }
                case "Gun":
                    {
                        Gun g = new Gun();
                        g.Shoot();
                        break;
                    }
                case "Bomb":
                    {
                       Bomb b = new Bomb();
                        b.Shoot();
                        break;
                    }
            }

        }

    }
}
