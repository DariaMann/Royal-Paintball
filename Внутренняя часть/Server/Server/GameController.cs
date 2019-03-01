using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class GameController: IPlayController
    {
        public Player wearons;
        public Weapons w;


        public GameController(Player wearon)
        {
            this.wearons = wearon;
        }
        public void Select(Weapons weapon)
        {
            //this.wearons.SelectedWearon = weapon;
        }
        public void MovePlayer(Position pos)
        {
           // this.wearons.SelectedWearon = weapon;
        }
        public void Shoot(Weapons weapon)
        {
            
        }

    }
}
