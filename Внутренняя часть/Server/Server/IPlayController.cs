using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public interface IPlayController
    {
        void MovePlayer(int playerID, string side);
        void Shoot(string  weapon, int playerID);
        void SelectWeapon(string weapon, int playerID);
    }
    
}
