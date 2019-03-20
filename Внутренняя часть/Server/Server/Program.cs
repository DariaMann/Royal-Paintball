using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
//using System.Runtime.Serialize;

namespace Server//TcpListenerApp
{
    class Program
    {
      //  static public string dasha = "";
      // static public Dictionary<string, string> dashas = new Dictionary<string, string>();
        static public Dictionary<string, Dictionary<string, string>> dasha = new Dictionary<string, Dictionary<string, string>>();
        static public string answer = "";
        static public string ID = "";
      

        static public string First()
        {
            Random rn = new Random(); // объявление переменной для генерации чисел
            string id = Convert.ToString(rn.Next(0,1000));
            ID = id;

            string serialized = JsonConvert.SerializeObject(id);
            return serialized;
            //if (!dasha.ContainsKey("333"))
            //{
            //    Random rn = new Random(); // объявление переменной для генерации чисел
            //    string id = "333";// Convert.ToString(rn.Next(333));
            //    ID = id;

            //    string serialized = JsonConvert.SerializeObject(id);
            //    return serialized;
            //}
            //else
            //{
            //    string id = "334";// Convert.ToString(rn.Next(333));
            //    ID = id;
            //    string serialized = JsonConvert.SerializeObject(id);
            //    return serialized;
            //}
        }
         static public string PlayerData(string Id)
        {
            Position pos = new Position();
            Bomb b = new Bomb();
            Shotgun s = new Shotgun();
            Gun g = new Gun();
            Pistol p = new Pistol();
            Dictionary<string, string> player1 = new Dictionary<string, string>();
            string x = Convert.ToString(pos.X);//x игрока
            string y = Convert.ToString(pos.Y);//у игрока
            string z = Convert.ToString(pos.Z);//z игрока
            string xW = Convert.ToString(Convert.ToDouble(x) + 0.75);//х оружия
            string yW = y;//у оружия
            string xRot = "-90";
            double xx = Convert.ToDouble(pos.X);
            double yy = Convert.ToDouble(pos.Y);
            double zz = Convert.ToDouble(pos.Z);
            double xxrot = Convert.ToDouble(xRot);
            Player pl = new Player(Convert.ToDouble(pos.X), Convert.ToDouble(pos.Y), Convert.ToDouble(pos.Z), Convert.ToDouble(xRot) );
            string id = Id;
            string life = Convert.ToString(pl.Lifes);
            string dir = pl.Direction;
            string shoot = pl.Shoot;
            string weapon = pl.Weapon;
            string countBulP =Convert.ToString(p.CountBullets);
            string countBulS = Convert.ToString(s.CountBullets);
            string countBulG = Convert.ToString(g.CountBullets);
            string countBulB = Convert.ToString(b.CountBullets);
            player1.Add("id", id);//
            player1.Add("pos_x", x); player1.Add("pos_y", y); player1.Add("pos_z", z);//позиция игрока
            player1.Add("rot_x", xRot); player1.Add("rot_y", "0"); player1.Add("rot_z", "0");//вращение игрока
            player1.Add("posW_x", xW); player1.Add("posW_y", y); player1.Add("posW_z", z);//позиция оружия игрока
            player1.Add("life", life); player1.Add("dir", dir); player1.Add("shoot", shoot); player1.Add("weapon", weapon);//жизни,направление движения,стрельба,выбранное оружие игрока
            player1.Add("bulP", countBulP); player1.Add("bulS", countBulS); player1.Add("bulG", countBulG); player1.Add("bulB", countBulB);//количество пуль оружий игрока
            if (!dasha.ContainsKey(id))
            dasha.Add(id, player1);
            string serialized = JsonConvert.SerializeObject(dasha);
            return serialized; 
        }
        const int port = 904; // порт для прослушивания подключений
      public static void count()
        {
            Field fds = new Field();
            GameController cont = new GameController(fds);
            while (true)
            {
                if (dasha.Count != 0)
                {
                  
                    if (dasha[ID]["dir"] != "N")
                    {
                        cont.MovePlayer(Convert.ToInt32(ID), dasha[ID]["dir"]);
                    }
                    if (Convert.ToInt32(dasha[ID]["life"]) <= Convert.ToInt32("0"))
                    {
                        cont.FinishGame(Convert.ToInt32(ID));
                    }
                    if (dasha[ID]["shoot"] == "T")
                    {
                        cont.Shoot(dasha[ID]["weapon"], Convert.ToInt32(ID));
                    }
                }


                // Console.WriteLine(dasha);
                // Console.WriteLine(PosPlayer(ID)); 
                Thread.Sleep(300);
            }
        }
        public static void SendMessage(string message, NetworkStream stream)// СООБЩЕНИЕ ДЛЯ ОТПРАВКИ КЛИЕНТУ
        {
            string response; response = message;
            
            // преобразуем сообщение в массив байтов
            byte[] data2 = Encoding.UTF8.GetBytes(response);
            stream.Write(data2, 0, data2.Length);
            Console.WriteLine("Отправлено сообщение: {0}", response);
        }
        public static void ResievedMessage(NetworkStream stream, String data1,int i, Byte[] bytes)//ПОЛУЧЕНИЕ СООБЩЕНИЯ ОТ КЛИЕНТА
        {
            //Преобразуйте байты данных в строку ASCII.
            data1 = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
            string message = JsonConvert.DeserializeObject<string>(data1);
           
            Console.WriteLine("Сообщение: {0}", data1);
            //dasha = data1;

            // Обработка данных, отправленных клиентом.
            data1 = data1.ToUpper();

            byte[] msg = System.Text.Encoding.ASCII.GetBytes(data1);

            // Отправить ответ.
            stream.Write(msg, 0, msg.Length);
            Console.WriteLine("Ответ: {0}", data1);
        }
        static void Main(string[] args)
        {
            TcpListener server = null;
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            server = new TcpListener(localAddr, port);

                // запуск слушателя
                server.Start();
                // Буфер для чтения данных
                Byte[] bytes = new Byte[256];
                String data1 = null;
            // диалог сервера с клиентами
            while (true)
            {
                Console.WriteLine("Ожидание подключений... ");
                
                Thread myT3 = new Thread(count);
                myT3.Start();
                // получаем входящее подключение
                TcpClient client = server.AcceptTcpClient();
                
                Console.WriteLine("Подключен клиент. Выполнение запроса...");

               
                
                // отправка сообщения
                //First();//метод для получение id клиента
               
                Thread t = new Thread(() => {//поток клиента
                
                data1 = null;

                // получаем сетевой поток для чтения и записи
                 NetworkStream stream = client.GetStream();
                    SendMessage(First(),stream);
                int i;
                    SendMessage(PlayerData(ID), stream);
                  
                    //Цикл для получения всех данных, отправленных клиентом.
                    try
                    {
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                            ResievedMessage(stream, data1, i, bytes);
                    }
                    // закрываем поток
                    stream.Close();

                }
                catch (Exception e)
                {
                        Console.WriteLine(e);
                }
            });
                t.Start();
               
            }
        }
        
    }
}