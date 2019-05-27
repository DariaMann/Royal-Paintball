using System;
using System.Collections.Generic;

namespace GameLibrary
{
    public class Field : ICloneable
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

        public float[] LT { get; private set; }
        public float[] RT { get; private set; }
        public float[] LD { get; private set; }
        public float[] RD { get; private set; }

        public Field(List<FirstPlayerData> Waiters)
        {
            Colors = new List<string> { "blue", "red", "yellow", "orange", "pink", "green", "black", "white" };
            X = 0;
            Y = 0;
            inpulse = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 1);
            Size = new int[2] { 50, 50 };

            Tree = new List<Tree>();
            Random rn = new Random(); // объявление переменной для генерации чисел
            for (int i = 0; i < 60; i++)
            {
                float x = rn.Next(-40, 40);
                float y = rn.Next(-40, 40);
                int tree = rn.Next(1, 3);
                string type = "";
                Dictionary<int, string> typeTree = new Dictionary<int, string>();
                typeTree.Add(1, "Oak");
                typeTree.Add(2, "Fir");
                typeTree.Add(3, "Poplar");
                type = typeTree[tree];
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
            for (int i = 0; i < Waiters.Count; i++)
            {
                Player.Add(
                Waiters[i].ID,
                new Player()
                {
                    Name = Waiters[i].Name,
                    ID = Waiters[i].ID,
                    Color = CreateColor()
                });
            }
            circle = new Circle();
        }

        public Field()
        {
            Colors = new List<string> { "blue", "red", "yellow", "orange", "pink", "green", "black", "white" };
            X = 0;
            Y = 0;
            inpulse = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 1);
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

        }

        private string CreateColor()
        {
            Random rn = new Random(); // объявление переменной для генерации чисел
            int col = rn.Next(0, Colors.Count);
            string color = Colors[col];
            Colors.Remove(color);
            return color;
        }

        public void PlayerOutOfCircle(Player player)
        {
            double a = Math.Abs(player.X - circle.X);
            double b = Math.Abs(player.Y - circle.Y);
            double gep = Math.Abs(Math.Sqrt(a * a + b * b));
            if (gep > circle.Radius)
            {
                player.OutCircle = true;
            }
            else
            { player.OutCircle = false; }
        }

        public void PlayerInFrontOfObject(Player player)
        {
            float aX = X - Size[0];
            float aY = Y + Size[1];
            float bX = X + Size[0];
            float bY = Y + Size[1];
            float cX = X + Size[0];
            float cY = Y - Size[1];
            float dX = X - Size[0];
            float dY = Y - Size[1];
            if (player.X < aX || player.X > bX || player.Y < dY || player.Y > aY)
            {
                if (player.X + 2 < bX)
                {
                    if (!player.StopIn.ContainsKey("A"))
                        player.StopIn.Add("A", "A");
                }
                else
                {
                    if (player.X + 1 > bX)
                    {
                        if (!player.StopIn.ContainsKey("D"))
                            player.StopIn.Add("D", "D");
                    }
                    else
                    {
                        if (player.Y + 1 < bY)
                        {
                            if (!player.StopIn.ContainsKey("S"))
                                player.StopIn.Add("S", "S");
                        }
                        else
                        {
                            if (player.Y + 1 > bY)
                            {
                                if (!player.StopIn.ContainsKey("W"))
                                    player.StopIn.Add("W", "W");
                            }
                        }
                    }
                }
            }
            else
            {
                player.StopIn.Clear();
            }
        }

        public void DecreaseInLives()
        {
            if (time.Seconds == inpulse.Second)
            {
                foreach (int id in Player.Keys)
                {
                    if (Player[id].OutCircle)
                    {
                        Player[id].Woundd(1);
                    }
                }
            }
            int second = time.Seconds;
            if (time.Seconds == 59)
            {
                second = 1;

            }
            else
            {
                if (time.Seconds == 58)
                {
                    second = 0;
                }
                else
                {
                    second += 1;
                }
            }
            inpulse = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, time.Minutes, second, time.Milliseconds);
        }

        public object Clone()
        {
            return new Field {
                X = this.X,
                Y = this.Y,
                Size = this.Size,
                Tree = new List<Tree>(this.Tree),
                Wall =  new List<Wall>(this.Wall),
                Bullet = new List<Bullet>(this.Bullet),
                Item = new Dictionary<int, Item>(this.Item),
                Player = new Dictionary<int, Player>(this.Player),
                circle = this.circle,
                Colors = new List<string>(this.Colors),
                time = this.time,
                inpulse = this.inpulse
            };
        }
    }
}