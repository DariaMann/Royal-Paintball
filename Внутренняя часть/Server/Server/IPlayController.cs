using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public interface IPlayController
    {
        Dictionary<string, Dictionary<string, string>> MovePlayerr(string playerID, Dictionary<string, Dictionary<string, string>> str, Field f);
        Dictionary<string, Dictionary<string, string>> Shoott(Dictionary<string, Dictionary<string, string>> str, string playerID, Field f);
        Dictionary<string, Dictionary<string, string>> ChangeWeapon(string playerID, Dictionary<string, Dictionary<string, string>> str, Field f);
    }
    
}
