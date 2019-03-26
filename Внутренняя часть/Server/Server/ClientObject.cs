﻿using System;
using System.Net.Sockets;
using System.Text;

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Server
{
    public class ClientObject
    {
        static public Dictionary<string, Dictionary<string, string>> dasha = new Dictionary<string, Dictionary<string, string>>();
        static public string ID = "555";
        public TcpClient client;
        public Field f;
        public GameController cont;

        public ClientObject(TcpClient tcpClient)
        {
            client = tcpClient;
        }
        static public void First()
        {
            Random rn = new Random(); // объявление переменной для генерации чисел
            string id = Convert.ToString(rn.Next(0, 1000));
            ID = id;
        }
         public string PlayerData()
        {
            // string serialized = JsonConvert.SerializeObject(First());
            First();
            Position pos = new Position();
            Bomb b = new Bomb();
            Shotgun s = new Shotgun();
            Gun g = new Gun();
            Pistol p = new Pistol();
            Dictionary<string, string> player1 = new Dictionary<string, string>();
            string x = Convert.ToString(pos.X);//x игрока
            string y = Convert.ToString(pos.Y);//у игрока
            string z = Convert.ToString(pos.Z);//z игрока
            string xW = Convert.ToString(Convert.ToSingle(x) + 0.7);
            string xRot = "-90";//вращение игрока по х
            Player pl = new Player(Convert.ToDouble(pos.X), Convert.ToDouble(pos.Y), Convert.ToDouble(pos.Z), Convert.ToDouble(xRot));
            string id = ID;
            string life = Convert.ToString(pl.Lifes);
            string dir = pl.Direction;//направление, куда движется игрок
            string shoot = pl.Shoot;
            string weapon = pl.Weapon;
            string wound = "false";
            string countBulP = Convert.ToString(p.CountBullets);//количество пуль пистолета
            string countBulS = Convert.ToString(s.CountBullets);//количество пуль дробовика
            string countBulG = Convert.ToString(g.CountBullets);//количество пуль автомата
            string countBulB = Convert.ToString(b.CountBullets);//количество бомб
            player1.Add("id", id);//
            player1.Add("pos_x", x); player1.Add("pos_y", y); player1.Add("pos_z", z);//позиция игрока
            player1.Add("rot_x", xRot); player1.Add("rot_y", "0"); player1.Add("rot_z", "0");//вращение игрока
            player1.Add("pos_xW", xW); player1.Add("pos_yW", y); player1.Add("pos_zW", z);
            player1.Add("life", life); player1.Add("dir", dir); player1.Add("shoot", shoot); player1.Add("weapon", weapon);//жизни,направление движения,стрельба,выбранное оружие игрока
            player1.Add("wound", wound);
            player1.Add("bulP", countBulP); player1.Add("bulS", countBulS); player1.Add("bulG", countBulG); player1.Add("bulB", countBulB);//количество пуль оружий игрока
            if (!dasha.ContainsKey(id))
                dasha.Add(id, player1);
            f = new Field();
            f.Players.Add(id, player1);
            string serialized = JsonConvert.SerializeObject(dasha);
            return serialized;
        }
        public Dictionary<string, Dictionary<string, string>> count(Dictionary<string,string> dic)
        {
             f = new Field();
             cont = new GameController(f);
            ID = dic["id"];
            dasha[ID] = dic;
            if (!(dic["dir"]=="N"))//движение игрока
            {
               
                dasha = cont.MovePlayer(ID, dasha);
            }
            if (Convert.ToInt32(dic["life"]) <= 0)
            {
                cont.FinishGame(ID);
            }
            if (dic["shoot"] == "T")
            {
                dasha = cont.Shoot(dasha, ID);
            }
            switch(dic["weapon"])
            {
                case "Pistol":
                    {
                        break;
                    }
                case "Shotgun":
                    {
                        break;
                    }
                case "Gun":
                    {
                        break;
                    }
                case "Bomb":
                    {
                        break;
                    }
            }
            if (dic["wound"]=="true")//ранение
            {

            }
            return dasha;
        }
        public void Process()
        {
            NetworkStream stream = null;
            try
            {
                stream = client.GetStream();
                byte[] data = new byte[256]; // буфер для получаемых данных
                while (true)
                {
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

                    Console.WriteLine("Клиент: " + message);
                
                    var jsonData1 = JsonConvert.DeserializeObject<Dictionary<string,string>>(message);
                    
                    if (jsonData1["id"] == "-1")
                    // отправляем обратно сообщение 
                    {
                        message = PlayerData();
                        Console.WriteLine("Сервер: " + message);
                        data = Encoding.UTF8.GetBytes(message);
                        stream.Write(data, 0, data.Length);
                    }
                    else
                    {
                        
                        var mess = JsonConvert.SerializeObject(count(jsonData1));
                        Console.WriteLine("Сервер: " + mess);
                        data = Encoding.UTF8.GetBytes(mess);
                        stream.Write(data, 0, data.Length);
                    }
                }
        }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (stream != null)
                    stream.Close();
                if (client != null)
                    client.Close();
            }
        }
    }
}