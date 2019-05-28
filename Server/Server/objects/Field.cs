using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json;

namespace Server
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
                Tree.Add(new Tree(x, y, type));
            }
            Wall = new List<Wall>();
            for (int i = 0; i < 30; i++)
            {
                float x = rn.Next(-40, 40); //rn.Next(-8, 8);
                float y = rn.Next(-40, 40);//rn.Next(-4, 4);
                Wall.Add(new Wall(x, y));
            }
            Bullet = new List<Bullet>();
            Item = new Dictionary<int, Item>();
            for (int i = 0; i < 10; i++)
            {
                float x = rn.Next(-40, 40); //rn.Next(-8, 8);
                float y = rn.Next(-40, 40);//rn.Next(-4, 4);
                Item.Add(Item.Count, new Item("Kit", 5, x, y, Item.Count));
            }

            for (int i = 0; i < 10; i++)
            {
                float x = rn.Next(-40, 40); //rn.Next(-8, 8);
                float y = rn.Next(-40, 40);//rn.Next(-4, 4);
                Item.Add(Item.Count, new Item("Pistol", 5, x, y, Item.Count));
            }
            this.time = new TimeSpan();
            Player = new Dictionary<int, Player>();
            circle = new Circle();
            Colors = new List<string> { "blue", "red", "yellow", "orange", "pink", "green", "black", "white" };
        }
        public string ChooseColor()
        {
            Random rn = new Random();
            int k = rn.Next(0, Colors.Count - 1);
            string chosenColor = Colors[k];
            Colors.Remove(chosenColor);
            return chosenColor;
        }

        public object Clone()
        {

            Field f = new Field();

            f.X = this.X;
            f.Y = this.Y;
            f.Size = (int[])this.Size.Clone();
                for (int i = 0; i < this.Tree.Count; i++)
            {
                f.Tree[i] = (Tree)this.Tree[i].Clone();
            }
            Wall = new List<Wall>(this.Wall);
            for (int i = 0; i < this.Bullet.Count; i++)
            {
                try
                {
                    f.Bullet[i] = (Bullet)this.Bullet[i].Clone();
                }
                catch
                {
                    f.Bullet.Add((Bullet)this.Bullet[i].Clone());
                }
            }
            foreach (int i in this.Item.Keys)
            {
                try
                {
                    f.Item[i] = (Item)this.Item[i].Clone();
                }
                catch
                {
                    f.Item.Add(i,(Item)this.Item[i].Clone());
                }
            }
            Player = new Dictionary<int, Player>(this.Player);

            circle = (Circle)this.circle.Clone();
            Colors = new List<string>(this.Colors);
            time = this.time;
            inpulse = this.inpulse;
            return f;
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
        
        public void DecreaseInLives()
        {
            if (time.Seconds == inpulse.Second)
            {
                foreach (int id in Player.Keys)
                {
                    if (Player[id].OutCircle)
                    {
                        Player[id].Wound(1);
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
            //{
            //    if (player.X + 2 < bX)
            //    {
            //        if (!player.StopIn.ContainsKey("A"))
            //            player.StopIn.Add("A", "A");
            //    }
            //    else
            //    {
            //        if (player.X + 1 > bX)
            //        {
            //            if (!player.StopIn.ContainsKey("D"))
            //                player.StopIn.Add("D", "D");
            //        }
            //        else
            //        {
            //            if (player.Y + 1 < bY)
            //            {
            //                if (!player.StopIn.ContainsKey("S"))
            //                    player.StopIn.Add("S", "S");
            //            }
            //            else
            //            {
            //                if (player.Y + 1 > bY)
            //                {
            //                    if (!player.StopIn.ContainsKey("W"))
            //                        player.StopIn.Add("W", "W");
            //                }
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    player.StopIn.Clear();
            //}
        
            {
                if (player.X + 2 < bX)
                {
                    if (!player.StopIn.ContainsKey("A"))

                        player.StopIn.Add("A", "A");
                    Console.WriteLine("RIGHT");
                }
                else
                {
                    if (player.X + 1 > bX)
                    {
                        if (!player.StopIn.ContainsKey("D"))

                            player.StopIn.Add("D", "D");
                        Console.WriteLine("LEFT");
                    }
                }

                if (player.Y + 1 < bY)
                {
                    if (!player.StopIn.ContainsKey("S"))

                        player.StopIn.Add("S", "S");
                    Console.WriteLine("UP");
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
            else
            {
                player.StopIn.Clear();
            }
        }
    }
}