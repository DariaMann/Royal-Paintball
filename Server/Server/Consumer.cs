﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using GameLibrary;

namespace Server
{
    public class Consumer : IPlayController
    {
        public Field field;//поле игры

        private readonly ConcurrentQueue<Player> queue;
        private readonly ConcurrentQueue<string> dataForSend;

        DateTime StartTime;
        DateTime now;
        TimeSpan interval;

        private Thread thread;
        private volatile bool stopped;

        public Consumer(Field field, ConcurrentQueue<Player> queue, ConcurrentQueue<string> dataForSend)
        {
            this.dataForSend = dataForSend;
            this.queue = queue;
            this.field = field;
            this.stopped = true;
            StartTime = DateTime.Now;
        }

        public void Start()
        {
            if (stopped)
            {
                thread = new Thread(Process);

                stopped = false;

                thread.Start();
            }
        }

        private void LiftItem(int ID)//поднятие вещей
        {

            foreach (int c in field.Player.Keys)
            {
                List<int> list = new List<int>();
                foreach (int i in field.Item.Keys)
                {
                    list.Add(i);
                }
                foreach (int i in list)
                {
                    float aX = field.Player[c].X - (field.Player[c].Size[0] + 2);
                    float aY = field.Player[c].Y + (field.Player[c].Size[1] + 2);
                    float bX = field.Player[c].X + (field.Player[c].Size[0] + 2);
                    float bY = field.Player[c].Y + (field.Player[c].Size[1] + 2);
                    float cX = field.Player[c].X + (field.Player[c].Size[0] + 2);
                    float cY = field.Player[c].Y - (field.Player[c].Size[1] + 2);
                    float dX = field.Player[c].X - (field.Player[c].Size[0] + 2);
                    float dY = field.Player[c].Y - (field.Player[c].Size[1] + 2);
                    if (field.Item[i].X > aX && field.Item[i].X < bX)
                    {
                        if (field.Item[i].Y > dY && field.Item[i].Y < aY)
                        {
                            switch (field.Item[i].Name)
                            {
                                case "Pistol":
                                    {
                                        field.Player[c].P.CountMagazine += field.Item[i].Count;

                                        break;
                                    }
                                case "Shotgun":
                                    {
                                        field.Player[c].S.CountMagazine += field.Item[i].Count;
                                        break;
                                    }
                                case "Gun":
                                    {

                                        field.Player[c].G.CountMagazine += field.Item[i].Count;
                                        break;
                                    }
                                case "Bomb":
                                    {
                                        field.Player[c].B.CountMagazine += field.Item[i].Count;
                                        break;
                                    }
                                case "Kit":
                                    {
                                        field.Player[c].Life += field.Item[i].Count;
                                        break;
                                    }
                            }
                            field.Item.Remove(i);
                        }
                    }
                }

            }
        }

        private int WeapWound(string weapon, int ID)
        {
            int countLife = 0;
            switch (weapon)
            {
                case "Pistol":
                    {
                        countLife = field.Player[ID].P.TakenLives;
                        break;
                    }
                case "Shotgun":
                    {
                        countLife = field.Player[ID].S.TakenLives;
                        break;
                    }
                case "Gun":
                    {
                        countLife = field.Player[ID].G.TakenLives;
                        break;
                    }
                case "Bomb":
                    {
                        countLife = field.Player[ID].B.TakenLives;
                        break;
                    }
            }
            return countLife;
        }

        private void Woundd(int ID, int takenLifes)//ранение
        {
            if (field.Player[ID].Life > 0)
            {
                field.Player[ID].Life -= takenLifes;
            }
        }

