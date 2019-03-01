using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Player : GameObject
    {
        public Weapons SelectedFigure { get; set; }
        public Player(Color color)
        {   
            this.Color = color;
        }
       //public void SelectedWearon()
       // {

       // }
    }
}
