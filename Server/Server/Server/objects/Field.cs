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
        public TimeSpan time { get; set; }

        public DateTime inpulse { get; set; }

        public Field()
        {
            X = 0;
            Y = 0;
            inpulse = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second , DateTime.Now.Millisecond);
            Size = new int[2] {50,50};

            Tree = new List<Tree>();
            Random rn = new Random(); // объявление переменной для генерации чисел
            for (int i = 0; i < 60; i++)
            {
               
               float x = rn.Next(-40, 40); //rn.Next(-8, 8);
                float y = rn.Next(-40, 40);//rn.Next(-4, 4);
                int tree = rn.Next(1, 3);
                string type = "";
                if (tree == 1)
                {
                    type = "Oak";
                }
                else
                {
                    if (tree == 2)
                    {
                        type = "Fir";
                    }
                    else
                    {
                        type = "Poplar";
                    }
                }
                Tree.Add(new Tree(x,y,type));
            }
            Wall = new List<Wall>();
            for (int i = 0; i < 30; i++)
            {
                float x = rn.Next(-40, 40); //rn.Next(-8, 8);
                float y = rn.Next(-40, 40);//rn.Next(-4, 4);
                Wall.Add(new Wall(x, y));
            }
            Bullet = new List<Bullet>();
            Item = new List<Item>();
            for (int i = 0; i < 10; i++)
            {
                float x = rn.Next(-40, 40); //rn.Next(-8, 8);
                float y = rn.Next(-40, 40);//rn.Next(-4, 4);
                Item.Add(new Item("Kit", 5, x, y, Item.Count));
            }
            this.time = new TimeSpan();
            Player = new Dictionary<int, Player>();
            circle = new Circle();
            Colors = new List<string> { "blue", "red", "yellow","orange", "pink", "green", "black", "white" };
        }
        public float[] chek()
        {
            Random rn = new Random();
            bool good = false;
            float[] cord = new float[] { 0, 0 };
            while (good != true)
            {
                float x = rn.Next(-40, 40); 
                float y = rn.Next(-40, 40);
                for(int i = 0;i<Tree.Count;i++)
                {
                    float aX = Tree[i].X - Tree[i].Size[0];
                    float aY = Tree[i].Y + Tree[i].Size[1];
                    float bX = Tree[i].X + Tree[i].Size[0];
                    float bY = Tree[i].Y + Tree[i].Size[1];
                    float cX = Tree[i].X + Tree[i].Size[0];
                    float cY = Tree[i].Y - Tree[i].Size[1];
                    float dX = Tree[i].X - Tree[i].Size[0];
                    float dY = Tree[i].Y - Tree[i].Size[1];
                    if (x < aX || x > bX)
                    {
                        if (y < dY || y > aY)
                        {
                            good = true;
                            cord = new float[] { x, y };
                            return cord;
                        }
                        else { i = Tree.Count - 1; }
                    }
                    else
                    {
                        i = Tree.Count-1;
                    }
                }
            }
            return cord;
        }
        public void AllKit()
        {
            float[] cord = new float[] { 0, 0 };
            Item = new List<Item>();
            for (int i = 0; i < 3; i++)
            {
                cord = chek();
                Item.Add(
                new Item("Kit", 5, cord[0], cord[1], Item.Count));
                Console.WriteLine("x: " + cord[0]);
                Console.WriteLine("y: " + cord[1]);
            }
        }
        public void AllWalls()
        {
            float[] cord = new float[] {0,0};
            Wall = new List<Wall>();
            for (int i = 0; i < 3; i++)
            {
                cord = chek();
                Wall.Add(new Wall(cord[0], cord[1]));
                Console.WriteLine("x: " + cord[0]);
                Console.WriteLine("y: " + cord[1]);
            }

        }

       
    }
}
