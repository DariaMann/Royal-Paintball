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
       public Dictionary<string, Dictionary<string, string>> Players { get; set; }
        public Weapons SelectedWeapons { get; set; }

        public delegate void FieldChanged();
        public event FieldChanged Changed;

        public Field()
        {
             this.Players =  new Dictionary<string, Dictionary<string, string>>();
            this.SelectedWeapons = null;
        }
      
        public void OnFieldChanged()
        {
            this.Changed();
        }

    }
}
