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
      //  static public string list = [];

            static public string First()
        {
            //Position pos = new Position();
            //Dictionary<string, string> player1 = new Dictionary<string, string>();
            //string x = Convert.ToString(pos.X);//x игрока
            //string y = Convert.ToString(pos.Y);//у игрока
            //string z = Convert.ToString(pos.Z);//z игрока
            Player pl = new Player(Convert.ToDouble(pos.X), Convert.ToDouble(pos.Y), Convert.ToDouble(pos.Z), -90);
            string id = Convert.ToString(pl.ID);
            player1.Add("id", id); player1.Add("pos_x", x); player1.Add("pos_y", y); player1.Add("pos_z", z);
           
            string serialized = JsonConvert.SerializeObject(dasha);
            return serialized;
        }
         static public string PosPlayer()
        {
            Position pos = new Position();
            //Player pl = new Player(Convert.ToDouble(pos.X), Convert.ToDouble(pos.Y), Convert.ToDouble(pos.Z), -90);
            //Data myCollection = new Data();
            //myCollection.Asd = new ASD();

            //myCollection.Asd= new ASD()
            //{
            //    X = Convert.ToString(pos.X),
            //    Y = Convert.ToString(pos.Y),
            //    Z = Convert.ToString(pos.Z),
            //    Xw = Convert.ToString(pos.X + 0.75),
            //    Yw = Convert.ToString(pos.Y),
            //    Zw = Convert.ToString(pos.Z),
            //    Xr = "-90",
            //    Yr = "0",
            //    Zr = "0",
            //    IP = Convert.ToString(pl.ID),
            //    Life = "30"
            //};

            //string serialized = JsonConvert.SerializeObject(myCollection);
            //return serialized;
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
            string id = Convert.ToString(pl.ID);
            string life = Convert.ToString(pl.Lifes);
            player1.Add("id", id); player1.Add("pos_x", x); player1.Add("pos_y", y); player1.Add("pos_z", z); player1.Add("rot_x", xRot);
            player1.Add("rot_y", "0"); player1.Add("rot_z", "0"); player1.Add("posW_x", xW); player1.Add("posW_y", y); player1.Add("posW_z", z);
            player1.Add("life", life);
          //  string value = x + "," + y + "," + z + "," + xRot + "," + life;//передаваемое сообщение
            dasha.Add(id, player1);
            string serialized = JsonConvert.SerializeObject(dasha);
            return serialized;

            //double x = Convert.ToDouble(pos.X);//x игрока
            //double y = Convert.ToDouble(pos.Y);//у игрока
            //double z = Constant.z;//z игрока
            //double xW = x + 0.75;//х оружия
            //double yW = y;//у оружия
            //double xRot = -90;
            //Player pl = new Player(x, y, z, xRot);
            //int id = pl.ID;
            //int life = pl.Lifes;
            //string position = id + "," + x + "," + y + "," + z + "," + xRot + "," + life;//передаваемое сообщение
            //return position;

        }
        public string Weapon(Weapons w)
        {
            return Convert.ToString(w.Index);
        }

        const int port = 904; // порт для прослушивания подключений
      public static void count()
        {
            Field fds = new Field();
            GameController cont = new GameController(fds);
            while (true)
            {
               
                // string a = "{ "action": "new_person", "weapon_list": [,,,,{{}},] ... }"

                //if (dasha["action"] == "new_person")
                //{
                //    fds.add_player(new Player(dasha["id"], dasha["pos"]));
                //    Player r = new Player(dasha["id"], dasha["pos"]);

                //    answer = "";
                //}

                //if (dasha["action"] == "move")
                //{
                //    cont.MovePlayer(dasha["id"], dasha["pos"], dasha["side"]);

                //   if ( res == false)
                //        answer = dasha;
                //    else 
                //        answer = dasha + 1;
                //}
                //if(dasha!="")
                Console.WriteLine(dasha);
                Thread.Sleep(300);
            }
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
            // CONVERSATION
            while (true)
            {
                Console.WriteLine("Ожидание подключений... ");

                Thread myT3 = new Thread(count);
                myT3.Start();
                // получаем входящее подключение
                TcpClient client = server.AcceptTcpClient();


                NetworkStream stream = client.GetStream();

                // СООБЩЕНИЕ ДЛЯ ОТПРАВКИ КЛИЕНТУ
                string response1; response1 = First();
                //    var jsonData = JsonConvert.SerializeObject(response);
                // Console.WriteLine(jsonData);
                // преобразуем сообщение в массив байтов
                byte[] data2 = Encoding.UTF8.GetBytes(response1);

                // отправка сообщения
                stream.Write(data2, 0, data2.Length);
                Console.WriteLine("Отправлено сообщение: {0}", response1);

                Console.WriteLine("Подключен клиент. Выполнение запроса...");
                Thread t = new Thread(() => {
                
                data1 = null;

                // получаем сетевой поток для чтения и записи
                

                int i;

                //// СООБЩЕНИЕ ДЛЯ ОТПРАВКИ КЛИЕНТУ
                //string response; response = PosPlayer();
                ////    var jsonData = JsonConvert.SerializeObject(response);
                //   // Console.WriteLine(jsonData);
                //// преобразуем сообщение в массив байтов
                //byte[] data = Encoding.UTF8.GetBytes(response);

                //// отправка сообщения
                //stream.Write(data, 0, data.Length);
                //Console.WriteLine("Отправлено сообщение: {0}", response );

                //Цикл для получения всех данных, отправленных клиентом.
               try {
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {


                        //ПОЛУЧЕНИЕ СООБЩЕНИЯ ОТ КЛИЕНТА
                        //Преобразуйте байты данных в строку ASCII.
                        data1 = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                       // Console.WriteLine("Сообщение: {0}", data1);
                       // dasha = data1;

                        // Обработка данных, отправленных клиентом.
                        data1 = data1.ToUpper();

                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(data1);

                        // Отправить ответ.
                        stream.Write(msg, 0, msg.Length);
                     //   Console.WriteLine("Ответ: {0}", data1);


                    }
                    // закрываем поток
                    stream.Close();

                }
                catch (Exception e)
                {
                       
                }
            });
                t.Start();
               
            }
        }
        
    }
}