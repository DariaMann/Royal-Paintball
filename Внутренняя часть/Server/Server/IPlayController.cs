using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public interface IPlayController
    {
        void MovePlayer(Position to);
        void Shoot(Weapons weapon);
        void Select(Weapons weapon);
    }
    
}
