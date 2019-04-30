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
       // public Dictionary<int, Wall> Walls { get; set; }
      //  public Dictionary<int, Tree> Trees { get; set; }

        public List<Tree> Tree { get; set; }
        public List<Wall> Wall { get; set; }
        public List<Bullet> Bullet { get; set; }
        public List<Item> Item { get; set; }
        public Dictionary<int, Player> Player { get; set; }
        public Circle circle { get; set; }

      TimerCallback tm = new TimerCallback(Count);
      public Timer timer { get; set; }
        public static int num { get; set; }

        public Field()
        {
            Tree = new List<Tree>();
            Tree.Add(
                new Tree
                {
                    X = 8,
                    Y = 0
                });
            Tree.Add(
                 new Tree
                 {
                     X = 7,
                     Y = 9
                 });
           
            Wall = new List<Wall>();
            Wall.Add(
                new Wall(0, 0)
                {
                    X = 0,
                    Y = 0
                });
            Wall.Add(
               new Wall(3, 0)
               {
                   X = 3,
                   Y = 0
               });
            Bullet = new List<Bullet>();
            Item = new List<Item>();

            Player = new Dictionary<int, Player>();
            circle = new Circle();
            num = 0;
            
           this.timer = new Timer(tm, num, 0, 1000);
            
        }
        public void AllTrees()
        {
            
        }
        public void AllWalls()
        {
            //Walls.Add(1, new Wall(-18, -38));

        }

        public static void Count(object obj)
        {
           num++;

            // Console.WriteLine(num);
        }
       
    }
}
