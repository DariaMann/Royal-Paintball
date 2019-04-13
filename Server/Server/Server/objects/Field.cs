using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Server
{
    public class Field
    {
        public Dictionary<string, Player> Players { get; set; }
        public List<Bullet> Bull { get; set; }
        public List<Item> Items { get; set; }
        public Dictionary<string, Bullet> Bullets { get; set; }
        public Dictionary<int, Wall> Walls { get; set; }
        public Weapons SelectedWeapons { get; set; }
        public List<int> Magazines = new List<int>();
        public Pistol P{ get; set; }
        public Shotgun S { get; set; }
        public Gun G { get; set; }
        public Bomb B { get; set; }
        TimerCallback tm = new TimerCallback(Count);
        public Timer timer { get; set; }
        public static int num { get; set; }

        public Field()
        {
            this.Magazines = new List<int>();
            this.Items = new List<Item>();
            this.Bull = new List<Bullet>();
             this.Players =  new Dictionary<string, Player>();
            this.Bullets = new Dictionary<string, Bullet>();
            this.P = new Pistol();
            this.S = new Shotgun();
            this.G = new Gun();
            this.B = new Bomb();
            this.SelectedWeapons = P;
            num = 0;
           this.timer = new Timer(tm,num, 0, 1000);
        }
        public static void Count(object obj)
        {
          //  int x = (int)obj;
          //  x += 1;
           // Console.WriteLine(x);
          //  num = x;
           num++;
           Console.WriteLine(num);

        }

    }
}
