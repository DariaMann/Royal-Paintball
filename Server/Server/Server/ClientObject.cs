using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Server
{
    public class ClientObject
    {
        static public Dictionary<string, Dictionary<string, string>> dasha = new Dictionary<string, Dictionary<string, string>>();
        static public string ID = "555";
        public TcpClient client;
        public Field f;//поле игры
        public GameController cont;
      
        public ClientObject(TcpClient tcpClient, Field field)
        {
            this.f = field;
            this.client = tcpClient;
        }
         public void First()
        {
            Random rn = new Random(); // объявление переменной для генерации чисел
            string id = Convert.ToString(rn.Next(0, 1000));
            ID = id;

           int countWall = rn.Next(4, 8);
            for(int i = 0;i<countWall;i++)
            {
                float x = rn.Next(-1, 5);
                float y = rn.Next(-1, 5);
                Wall w = new Wall(x, y, countWall);
                //f.Walls.Add(countWall,w);
            }
        }
         public string PlayerData()//создание игрока 
        {
            First();//генерация ID игрока
            Position pos = new Position();
            Bomb b = new Bomb();
            Shotgun s = new Shotgun();
            Gun g = new Gun();
            Pistol p = new Pistol();
            Dictionary<string, string> player1 = new Dictionary<string, string>();
            string x = Convert.ToString(pos.X);//x игрока
            string y = Convert.ToString(pos.Y);//у игрока
           // string z = Convert.ToString(pos.Z);//z игрока
          //  string xW = Convert.ToString(Convert.ToSingle(x) + 0.7);
            string xRot = "-90";//вращение игрока по х
            string id = ID;
            Player pl = new Player(Convert.ToInt32(id),Convert.ToSingle(pos.X), Convert.ToSingle(pos.Y), Convert.ToSingle(xRot),f.SelectedWeapons);
            
            string life = Convert.ToString(pl.Lifes);
            string dir = pl.Direction;//направление, куда движется игрок
            string shoot = pl.Shoot;
            string weapon = pl.Weapon;
            string wound = "F";
            string countBulP = Convert.ToString(f.P.CountBullets);//количество пуль пистолета
            string countBulS = Convert.ToString(f.S.CountBullets);//количество пуль дробовика
            string countBulG = Convert.ToString(f.G.CountBullets);//количество пуль автомата
            string countBulB = Convert.ToString(f.B.CountBullets);//количество бомб

            string magazineP = Convert.ToString(f.P.CountMagazine);//количество пуль пистолета
            string magazineS = Convert.ToString(f.S.CountMagazine);//количество пуль дробовика
            string magazineG = Convert.ToString(f.G.CountMagazine);//количество пуль автомата
            string magazineB = Convert.ToString(f.B.CountMagazine);//количество бомб

            string liftItem = "F";
            string reload = "F";
            string countMag = "0";

            string startPosX = "N";
            string startPosY = "N";
            string startPosZ = "N";

            string endPosX = "N";
            string endPosY = "N";
            string endPosZ = "N";

            string timer = Convert.ToString(f.timer);

            player1.Add("id", id);//
            player1.Add("pos_x", x); player1.Add("pos_y", y); //позиция игрока
            player1.Add("rot_x", xRot); player1.Add("rot_y", "0"); player1.Add("rot_z", "0");//вращение игрока
        //    player1.Add("pos_xW", xW); player1.Add("pos_yW", y); //player1.Add("pos_zW", z);
            player1.Add("life", life); player1.Add("dir", dir); player1.Add("shoot", shoot); player1.Add("weapon", weapon);//жизни,направление движения,стрельба,выбранное оружие игрока
            player1.Add("wound", wound);
            player1.Add("bulP", countBulP); player1.Add("bulS", countBulS); player1.Add("bulG", countBulG); player1.Add("bulB", countBulB);//количество пуль оружий игрока
            player1.Add("magazineP", magazineP); player1.Add("magazineS", magazineS); player1.Add("magazineG", magazineG); player1.Add("magazineB", magazineB);//магазины
            player1.Add("liftItem", liftItem); player1.Add("reload", reload); player1.Add("countMag", countMag); //поднятие вещей, перезарядка оружия,количество выпавших магазинов
            player1.Add("startX", startPosX); player1.Add("startY", startPosY); player1.Add("startZ", startPosZ);
            player1.Add("endX", endPosX); player1.Add("endY", endPosY); player1.Add("endZ", endPosZ);
            player1.Add("timer", timer);
            if (!dasha.ContainsKey(id))
                dasha.Add(id, player1);
           
            f.Players.Add(id, pl);
            string serialized = JsonConvert.SerializeObject(dasha);
            return serialized;
        }
        public Dictionary<string, Dictionary<string, string>> count(Dictionary<string,string> dic)//метод реакции сервера на сообщения клиента
        {
             cont = new GameController(f);
            ID = dic["id"];
            dasha[ID] = dic;
            if (!(dic["dir"]=="N"))//движение игрока
            {
                dasha = cont.MovePlayerr(ID, dasha,f);
            }
            if (dic["shoot"] == "T")//выстрел
            {
                dasha = cont.Shoott(dasha, ID,f);
            }
           
            if (dic["wound"]=="T")//ранение/смерть
            {
                dasha = cont.Woundd(ID, dasha,f);
            }
            if (dic["liftItem"] == "T")//поднятие вещей
            {
                dasha = cont.LiftItem(ID, dasha);
            }
            dasha = cont.ChangeWeapon(ID, dasha, f);
            if (dic["reload"] == "T")//перезарядка
            {
                dasha = cont.Reload(ID, dasha,f);
            }
           
           
            return dasha;
        }
        public void Process()
        {
            NetworkStream stream = null;
            //try
            //{
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
     //   }
    }
}