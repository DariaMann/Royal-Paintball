using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{

    public class Weapons
    {
        public Color Color { get; set; }//цвет оружия
        public Color ColorBul { get; set; }//цвет пули
        public double Direction { get; set; }//направление полета пули(местонахождение мыши)
        public int CoutnBullets { get; set; }//количество пуль за один выстрел
        public int Power { get; set; }//сила удара пули
        public double FlightTime { get; set; }//время полета пули
        public double RechargeTime { get; set; }//время для перезарядки оружия
        public int InityalCountBul { get; set; }//начальное количество пуль

        public virtual void Shoot()
        {

        }
    }
  
}
