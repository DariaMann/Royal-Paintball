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

        public Player(double x, double y, double z,double xR)//(Color color, int ID, Position pos, Rotation rot)
        {
            // this.Color = color;
            this.Lifes = 30;
            //   this.Rotation.X = xR;
            //this.Position.X = x;
            //this.Position.Y = y;
            //this.Position.Z = z;
            this.Position = new Position();
            Random rn = new Random(); // объявление переменной для генерации чисел
            this.ID = rn.Next(0,10000);
        }
        //public Player(int id, Position pos, Rotation rot)//(Color color, int ID, Position pos, Rotation rot)
        //{   
        //   // this.Color = color;
        //    this.Lifes = 30;
        //    this.Rotation = rot;
        //   this.Position = pos;
        //    this.ID = id;
        //}

    }
}
