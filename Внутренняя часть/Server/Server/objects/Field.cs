using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Field
    {
        public Color color { get; set; }
        //public IList<Player> Players { get; set; }
       public Dictionary<string,Player> Players { get; set; }
       public List<Bullet> Bull = new List<Bullet>();
        public Dictionary<string, Bullet> Bullets { get; set; }
        public Weapons SelectedWeapons { get; set; }

        //public delegate void FieldChanged();
        //public event FieldChanged Changed;

        public Field()
        {
            this.Bull = new List<Bullet>();
             this.Players =  new Dictionary<string, Player>();
            this.Bullets = new Dictionary<string, Bullet>();
            this.SelectedWeapons = null;
        }
      
        //public void OnFieldChanged()
        //{
        //    this.Changed();
        //}

    }
}
