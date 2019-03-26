using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Player //: GameObject
    {
        public Weapons SelectedFigure { get; set; }//выбранная фигура
        public Color Color { get; set; }
        public Position Position { get; set; }
        public int Lifes { get; set; }
        public Rotation Rotation { get; set; }
        public int ID { get; set; }
        public string Direction { get; set; }
        public string Weapon { get; set; }
        public string Shoot { get; set; }

        public Player( double x, double y, double z,double xR)
        {
            this.Lifes = 30;
            this.Direction = "N";
            this.Shoot = "F";
            this.Weapon = "Pistol";
            this.Position = new Position();
            Random rn = new Random(); // объявление переменной для генерации чисел
            this.ID = rn.Next(0,10000);
        }
    
    }
}
