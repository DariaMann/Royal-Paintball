using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Server
{
    public class GameObject
    {
        public Color Color { get; set; }
        public Position Position { get; set; }
        public int Lifes { get; set; }
        public Rotation Rotation { get; set; }
    }
}
