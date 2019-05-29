using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json;

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
                List<string> types = new List<string>();
                types.Add("Oak");
                types.Add("Fir");
                types.Add("Poplar");
                type = types[tree];
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
                int weapon = rn.Next(1, 4);
                string type = "";
                List<string> types = new List<string>();
                types.Add("Pistol");
                types.Add("Shotgun");
                types.Add("Gun");
                types.Add("Bomb");
                type = types[weapon];
                Item.Add(Item.Count, new Item(type, 5, x, y, Item.Count));
            }
            this.time = new TimeSpan();
            Player = new Dictionary<int, Player>();
            circle = new Circle();
            Colors = new List<string> { "blue", "red", "yellow", "orange", "pink", "green", "black", "white" };
        }

        public Field Clone()
        {
            string f1 = JsonConvert.SerializeObject(this, Formatting.Indented);
            Field f2 = JsonConvert.DeserializeObject<Field>(f1);
            return f2;
        }

        public string ChooseColor()
        {
            Random rn = new Random();
            int k = rn.Next(0, Colors.Count - 1);
            string chosenColor = Colors[k];
            Colors.Remove(chosenColor);
            return chosenColor;
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
    }
}