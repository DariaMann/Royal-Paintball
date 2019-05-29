using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;
using Newtonsoft.Json;

namespace Server
{
    public class Logic
    {
        public Field field;//поле игры

        private readonly ConcurrentQueue<Player> queue;
        private readonly ConcurrentQueue<Field> dataForSend;

        DateTime StartTime;
        DateTime now;
        TimeSpan interval;

        private Thread thread;
        private volatile bool stopped;

        public Logic(Field field, ConcurrentQueue<Player> queue, ConcurrentQueue<Field> dataForSend)
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
                    bool result = field.Player[ID].LiftItemInGame(field.Item[i]);
                    if (result)
                    {
                        field.Item.Remove(i);
                    }
                }
            }
        }

        private void Hit(int ID)//ранение
        {
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
                        if (!field.Player[ID].StopIn.ContainsKey("A"))

                            field.Player[ID].StopIn.Add("A", "A");
                    }
                    else
                    {
                        if (field.Player[c].X + 1 > bX)
                        {
                            if (!field.Player[ID].StopIn.ContainsKey("D"))

                                field.Player[ID].StopIn.Add("D", "D");
                        }
                    }

                    if (field.Player[c].Y + 1 < bY)
                    {
                        if (!field.Player[ID].StopIn.ContainsKey("S"))

                            field.Player[ID].StopIn.Add("S", "S");
                    }
                    else
                    {
                        if (field.Player[c].Y + 1 > bY)
                        {
                            if (!field.Player[ID].StopIn.ContainsKey("W"))

                                field.Player[ID].StopIn.Add("W", "W");

                        }
                    }
                    break;
                }
                else
                {
                    field.Player[ID].StopIn.Clear();
                }
            }
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
                                if (!field.Player[ID].StopInWall.ContainsKey("A"))
                                    field.Player[ID].StopInWall.Add("A", "A");
                            }
                            else
                            {
                                if (field.Player[c].X + 1 < bX)
                                {
                                    if (!field.Player[ID].StopInWall.ContainsKey("D"))
                                        field.Player[ID].StopInWall.Add("D", "D");
                                }
                            }

                            if (field.Player[c].Y + 1 > bY)
                            {
                                if (!field.Player[ID].StopInWall.ContainsKey("S"))
                                    field.Player[ID].StopInWall.Add("S", "S");
                            }
                            else
                            {
                                if (field.Player[c].Y + 1 < bY)
                                {
                                    if (!field.Player[ID].StopInWall.ContainsKey("W"))
                                        field.Player[ID].StopInWall.Add("W", "W");
                                }
                            }
                            break;
                        }
                        else
                        {
                            field.Player[ID].StopInWall.Clear();
                        }
                    }
                    else
                    {
                        field.Player[ID].StopInWall.Clear();
                    }
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
            foreach (Player player in field.Player.Values)
            {
                field.PlayerOutOfCircle(player);
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

        private void BulFlight()//направление полета пули
        {

            for (int i = 0; i < field.Bullet.Count; i++)
            {
                TimeSpan intervalBul = field.Bullet[i].time - DateTime.Now;
                if (intervalBul.Negate().Seconds < 2)
                {
                    field.Bullet[i].X += field.Bullet[i].a;
                    field.Bullet[i].Y += field.Bullet[i].b;
                }
                else
                {
                    DelBul(field.Bullet[i]);
                }
            }
        }

        private int TimeForFly(string weapon, int playerID)
        {
            int seconds = 0;
            switch (weapon)
            {
                case "Pistol": seconds = field.Player[playerID].P.TimeFly; break;
                case "Gun": seconds = field.Player[playerID].P.TimeFly; break;
                case "Shotgun": seconds = field.Player[playerID].P.TimeFly; break;
                case "Bomb": seconds = field.Player[playerID].P.TimeFly; break;
            }
            return seconds;
        }

        private void Shoott(int ID, Player player)//стрельба
        {
            float speed = 0.1f;
            field.Bullet.Add(new Bullet(player.End[0], player.End[1], player.Start[0], player.Start[1], player.Weapon, ID, speed, player.Color));
            field.Player[ID].Weap.Shoot();
        }

        private void DelBul(Bullet bul)
        {
            if (field.Bullet.Contains(bul))
                field.Bullet.Remove(bul);
        }

        private void Reaction(Player player)//метод реакции сервера на сообщения клиента
        {
            if (field.Player.ContainsKey(player.ID))
            {
                Hit(player.ID);
                if (!(player.Direction == "N"))//движение игрока
                {
                    field.Player[player.ID].MovePlayer(player.Direction);
                }
                ChangeWeapon(player.ID, player.Weapon);
                if (player.Shoot == true)//выстрел
                {
                    Shoott(player.ID, player);
                }
                if (player.Death)
                {
                    FinishGame(player.ID);
                }
                if (field.Player.ContainsKey(player.ID))
                {
                    if (player.Life <= 0)
                    {
                        field.Player[player.ID].Death = true;
                    }
                    if (field.Player.Count == 1)//поднятие вещей
                    {
                        field.Player[player.ID].Win = true;
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
                    field.circle.SmallCircleMove();
                }
            }
        }

        private void ReactionInTime(Player player)//метод который выполняется независимо от клиента
        {
            if (field.time.Seconds == 30)
            {
                field.circle.SmallCircleCall();
            }
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

        public Field CloneField()
        {
            var mess = JsonConvert.SerializeObject(field, Formatting.Indented);
            Field f2 = JsonConvert.DeserializeObject<Field>(mess);
            return f2;
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

                Field f = field.Clone(field);

                if (dataForSend.Count < 100)
                { this.dataForSend.Enqueue(field); }
            }
        }
    }
}

