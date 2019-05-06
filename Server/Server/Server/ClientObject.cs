using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Server
{
    public class ClientObject//Producer
    {
        static public int ID = 555;
        public TcpClient client;
        public Field field;//поле игры
        public GameController cont;
        DateTime StartTime;
        DateTime now;
        TimeSpan interval;
        public ClientObject(TcpClient tcpClient, Field Field)
        {
            this.field = Field;
            this.client = tcpClient;
        }
        public string PlayerData1()//создание игрока 
        {
            string color = First();//генерация ID игрока
            field.Player.Add(
                ID,
                new Player()
                {
                    ID = ID,
                    Color = color
                });
         //   field.time = now.Minute;
            string json = JsonConvert.SerializeObject(field, Formatting.Indented);
            return json;
        }
            public string First()
        {
            Random rn = new Random(); // объявление переменной для генерации чисел
            int id = rn.Next(0, 1000);
            ID = id;
            int col = rn.Next(0, field.Colors.Count);
            string color = field.Colors[col];
            field.Colors.Remove(color);
            return color;
        }
        public Player count(Player player)//метод реакции сервера на сообщения клиента
        {
            cont = new GameController(field);

            if (!(player.Direction == "N"))//движение игрока
            {
                cont.MovePlayer(player.ID, player.Direction);
            }

            cont.ChangeWeapon(player.ID,player.Weapon);
            if (player.Shoot == true)//выстрел
            {
                cont.Shoott(player.ID,player);
            }

                 cont.Hit(ID);

            if (player.Life == 0)
            {
                cont.FinishGame(player.ID);
            }
            //if(player.Life > 31)
            //{
            //    cont.FinishGame(player.ID);
            //}

            if (player.LiftItem == true)//поднятие вещей
            {
                cont.LiftItem(player.ID);
            }
            
            if (player.Reload == true)//перезарядка
            {
                cont.Reload(player.ID,player);
            }
            if (field.Bullet.Count != 0)
            {
                cont.BulFlight();
            }
            if(interval.Seconds == 3)
            {
                cont.SmallCircle();
            }
            field.time = interval;
            return player;
        }
        public void Process()
        {
            NetworkStream stream = null;
          //  try
           // {
                stream = client.GetStream();
                byte[] data = new byte[256]; // буфер для получаемых данных
            StartTime = DateTime.Now;
            while (true)
                {

                now = DateTime.Now;
                interval = StartTime - now;
                interval = interval.Negate();
                //now = new DateTime(now.Year, now.Month, now.Day, now.Hour, Math.Abs( now.Minute - StartTime.Minute), Math.Abs(StartTime.Second - now.Second), Math.Abs(now.Millisecond - StartTime.Millisecond));
                Console.WriteLine(interval);





                // получаем сообщение
                StringBuilder builder = new StringBuilder();
                    int bytes = 0;

                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.UTF8.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);


                    string message = builder.ToString();

              //  Console.WriteLine("Клиент: " + message);

                Player jsonData1 = JsonConvert.DeserializeObject<Player>(message);
                
                    if (jsonData1.ID == -1 )
                    // отправляем обратно сообщение 
                    {
                        message = PlayerData1();
                    Console.WriteLine("СЕРВЕР: " + message);
                        data = Encoding.UTF8.GetBytes(message);
                        stream.Write(data, 0, data.Length);
                    }
                    else
                    {
                    
                        jsonData1 = count(jsonData1);
                        
                        var mess = JsonConvert.SerializeObject(field, Formatting.Indented);
                       Console.WriteLine("СЕРВЕР: " + mess);
                        data = Encoding.UTF8.GetBytes(mess);
                        stream.Write(data, 0, data.Length);
                        stream.Flush();
                    }
                    //}
                    //else { stream.Flush(); }
                }

            //}


            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            //finally
            //{
            //    if (stream != null)
            //        stream.Close();
            //    if (client != null)
            //        client.Close();
            //}

        }

        }

    }
    