using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Server
{
    class Data
    {
        public List<Tree> Tree { get; set; }
        public List<Wall> Wall { get; set; }
        public List<Bullet> Bullet { get; set; }
        public List<Item> Item { get; set; }
        public int ID { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float XRot { get; set; }
        public float YRot { get; set; }
        public int Life { get; set; }
        public string Direct { get; set; }
        public bool Shoot { get; set; }
        public string Weapon { get; set; }
        public bool Reload { get; set; }
        public int BulP { get; set; }
        public int BulS { get; set; }
        public int BulG { get; set; }
        public int BulB { get; set; }
        public int MagP { get; set; }
        public int MagS { get; set; }
        public int MagG { get; set; }
        public int MagB { get; set; }
    }
    public class ClientObject//Producer
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
        public string PlayerData1()//создание игрока 
        {
         //   First();//генерация ID игрока
            Position pos = new Position();

            Data data = new Data();
            data.Tree = new List<Tree>();
            data.Tree.Add(
                new Tree
                {
                    X = 8,
                    Y = 0
                });
            data.Tree.Add(
                 new Tree
                 {
                     X = 7,
                     Y = 9
                 });
            data.Wall = new List<Wall>();
            data.Wall.Add(
                new Wall(1,2)
                {
                    X = 22,
                    Y = 1
                });
            data.Bullet = new List<Bullet>();
            data.Item = new List<Item>();


            data.ID = Convert.ToInt32(ID);
            data.X = pos.X;
            data.Y = pos.Y;
            data.XRot = -90;
            data.YRot = 0;
            Player pl = new Player(data.ID, data.X, data.Y, data.XRot);
            data.Life = pl.Lifes;
            data.Direct = pl.Direction;
            data.Shoot = false;
            data.Weapon = pl.Weapon;
            data.BulP = pl.P.CountBullets;
            data.BulS = pl.S.CountBullets;
            data.BulG = pl.G.CountBullets;
            data.BulB = pl.B.CountBullets;
            data.MagP = pl.P.CountMagazine;
            data.MagS = pl.S.CountMagazine;
            data.MagG = pl.G.CountMagazine;
            data.MagB = pl.B.CountMagazine;

            // сериализуем объект
            // при помощи Formatting.Indented указываем, что хотим переносить
            // каждую сущность на новую строку
            
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            return json;
        }
            public void First()
        {
            Random rn = new Random(); // объявление переменной для генерации чисел
            string id = Convert.ToString(rn.Next(0, 1000));
            ID = id;
            Console.WriteLine(PlayerData1());
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

            Dictionary<string, string> walls = new Dictionary<string, string>();
            Dictionary<string, string> bullets = new Dictionary<string, string>();
            Dictionary<string, string> trees = new Dictionary<string, string>();

            string x = Convert.ToString(pos.X);//x игрока
            string y = Convert.ToString(pos.Y);//у игрока
                                               // string z = Convert.ToString(pos.Z);//z игрока
                                               //  string xW = Convert.ToString(Convert.ToSingle(x) + 0.7);
            string xRot = "-90";//вращение игрока по х
            string id = ID;
            Player pl = new Player(Convert.ToInt32(id), Convert.ToSingle(pos.X), Convert.ToSingle(pos.Y), Convert.ToSingle(xRot));

            string life = Convert.ToString(pl.Lifes);
            string dir = pl.Direction;//направление, куда движется игрок
            string shoot = pl.Shoot;
            string weapon = pl.Weapon;
            string wound = "F";
            string countBulP = Convert.ToString(pl.P.CountBullets);//количество пуль пистолета
            string countBulS = Convert.ToString(pl.S.CountBullets);//количество пуль дробовика
            string countBulG = Convert.ToString(pl.G.CountBullets);//количество пуль автомата
            string countBulB = Convert.ToString(pl.B.CountBullets);//количество бомб

            string magazineP = Convert.ToString(pl.P.CountMagazine);//количество пуль пистолета
            string magazineS = Convert.ToString(pl.S.CountMagazine);//количество пуль дробовика
            string magazineG = Convert.ToString(pl.G.CountMagazine);//количество пуль автомата
            string magazineB = Convert.ToString(pl.B.CountMagazine);//количество бомб

            string liftItem = "F";
            string reload = "F";
            string countMag = "0";

            string startPosX = "N";
            string startPosY = "N";
            string startPosZ = "N";

            string endPosX = "N";
            string endPosY = "N";
            string endPosZ = "N";

            string timer = "0";//Convert.ToString(f.timer);

            if (!dasha.ContainsKey("walls"))
            {
               
                walls.Add("1x", Convert.ToString(f.Walls[1].X)); walls.Add("1y", Convert.ToString(f.Walls[1].Y));
                dasha.Add("walls", walls);
            }
            if (!dasha.ContainsKey("trees"))
            {

                trees.Add("1x", "46"); trees.Add("1y", "-22");
                trees.Add("2x", "26"); trees.Add("2y", "-16");
                trees.Add("3x", "23"); trees.Add("3y", "-21");
                trees.Add("4x", "14"); trees.Add("4y", "-10");
                trees.Add("5x", "-12"); trees.Add("5y", "36");
                trees.Add("6x", "-33"); trees.Add("6y", "-17");
                trees.Add("7x", "-26"); trees.Add("7y", "-43");
                trees.Add("8x", "29"); trees.Add("8y", "-36");
                trees.Add("9x", "47"); trees.Add("9y", "40");
                trees.Add("10x", "25"); trees.Add("10y", "-36");
                trees.Add("11x", "9"); trees.Add("11y", "41");
                trees.Add("12x", "-19"); trees.Add("12y", "-6");
                trees.Add("13x", "22"); trees.Add("13y", "39");
                trees.Add("14x", "17"); trees.Add("14y", "11");
                trees.Add("15x", "-2"); trees.Add("15y", "17");
                trees.Add("16x", "-23"); trees.Add("16y", "20");
                trees.Add("17x", "31"); trees.Add("17y", "6");
                trees.Add("18x", "-38"); trees.Add("18y", "-12");
                trees.Add("19x", "35"); trees.Add("19y", "-6");
                trees.Add("20x", "-13"); trees.Add("20y", "16");
                trees.Add("21x", "-37"); trees.Add("21y", "-40");
                trees.Add("22x", "-19"); trees.Add("22y", "28");
                trees.Add("23x", "17"); trees.Add("23y", "27");
                trees.Add("24x", "-27"); trees.Add("24y", "32");
                trees.Add("25x", "-26"); trees.Add("25y", "23");
                trees.Add("26x", "5"); trees.Add("26y", "-27");
                trees.Add("27x", "4"); trees.Add("27y", "-37");
                trees.Add("28x", "36"); trees.Add("28y", "-41");
                trees.Add("29x", "4"); trees.Add("29y", "-8");
                trees.Add("30x", "-7"); trees.Add("30y", "-22");
                trees.Add("31x", "13"); trees.Add("31y", "21");
                trees.Add("32x", "7"); trees.Add("32y", "40");
                trees.Add("33x", "38"); trees.Add("33y", "42");
                trees.Add("34x", "-35"); trees.Add("34y", "2");
                trees.Add("35x", "-30"); trees.Add("35y", "-38");
                trees.Add("36x", "20"); trees.Add("36y", "-4");
                trees.Add("37x", "26"); trees.Add("37y", "-22");
                trees.Add("38x", "-20"); trees.Add("38y", "10");
                trees.Add("39x", "-6"); trees.Add("39y", "2");
                trees.Add("40x", "-7"); trees.Add("40y", "0");
                trees.Add("41x", "12"); trees.Add("41y", "2");
                dasha.Add("trees", trees);
            }
            if (!dasha.ContainsKey("bullets"))
                dasha.Add("bullets", bullets);

            player1.Add("id", id);//
            player1.Add("pos_x", x); player1.Add("pos_y", y); //позиция игрока
            player1.Add("rot_x", xRot); player1.Add("rot_y", "0"); player1.Add("rot_z", "0");//вращение игрока
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
        public Dictionary<string, Dictionary<string, string>> count(Dictionary<string, string> dic)//метод реакции сервера на сообщения клиента
        {
            cont = new GameController(f);
            ID = dic["id"];
            dasha[ID] = dic;


            if (!(dic["dir"] == "N"))//движение игрока
            {
                cont.MovePlayer(ID,dasha[ID]["dir"]);
            }
            if (dic["shoot"] == "T")//выстрел
            {
                dasha = cont.Shoott(dasha, ID);
            }

            if (dic["wound"] == "T")//ранение
            {
                cont.Woundd(ID);
            }

            if (Convert.ToInt32(dasha[ID]["life"]) == 0)
            {
                dasha = cont.FinishGame(ID, dasha);
            }

            if (dic["liftItem"] == "T")//поднятие вещей
            {
                dasha = cont.LiftItem(ID, dasha);
            }

            cont.ChangeWeapon(ID, dasha[ID]["weapon"]);

            if (dic["reload"] == "T")//перезарядка
            {
                dasha = cont.Reload(ID, dasha);
            }
            dasha = cont.DelBul(dasha, ID);
            dasha = cont.PlayerData(ID, dasha);
            return dasha;
        }
        public void Process()
        {
            NetworkStream stream = null;
          //  try
           // {
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

                    var jsonData1 = JsonConvert.DeserializeObject<Dictionary<string, string>>(message);
                    //if (jsonData1 == null)
                    //{
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

                        Dictionary<string, Dictionary<string, string>> serverMess = count(jsonData1);

                    Console.WriteLine("_________________________________________________________________________________________________________");
                    Console.WriteLine(serverMess);
                    Console.WriteLine("_________________________________________________________________________________________________________");


                        var mess = JsonConvert.SerializeObject(serverMess);
                        Console.WriteLine("Сервер: " + mess);
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
    