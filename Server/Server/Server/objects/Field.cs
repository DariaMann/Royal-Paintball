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
        public Dictionary<string, Bullet> Bullets { get; set; }
        public Dictionary<int, Wall> Walls { get; set; }
        public Dictionary<int, Tree> Trees { get; set; }
        TimerCallback tm = new TimerCallback(Count);
      public Timer timer { get; set; }
        public static int num { get; set; }

        public Field()
        {
             this.Players =  new Dictionary<string, Player>();
            this.Bullets = new Dictionary<string, Bullet>();
            this.Walls = new Dictionary<int, Wall>();
            this.Trees = new Dictionary<int, Tree>();
            
            num = 0;

           this.timer = new Timer(tm, num, 0, 1000);
            AllWalls();
        }
        public void AllTrees()
        {
            
        }
        public void AllWalls()
        {
            Walls.Add(1, new Wall(-18, -38));

        }

        public static void Count(object obj)
        {
           num++;
           Console.WriteLine(num);
        }
       
    }
}
