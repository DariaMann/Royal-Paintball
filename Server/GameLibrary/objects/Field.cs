using System;
using System.Collections.Generic;

namespace GameLibrary
{
    public class Field
    {
        public float X { get; set; }
        public float Y { get; set; }
        public int[] Size { get; set; }
        public List<Tree> Tree { get; set; }
        public List<Wall> Wall { get; set; }
        public List<Bullet> Bullet { get; set; }
        public Dictionary<int, Item> Item { get; set; }
        public Dictionary<int, Player> Player { get; set; }
        public Circle circle { get; set; }

        public List<string> Colors { get; set; }
        public TimeSpan time { get; set; }

        public DateTime inpulse { get; set; }

        public Field()
        {
            X = 0;
            Y = 0;
            inpulse = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
            Size = new int[2] { 50, 50 };

            Tree = new List<Tree>();
            Random rn = new Random(); // объявление переменной для генерации чисел
            for (int i = 0; i < 60; i++)
            {

                float x = rn.Next(-40, 40);
                float y = rn.Next(-40, 40);
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
                Tree.Add(new Tree(x, y, type));
            }
            Wall = new List<Wall>();
            for (int i = 0; i < 30; i++)
            {
                float x = rn.Next(-40, 40); 
                float y = rn.Next(-40, 40);
                Wall.Add(new Wall(x, y));
            }
            Bullet = new List<Bullet>();
            Item = new Dictionary<int, Item>();
            for (int i = 0; i < 10; i++)
            {
                float x = rn.Next(-40, 40);
                float y = rn.Next(-40, 40);
                Item.Add(Item.Count, new Item("Kit", 5, x, y, Item.Count));
            }
            
            for (int i = 0; i < 10; i++)
            {
                float x = rn.Next(-40, 40); 
                float y = rn.Next(-40, 40);
                Item.Add(Item.Count, new Item("Pistol", 5, x, y, Item.Count));
            }
            this.time = new TimeSpan();
            Player = new Dictionary<int, Player>();
            circle = new Circle();
            Colors = new List<string> { "blue", "red", "yellow", "orange", "pink", "green", "black", "white" };
        }
    }
}