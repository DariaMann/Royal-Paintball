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
        public float X { get; set; }
        public float Y { get; set; }
        public int[] Size { get; set; }
        public List<Tree> Tree { get; set; }
        public List<Wall> Wall { get; set; }
        public List<Bullet> Bullet { get; set; }
        public List<Item> Item { get; set; }
        public Dictionary<int, Player> Player { get; set; }
        public Circle circle { get; set; }

        public List<string> Colors { get; set; }
        public TimeSpan time;
        
        public Field()
        {
            X = 0;
            Y = 0;
            Size = new int[2] {50,50};
            Tree = new List<Tree>();
            Tree.Add(
                new Tree
                {
                    X = 8,
                    Y = 0,
                    Type = "Oak"
                });
            Tree.Add(
                 new Tree
                 {
                     X = 7,
                     Y = 9,
                     Type = "Fir"

                 });
           
            Wall = new List<Wall>();
            //Wall.Add(
            //    new Wall(0, 0)
            //    {
            //        X = 0,
            //        Y = 0
            //    });
            Wall.Add(
               new Wall(3, 0)
               {
                   X = 3,
                   Y = 0
               });
            Bullet = new List<Bullet>();
            Item = new List<Item>();
            this.time = new TimeSpan();
            Player = new Dictionary<int, Player>();
            circle = new Circle();
            Colors = new List<string> { "blue", "red", "yellow","orange", "pink", "green", "black", "white" };
           
        }
        public void AllTrees()
        {
            
        }
        public void AllWalls()
        {
            //Walls.Add(1, new Wall(-18, -38));

        }

       
    }
}
