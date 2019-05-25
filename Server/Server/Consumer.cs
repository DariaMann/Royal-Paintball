using System;
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
        private readonly ConcurrentQueue<Field> dataForSend;
        DateTime StartTime;
        DateTime now;
        TimeSpan interval;
        private Thread thread;
        private volatile bool stopped;

        public Consumer(Field field, ConcurrentQueue<Player> queue, ConcurrentQueue<Field> dataForSend)
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
            foreach (Player player in field.Player.Values)
            {
                List<int> list = new List<int>();
                foreach (int i in field.Item.Keys)
                {
                    list.Add(i);
                }
                foreach (int i in list)
                {
                    bool result = player.LiftItemInGame(field.Item[i]);
                    if (result)
                    { field.Item.Remove(i); }
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

        private void Hit()
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
                                if (!field.Player[c].StopIn.ContainsKey("A"))
                                    field.Player[c].StopIn.Add("A", "A");
                            }
                            else
                            {
                                if (field.Player[c].X + 1 < bX)
                                {
                                    if (!field.Player[c].StopIn.ContainsKey("D"))
                                        field.Player[c].StopIn.Add("D", "D");
                                }
                            }

                            if (field.Player[c].Y + 1 > bY)
                            {
                                if (!field.Player[c].StopIn.ContainsKey("S"))
                                    field.Player[c].StopIn.Add("S", "S");
                            }
                            else
                            {
                                if (field.Player[c].Y + 1 < bY)
                                {
                                    if (!field.Player[c].StopIn.ContainsKey("W"))
                                        field.Player[c].StopIn.Add("W", "W");
                                }
                            }
                            break;
                        }
                        else
                        {
                            field.Player[c].StopIn.Clear();
                        }
                    }
                    else
                    {
                        field.Player[c].StopIn.Clear();
                    }
                }
            }//игрок у стены или у края зоны
            foreach (int c in field.Player.Keys)
            {
                float aX = field.X - field.Size[0];
                float aY = field.Y + field.Size[1];
                float bX = field.X + field.Size[0];
                float bY = field.Y + field.Size[1];
                float cX = field.X + field.Size[0];
                float cY = field.Y - field.Size[1];
                float dX = field.X - field.Size[0];
                float dY = field.Y - field.Size[1];
                if (field.Player[c].X < aX || field.Player[c].X > bX || field.Player[c].Y < dY || field.Player[c].Y > aY)
                {
                    if (field.Player[c].X + 2 < bX)
                    {
                        if (!field.Player[c].StopIn.ContainsKey("A"))
                            field.Player[c].StopIn.Add("A", "A");
                    }
                    else
                    {
                        if (field.Player[c].X + 1 > bX)
                        {
                            if (!field.Player[c].StopIn.ContainsKey("D"))
                                field.Player[c].StopIn.Add("D", "D");
                        }
                        else
                        {
                            if (field.Player[c].Y + 1 < bY)
                            {
                                if (!field.Player[c].StopIn.ContainsKey("S"))
                                    field.Player[c].StopIn.Add("S", "S");
                            }
                            else
                            {
                                if (field.Player[c].Y + 1 > bY)
                                {
                                    if (!field.Player[c].StopIn.ContainsKey("W"))
                                        field.Player[c].StopIn.Add("W", "W");
                                }
                            }
                        }
                    }
                    break;
                }
                else
                {
                    field.Player[c].StopIn.Clear();
                }
            }

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
            }//пуля во что то врезалась
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
                                int countTakenLife = WeapWound(field.Bullet[i].Weapon, c);
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
                if (gep > field.circle.Radius)
                {
                    field.Player[c].OutCircle = true;
                }
                else
                { field.Player[c].OutCircle = false; }
            }//игрок вне круга
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
            if (field.Player[ID].Weap.CamShot)
            {
                if (field.Player[ID].Weap.CountBullets != 0)
                {
                    float speed = 0.1f;
                    field.Bullet.Add(new Bullet(player.End[0], player.End[1], player.Start[0], player.Start[1], player.Weapon, ID, speed, player.Color, field.Player[ID].Weap));
                    field.Player[ID].Weap.Shoot();
                }
            }
        }

        //private void BulFlight()//направление полета пули
        //{
        //    for (int i = 0; i < field.Bullet.Count; i++)
        //    {
        //        TimeSpan intervalBul = field.Bullet[i].time - now;
        //        Console.WriteLine(intervalBul.Negate().Seconds);
        //        if (intervalBul.Negate().Seconds < 2)
        //        {
        //            field.Bullet[i].X += field.Bullet[i].a;
        //            field.Bullet[i].Y += field.Bullet[i].b;
        //        }
        //        else
        //        {
        //            DelBul(field.Bullet[i]);
        //        }
        //    }
        //}
        private void BulFlight()
        {
            for (int i = 0; i < field.Bullet.Count; i++)//полет пули
            {
                bool result = field.Bullet[i].BulFlight();
                if (!result)
                {
                    DelBul(field.Bullet[i]);
                }
            }
        }

        private void DelBul(Bullet bul)
        {
            if (field.Bullet.Contains(bul))
                field.Bullet.Remove(bul);
        }

        private void Hit2()
        {
            for (int i = 0; i < field.Bullet.Count; i++)
            {
                foreach (Wall wall in field.Wall)
                {
                    if (field.Bullet[i].BulletInObject(wall))
                    {
                        DelBul(field.Bullet[i]);
                        break;
                    }
                }
            }
            for (int i = 0; i < field.Bullet.Count; i++)
            {
                foreach (Tree tree in field.Tree)
                {
                    if (field.Bullet[i].BulletInObject(tree))
                    {
                        DelBul(field.Bullet[i]);
                        break;
                    }
                }
            }
            for (int i = 0; i < field.Bullet.Count; i++)
            {
                foreach (Player player in field.Player.Values)
                {
                    if (field.Bullet[i].BulletInObject(player))
                    {
                        DelBul(field.Bullet[i]);
                        break;
                    }
                }
            }
            foreach (Player player in field.Player.Values)
            {
                field.PlayerOutOfCircle(player);
            }
        }

        private void Reaction(Player player)//метод реакции сервера на сообщения клиента
        {
            if (!(player.Direction == "N"))//движение игрока
            {
                field.Player[player.ID].MovePlayer(player.Direction);
            }
            field.Player[player.ID].ChangeWeapon(player.Weapon);
            if (player.Shoot == true)//выстрел
            {
                Shoott(player.ID, player);
            }
            if (player.Death)
            {
                FinishGame(player.ID);
            }
            if (player.LiftItem == true)//поднятие вещей
            {
                LiftItem(player.ID);
            }
            if (player.Reload == true)//перезарядка
            {
                field.Player[player.ID].ReloadCall();//вызов перезарядки
            }
            field.Player[player.ID].ReloadWeapon();//перезарядка
            BulFlight();
            field.circle.SmallCircleMove();//уменьшение круга
        }


        private void ReactionInTime(Player player)//метод который выполняется независимо от клиента
        {
            if (field.time.Seconds == 30)
            {
                field.circle.SmallCircleCall();
            }
            Hit2();
            // Hit();
            field.time = interval.Negate();
            field.DecreaseInLives();
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
                Thread.Sleep(50);
                if (this.queue.TryDequeue(out Player pl))
                {
                    Reaction(pl);
                }
                now = DateTime.Now;
                interval = StartTime - now;

                ReactionInTime(pl);

                Field f = field;
                if (field.Player.Count != 0)
                {
                    if (dataForSend.Count < 1)
                    { this.dataForSend.Enqueue(f); }
                }
            }
        }
    }
}

