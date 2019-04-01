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
        public Weapons Weap{ get; set; }
        public string Shoot { get; set; }
        public float[] Pos { get; set; }

        public Player( int id, float x, float y, float z, float xR,Weapons weap)
        {
            this.Weap = weap;
            this.Lifes = 30;
            this.Direction = "N";
            this.Shoot = "F";
            this.Weapon = "Pistol";
            this.Position = new Position();
            Pos = new float[3] {x,y,z};
          //  Random rn = new Random(); // объявление переменной для генерации чисел
            this.ID = id;
        }
    
    }
}
