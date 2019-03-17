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
        public void MovePlayer(int playerID, string side)
        {
            Player player;
            player = field.Players[playerID];
            switch (side)
            {
                case "W": player.Position.Y++; break;
                case "S": player.Position.Y--; break;
                case "A": player.Position.X--; break;
                case "D": player.Position.X++; break;
            }
        
        }
        public void WeaponRotation(string mousPos, int playerID)
        {

        }
        public void Shoot(string weapon, int playerID)
        {
            Player player;
            player = field.Players[playerID];
            Weapons weap;
           // switch (weapon)
            //{
            //    case "pistol": weap.Shoot() ; break;
            //    case "shotgun": player.Position.Y--; break;
            //    case "gun": player.Position.X--; break;
            //    case "bomb": player.Position.X++; break;
            //}
            
        }

    }
}