        private void Hit(int ID)//ранение
        {
            foreach (int c in field.Player.Keys)
            {
                foreach (Wall w in field.Wall)
                {
                    float aX = w.X - w.Size[0];
                    float aY = w.Y + w.Size[1];
                    float bX = w.X + w.Size[0];
                    float bY = w.Y + w.Size[1];
                    float cX = w.X + w.Size[0];
                    float cY = w.Y - w.Size[1];
                    float dX = w.X - w.Size[0];
                    float dY = w.Y - w.Size[1];
                    if (field.Player[c].X > aX && field.Player[c].X < bX)
                    {
                        if (field.Player[c].Y > dY && field.Player[c].Y < aY)
                        {
                            if (field.Player[c].X + 1 > bX)
                            {
                                if (!field.Player[ID].StopIn.ContainsKey("A"))
                                    field.Player[ID].StopIn.Add("A", "A");
                                Console.WriteLine("RIGHT");
                            }
                            else
                            {
                                if (field.Player[c].X + 1 < bX)
                                {
                                    if (!field.Player[ID].StopIn.ContainsKey("D"))
                                        field.Player[ID].StopIn.Add("D", "D");
                                    Console.WriteLine("LEFT");
                                }
                            }

                            if (field.Player[c].Y + 1 > bY)
                            {
                                if (!field.Player[ID].StopIn.ContainsKey("S"))
                                    field.Player[ID].StopIn.Add("S", "S");
                                Console.WriteLine("UP");
                            }
                            else
                            {
                                if (field.Player[c].Y + 1 < bY)
                                {
                                    if (!field.Player[ID].StopIn.ContainsKey("W"))
                                        field.Player[ID].StopIn.Add("W", "W");
                                    Console.WriteLine("DOWN");
                                    Console.WriteLine("bX: " + bX);
                                    Console.WriteLine("field.Player[c].Y + 1: " + field.Player[c].Y + 1);

                                }
                            }
                            break;
                        }
                        else
                        {
                            field.Player[ID].StopIn.Clear();
                        }
                    }
                    else
                    {
                        field.Player[ID].StopIn.Clear();
                    }
                }
            }
            //foreach (int c in field.Player.Keys)
            //{
            //    float aX = field.X - field.Size[0];
            //    float aY = field.Y + field.Size[1];
            //    float bX = field.X + field.Size[0];
            //    float bY = field.Y + field.Size[1];
            //    float cX = field.X + field.Size[0];
            //    float cY = field.Y - field.Size[1];
            //    float dX = field.X - field.Size[0];
            //    float dY = field.Y - field.Size[1];
            //    if (field.Player[c].X < aX || field.Player[c].X > bX || field.Player[c].Y < dY || field.Player[c].Y > aY)
            //    {
            //                if (field.Player[c].X + 2 < bX)
            //                {
            //                    field.Player[ID].StopIn.Add("A", "A");
            //                    Console.WriteLine("RIGHT");
            //                }
            //                else
            //                {
            //                    if (field.Player[c].X + 1 > bX)
            //                    {
            //                        field.Player[ID].StopIn.Add("D", "D");
            //                        Console.WriteLine("LEFT");
            //                    }
            //                }

            //                if (field.Player[c].Y + 1 < bY)
            //                {
            //                    field.Player[ID].StopIn.Add("S", "S");
            //                    Console.WriteLine("UP");
            //                }
            //                else
            //                {
            //                    if (field.Player[c].Y + 1 > bY)
            //                    {
            //                        field.Player[ID].StopIn.Add("W", "W");
            //                        Console.WriteLine("DOWN");
            //                        Console.WriteLine("bX: " + bX);
            //                        Console.WriteLine("field.Player[c].Y + 1: " + field.Player[c].Y + 1);

            //                    }
            //                }
            //                break;
            //            }
            //            else
            //            {
            //                field.Player[ID].StopIn.Clear();
            //            }
            //}
            for (int i = 0; i < field.Bullet.Count; i++)
            {
                foreach (Wall c in field.Wall)
                {
                    float aX = c.X - c.Size[0];
                    float aY = c.Y + c.Size[1];
                    float bX = c.X + c.Size[0];
                    float bY = c.Y + c.Size[1];
                    float cX = c.X + c.Size[0];
                    float cY = c.Y - c.Size[1];
                    float dX = c.X - c.Size[0];
                    float dY = c.Y - c.Size[1];
                    if (field.Bullet[i].X > aX && field.Bullet[i].X < bX)
                    {
                        if (field.Bullet[i].Y > dY && field.Bullet[i].Y < aY)
                        {
                            DelBul(field.Bullet[i]);
                            break;
                        }
                    }
                }
            }
            for (int i = 0; i < field.Bullet.Count; i++)
            {
                foreach (Tree c in field.Tree)
                {
                    float aX = c.X - c.Size[0];
                    float aY = c.Y + c.Size[1];
                    float bX = c.X + c.Size[0];
                    float bY = c.Y + c.Size[1];
                    float cX = c.X + c.Size[0];
                    float cY = c.Y - c.Size[1];
                    float dX = c.X - c.Size[0];
                    float dY = c.Y - c.Size[1];
                    if (field.Bullet[i].X > aX && field.Bullet[i].X < bX)
                    {
                        if (field.Bullet[i].Y > dY && field.Bullet[i].Y < aY)
                        {
                            DelBul(field.Bullet[i]);
                            break;
                        }
                    }
                }
            }
            for (int i = 0; i < field.Bullet.Count; i++)
            {
                foreach (int c in field.Player.Keys)
                {
                    float aX = field.Player[c].X - field.Player[c].Size[0];
                    float aY = field.Player[c].Y + field.Player[c].Size[1];
                    float bX = field.Player[c].X + field.Player[c].Size[0];
                    float bY = field.Player[c].Y + field.Player[c].Size[1];
                    float cX = field.Player[c].X + field.Player[c].Size[0];
                    float cY = field.Player[c].Y - field.Player[c].Size[1];
                    float dX = field.Player[c].X - field.Player[c].Size[0];
                    float dY = field.Player[c].Y - field.Player[c].Size[1];
                    if (field.Bullet[i].X > aX && field.Bullet[i].X < bX)
                    {
                        if (field.Bullet[i].Y > dY && field.Bullet[i].Y < aY)
                        {
                            if (field.Player[c].ID != field.Bullet[i].ID)
                            {
                                int countTakenLife = WeapWound(field.Bullet[i].Weapon, ID);
                                Woundd(c, countTakenLife);
                                DelBul(field.Bullet[i]);
                                break;
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < field.Bullet.Count; i++)
            {
                float aX = field.X - field.Size[0];
                float aY = field.Y + field.Size[1];
                float bX = field.X + field.Size[0];
                float bY = field.Y + field.Size[1];
                float cX = field.X + field.Size[0];
                float cY = field.Y - field.Size[1];
                float dX = field.X - field.Size[0];
                float dY = field.Y - field.Size[1];
                if (field.Bullet[i].X < aX || field.Bullet[i].X > bX || field.Bullet[i].Y < dY || field.Bullet[i].Y > aY)
                {
                    DelBul(field.Bullet[i]);

                }
            }
            foreach (int c in field.Player.Keys)
            {
                double a = Math.Abs(field.Player[c].X - field.circle.X);
                double b = Math.Abs(field.Player[c].Y - field.circle.Y);
                double gep = Math.Abs(Math.Sqrt(a * a + b * b));
                //  DateTime dt = new DateTime(now.Year, now.Month, now.Month, now.Day, now.Hour, now.Minute, now.Second + 1);
                if (gep > field.circle.Radius)
                {
                    Woundd(c, 1);
                    field.Player[c].OutCircle = true;
                }
                else
                { field.Player[c].OutCircle = false; }
            }
        }

        private void ChangeWeapon(int ID, string weapon)//смена оружия
        {
            if (weapon == "Pistol")
            {
                field.Player[ID].Weap = field.Player[ID].P;
            }
            if (weapon == "Shotgun")
            {
                field.Player[ID].Weap = field.Player[ID].S;
            }
            if (weapon == "Gun")
            {
                field.Player[ID].Weap = field.Player[ID].G;
            }
            if (weapon == "Bomb")
            {
                field.Player[ID].Weap = field.Player[ID].B;
            }
            field.Player[ID].Weapon = weapon;

        }

        private void ReloadCall(int ID)
        {
            field.Player[ID].Weap.CamShot = false;
            int second = now.Second;
            {
                if (now.Second == 59)
                {
                    second = 1;

                }
                else
                {
                    if (now.Second == 60)
                    {
                        second = 2;
                    }
                    else
                    {
                        second = now.Second + 2;
                    }
                }

            }
            DateTime dt = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, second, now.Millisecond);
            field.Player[ID].Weap.time = dt;
        }

        private void SmallCircleCall()
        {
            field.circle.go = true;
            Random rn = new Random(); // объявление переменной для генерации чисел
            int x = rn.Next(-40, 40); //rn.Next(-8, 8);
            int y = rn.Next(-40, 40);//rn.Next(-4, 4);
            float speed = 0.3f;
            field.circle.Move(x, y, speed);

        }

        private void SmallCircleMove()
        {
            if (field.circle.go == true)
            {
                if (field.circle.X < field.circle.endX - 3 || field.circle.X > field.circle.endX + 3) //(Math.Abs(field.circle.X) > Math.Abs(field.circle.endX)+1&&Math.Abs(field.circle.X) < Math.Abs(field.circle.endX-1))
                {
                    field.circle.X += field.circle.a;
                    field.circle.Y += field.circle.b;
                    if (field.circle.Size[0] > 3)
                    {
                        double i = field.circle.Size[0];
                        field.circle.Size[0] -= 0.01;
                        field.circle.Size[1] -= 0.01;
                        field.circle.Radius = Convert.ToSingle((field.circle.Radius * field.circle.Size[0]) / i);
                    }
                }
                else
                {
                    field.circle.go = false;
                }

            }
        }

        private void Reload(int ID, Player player)//перезарядка
        {
            if (field.Player.ContainsKey(ID))
            {
                if (field.Player[ID].Weap.CamShot == false)
                {
                    if (now.Second == field.Player[ID].Weap.time.Second)
                    {
                        if (field.Player[ID].Weap.CountMagazine != 0)
                        {
                            if (field.Player[ID].Weap.CountBullets == 0)
                            {
                                if (field.Player[ID].Weap.CountMagazine == field.Player[ID].Weap.MaxCountMag)
                                {
                                    field.Player[ID].Weap.CountBullets = field.Player[ID].Weap.CountMagazine;
                                    field.Player[ID].Weap.CountMagazine = field.Player[ID].Weap.CountMagazine - field.Player[ID].Weap.MaxCountMag;
                                }
                                else
                                {
                                    if (field.Player[ID].Weap.CountMagazine > field.Player[ID].Weap.MaxCountMag)
                                    {
                                        field.Player[ID].Weap.CountBullets = field.Player[ID].Weap.MaxCountMag;
                                        field.Player[ID].Weap.CountMagazine = field.Player[ID].Weap.CountMagazine - field.Player[ID].Weap.MaxCountMag;
                                    }
                                    else
                                    {
                                        if (field.Player[ID].Weap.CountMagazine < field.Player[ID].Weap.MaxCountMag)
                                        {
                                            field.Player[ID].Weap.CountBullets = field.Player[ID].Weap.CountMagazine;
                                            field.Player[ID].Weap.CountMagazine = 0;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (!(field.Player[ID].Weap.CountBullets == field.Player[ID].Weap.MaxCountMag))
                                {
                                    if (field.Player[ID].Weap.CountMagazine < (field.Player[ID].Weap.MaxCountMag - field.Player[ID].Weap.CountBullets))
                                    {
                                        field.Player[ID].Weap.CountBullets += field.Player[ID].Weap.CountMagazine;
                                        field.Player[ID].Weap.CountMagazine = 0;
                                    }
                                    else
                                    {
                                        int i = field.Player[ID].Weap.MaxCountMag - field.Player[ID].Weap.CountBullets;
                                        field.Player[ID].Weap.CountBullets = field.Player[ID].Weap.MaxCountMag;
                                        field.Player[ID].Weap.CountMagazine -= i;
                                    }
                                }
                            }
                        }
                        field.Player[ID].Weap.CamShot = true;
                    }
                }
            }
        }

        private void FinishGame(int ID)//конец игры
        {
            int lastID = 0;
            foreach (int i in field.Item.Keys)
            {
                lastID = i;
            }
            int pistol = field.Player[ID].P.CountBullets + field.Player[ID].P.CountMagazine;
            if (pistol != 0)
            {
                lastID += 1;
                field.Item.Add(lastID, new Item("Pistol", pistol, field.Player[ID].X, field.Player[ID].Y + 1.5f, field.Item.Count));
            }
            int shotgun = field.Player[ID].S.CountBullets + field.Player[ID].S.CountMagazine;
            if (shotgun != 0)
            {
                lastID += 1;
                field.Item.Add(lastID, new Item("Shotgun", shotgun, field.Player[ID].X, field.Player[ID].Y - 1.5f, field.Item.Count));
            }
            int gun = field.Player[ID].G.CountBullets + field.Player[ID].G.CountMagazine;
            if (gun != 0)
            {
                lastID += 1;
                field.Item.Add(lastID, new Item("Gun", gun, field.Player[ID].X - 1.5f, field.Player[ID].Y, field.Item.Count));
            }
            int bomb = field.Player[ID].B.CountBullets + field.Player[ID].B.CountMagazine;
            if (bomb != 0)
            {
                lastID += 1;
                field.Item.Add(lastID, new Item("Bomb", bomb, field.Player[ID].X + 1.5f, field.Player[ID].Y, field.Item.Count));
            }
            field.Player[ID].P.CountBullets = 0; field.Player[ID].P.CountMagazine = 0;
            field.Player[ID].S.CountBullets = 0; field.Player[ID].S.CountMagazine = 0;
            field.Player[ID].G.CountBullets = 0; field.Player[ID].G.CountMagazine = 0;
            field.Player[ID].B.CountBullets = 0; field.Player[ID].B.CountMagazine = 0;
            field.Player.Remove(ID);
        }

        private void Shoott(int ID, Player player)//стрельба
        {
            float speed = 0.1f;
            field.Bullet.Add(new Bullet(player.End[0], player.End[1], player.Start[0], player.Start[1], player.Weapon, ID, speed, player.Color));
            field.Player[ID].Weap.Shoot();
        }

        private void BulFlight()//направление полета пули
        {

            for (int i = 0; i < field.Bullet.Count; i++)
            {
                field.Bullet[i].X += field.Bullet[i].a;
                field.Bullet[i].Y += field.Bullet[i].b;
                field.Bullet[i].time = now;
            }
        }

        private void DelBul(Bullet bul)
        {
            if (field.Bullet.Contains(bul))
                field.Bullet.Remove(bul);
        }

        private void SmallCircle(bool i)
        {
            if (i)
            {
                field.circle.Size[0] -= 15;
                field.circle.Size[1] -= 15;
                i = false;
            }
        }

        private void MovePlayer(int ID, string dir)//движение игрока
        {
            float speed = 0.4f;
            switch (dir)
            {
                case "W":
                    {
                        if (!field.Player[ID].StopIn.ContainsKey("W"))
                            field.Player[ID].Y += speed;
                        break;
                    }
                case "S":
                    {
                        if (!field.Player[ID].StopIn.ContainsKey("S"))
                            field.Player[ID].Y -= speed;
                        break;
                    }
                case "A":
                    {
                        if (!field.Player[ID].StopIn.ContainsKey("A"))
                            field.Player[ID].X -= speed;
                        break;
                    }
                case "D":
                    {
                        if (!field.Player[ID].StopIn.ContainsKey("D"))
                            field.Player[ID].X += speed;
                        break;
                    }
            }
        }

        private void FirstMessage(Player player)
        {
            Random rn = new Random(); // объявление переменной для генерации чисел
            bool difer = true;
            int id = rn.Next(0, 1000);
            while (difer)
            {
                if(!field.Player.ContainsKey(id))
                {
                    difer = false;
                }
                else id = rn.Next(0, 1000);
            }
            int col = rn.Next(0, field.Colors.Count);
            string color = field.Colors[col];
            field.Colors.Remove(color);
            field.Player.Add(
               id,
               new Player()
               {
                   Name = player.Name,
                   ID = id,
                   Color = color
               });
        }

        private void Reaction(Player player)//метод реакции сервера на сообщения клиента
        {
            if (player.ID == -1)
            {
                Console.WriteLine("--------------------------------------------------------------");
                FirstMessage(player);
            }
            else
            {
                if (field.Player.ContainsKey(player.ID))
                {
                    Hit(player.ID);
                    if (!(player.Direction == "N"))//движение игрока
                    {
                        MovePlayer(player.ID, player.Direction);
                    }
                    ChangeWeapon(player.ID, player.Weapon);

                    if (player.Shoot == true)//выстрел
                    {
                        Shoott(player.ID, player);
                    }

                    if (player.Life <= 0)
                    {
                        FinishGame(player.ID);
                    }
                    if (player.LiftItem == true)//поднятие вещей
                    {
                        LiftItem(player.ID);
                    }
                    Reload(player.ID, player);
                    if (player.Reload == true)//перезарядка
                    {
                        ReloadCall(player.ID);

                    }
                    BulFlight();
                    SmallCircleMove();
                }
            }
        }

        private void ReactionInTime(Player player)//метод который выполняется независимо от клиента
        {
            if (field.time.Seconds == 30)
            {
                SmallCircleCall();
            }
            field.time = interval.Negate();
        }

        public void Stop()
        {
            if (!stopped)
            {
                this.stopped = true;

                this.thread.Join();
            }

        }

        public void Process()
        {

            while (!stopped)
            {
                if (this.queue.TryDequeue(out Player pl))
                {
                    Reaction(pl);
                }
                now = DateTime.Now;
                interval = StartTime - now;
                ReactionInTime(pl);
              //  Console.WriteLine(dataForSend.Count);
                string mess = JsonConvert.SerializeObject(field, Formatting.Indented);
                if (field.Player.Count != 0)
                {
                    if (dataForSend.Count < 1)
                    { this.dataForSend.Enqueue(mess); }
                }
            }
        }
    }
}

