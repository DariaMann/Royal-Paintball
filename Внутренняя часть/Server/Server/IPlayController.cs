using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public interface IPlayController
    {
        Dictionary<string, Dictionary<string, string>> MovePlayer(int playerID, Dictionary<string, Dictionary<string, string>> str);
        void Shoot(string  weapon, int playerID);
        void SelectWeapon(string weapon, int playerID);
    }
    
}
