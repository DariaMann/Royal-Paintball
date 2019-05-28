using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Server;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace ServerTest
{
    [TestClass]
    public class LogicTest
    {
        [TestMethod]
        public void MovePlayerTest_1()
        {
            Player player = new Player() { X = 0, Y = 0 };
            player.MovePlayer("W");
            float expected = 0.4f;
            Assert.AreEqual(expected, player.Y);
        }
        [TestMethod]
        public void MovePlayerTest_2()
        {
            Player player = new Player() { X = 0, Y = 0 };
            player.MovePlayer("S");
            float expected = 0.4f;
            Assert.AreNotEqual(expected, player.Y);
        }
        [TestMethod]
        public void ChangeWeaponTest()
        {
            Player player = new Player();
            player.ChangeWeapon("Gun");
            Weapons expected = player.G;
            Assert.AreEqual(expected, player.Weap);
        }
        //[TestMethod]
        //public void CreateBulletTest()
        //{
        //    Player player = new Player();
        //    player.Weap = player.G;
        //    Bullet bullet = new Bullet(0, 0, 0, 0, "Gun", 1, 0.1f, "black", player.Weap);
        //    Weapons expected = player.G;
        //    Assert.AreEqual(expected);
        //    Assert.AreSame(expected, bullet.weapon);
        //}
        [TestMethod]
        public void PlayerOutOfCircleTest_1()
        {
            Field f = new Field();
            Player player = new Player() { X = 70, Y = 70 };
            f.PlayerOutOfCircle(player);
            bool actual = player.OutCircle;
            bool expected = true;
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void PlayerOutOfCircleTest_2()
        {
            Field f = new Field();
            Player player = new Player() { X = 5, Y = 5 };
            f.PlayerOutOfCircle(player);
            bool actual = player.OutCircle;
            bool expected = false;
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void BulletInObjectTest_1()
        {
            Bullet bullet = new Bullet(0, 0, 1, 0, "Gun", 1, 0.1f, "black");
            Wall wall = new Wall(0, 0);
            bool actual = bullet.BulletInObject(wall);
            bool expected = true;
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void BulletInObjectTest_2()
        {
            Bullet bullet = new Bullet(0, 0, 5, 0, "Gun", 1, 0.1f, "black");
            Wall wall = new Wall(0, 0);
            bool actual = bullet.BulletInObject(wall);
            bool expected = false;
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void BulletInObjectTest_3()
        {
            Player player = new Player() { X = 0, Y = 0 };
            Bullet bullet = new Bullet(0, 0, 0, 0, "Gun", 1, 0.1f, "black");
            bool actual = bullet.BulletInObject(player);
            bool expected = true;
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void PlayerInFrontOfObjectTest_1()
        {
            Field f = new Field();
            Player player = new Player() { X = 0, Y = 0 };
            f.PlayerInFrontOfObject(player);
            Dictionary<string, string> expected = new Dictionary<string, string>();
            Assert.AreEqual(expected, player.StopIn);
        }
        [TestMethod]
        public void PlayerInFrontOfObjectTest_2()
        {
            Field f = new Field();
            Player player = new Player() { X = 53, Y = 0 };
            f.PlayerInFrontOfObject(player);
            Dictionary<string, string> expected = new Dictionary<string, string>();
            expected.Add("D", "D");
            //Assert.AreNotEqual(expected, player.StopIn);
            Assert.AreEqual(expected, player.StopIn);
        }
        [TestMethod]
        public void ReloadWeaponTest_1()
        {
            Player player = new Player();
            player.Weap.CountBullets = 0;
            player.Weap.CountMagazine = 0;
            player.Weap.CamShot = false;
            DateTime dateTime = DateTime.Now;
            player.Weap.time = dateTime;
            player.ReloadWeapon();
            int expected = 0;
            Assert.AreEqual(expected, player.Weap.CountBullets);
        }
        [TestMethod]
        public void ReloadWeaponTest_2()
        {
            Player player = new Player();
            player.Weap.CountBullets = 1;
            player.Weap.CountMagazine = 10;
            player.Weap.CamShot = false;
            DateTime dateTime = DateTime.Now;
            player.Weap.time = dateTime;
            player.ReloadWeapon();
            int expected = 11;
            Assert.AreEqual(expected, player.Weap.CountBullets);
        }
        [TestMethod]
        public void ReloadWeaponTest_3()
        {
            Player player = new Player();
            player.Weap.CountBullets = 0;
            player.Weap.CountMagazine = 12;
            player.Weap.CamShot = false;
            DateTime dateTime = DateTime.Now;
            player.Weap.time = dateTime;
            player.ReloadWeapon();
            int expected = 12;
            Assert.AreEqual(expected, player.Weap.CountBullets);
        }
        [TestMethod]
        public void ReloadWeaponTest_4()
        {
            Player player = new Player();
            player.Weap.CountBullets = 2;
            player.Weap.CountMagazine = 13;
            player.Weap.CamShot = false;
            DateTime dateTime = DateTime.Now;
            player.Weap.time = dateTime;
            player.ReloadWeapon();
            int expected = 12;
            Assert.AreEqual(expected, player.Weap.CountBullets);
        }
        [TestMethod]
        public void ReloadWeaponTest_5()
        {
            Player player = new Player();
            player.Weap.CountBullets = 0;
            player.Weap.CountMagazine = 14;
            player.Weap.CamShot = false;
            DateTime dateTime = DateTime.Now;
            player.Weap.time = dateTime;
            player.ReloadWeapon();
            int expected = 2;
            Assert.AreEqual(expected, player.Weap.CountMagazine);
        }
        [TestMethod]
        public void LiftItemInGameTest_1()
        {
            Player player = new Player() { X = 0, Y = 0 };
            Item item = new Item("Kit", 5, 0, 0, 0);
            bool actual = player.LiftItemInGame(item);
            bool expected = true;
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(55, player.Life);
        }
        [TestMethod]
        public void LiftItemInGameTest_2()
        {
            Player player = new Player() { X = 5, Y = 5 };
            Item item = new Item("Kit", 5, 0, 0, 0);
            bool actual = player.LiftItemInGame(item);
            bool expected = false;
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void LifeTest_1()
        {
            Player player = new Player() { ID = 0, X = 5, Y = 5, Life = 1 };
            Field f = new Field();
            f.Player.Add(0, player);
            f.time = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            f.inpulse = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            f.DecreaseInLives();
            int actual = f.Player[player.ID].Life;
            int expected = 0;
            Assert.AreEqual(f.time.Seconds, f.inpulse.Second - 1);
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void WoundTest_1()
        {
            Player player = new Player();
            player.Wound(5);
            int actual = player.Life;
            float expected = 45;
            Assert.AreEqual(expected, actual);
        }
    }

    [TestClass]
    public class CommunicationTests
    {
        [TestMethod]
        public void CloneTest_0()
        {
            Field f1 = new Field();
            var mess = JsonConvert.SerializeObject(f1, Formatting.Indented);
            f1.Player.Add(1,new Player());
            Field f2 = JsonConvert.DeserializeObject<Field>(mess);
            // Assert.AreNotSame(f2.Bullet, f1.Bullet);
            Assert.AreNotEqual(f2.Player.Count, f1.Player.Count);
        }
        [TestMethod]
        public void CloneTest_1()
        {
            Field f1 = new Field();
            Field f2 = (Field)f1.Clone();
           // Assert.AreNotSame(f2.Bullet, f1.Bullet);
            Assert.AreNotSame(f2.Tree[1], f1.Tree[10]);
        }
        [TestMethod]
        public void CloneTest_2()
        {
            Field f1 = new Field();
            f1.Player.Add(0, new Player() { Death = true });
            Field f2 = (Field)f1.Clone();
            Assert.AreEqual(f2.Player[0], f1.Player[0]);
        }
        [TestMethod]
        public void CloneTest_3()
        {
            Field f1 = new Field();
            Field f2 = (Field)f1.Clone();
            Assert.AreNotSame(f2.Wall, f1.Wall);
        }
        [TestMethod]
        public void CloneTest_4()
        {
            Field f1 = new Field();
            Field f2 = (Field)f1.Clone();
            Assert.AreNotSame(f2.circle, f1.circle);
        }
        [TestMethod]
        public void CloneTest_5()
        {
            Field f1 = new Field();
            Field f2 = (Field)f1.Clone();
            Assert.AreNotSame(f2.Colors, f1.Colors);
        }
        [TestMethod]
        public void CloneTest_6()
        {
            Field f1 = new Field();
            Field f2 = (Field)f1.Clone();
            Assert.AreNotSame(f2.Size, f1.Size);
        }
        [TestMethod]
        public void CloneTest_7()
        {
            Field f1 = new Field();
            Field f2 = (Field)f1.Clone();
            Assert.AreNotSame(f2.time, f1.time);
        }
        [TestMethod]
        public void CloneTest_8()
        {
            Field f1 = new Field();
            Field f2 = (Field)f1.Clone();
            Assert.AreNotSame(f2.inpulse, f1.inpulse);
        }
        [TestMethod]
        public void CloneTest_9()
        {
            Field f1 = new Field();
            Field f2 = (Field)f1.Clone();
            Assert.AreNotSame(f2.X, f1.X);
        }
        [TestMethod]
        public void CloneTest_10()
        {
            Field f1 = new Field();
            Field f2 = (Field)f1.Clone();
            Assert.AreNotSame(f2.Y, f1.Y);
        }
        [TestMethod]
        public void CloneTest_11()
        {
            Field f1 = new Field();
            Field f2 = (Field)f1.Clone();
            Assert.AreNotSame(f2, f1);
        }
        [TestMethod]
        public void CloneTest_12()
        {
            Field f1 = new Field();
            f1.Item.Add(20, new Item("a", 1, 1,1, 1));
            f1.Item.Add(21, new Item("a", 1, 1, 1, 1));
            Field f2 = (Field)f1.Clone();
            Assert.AreEqual(f2.Item[21].Name, f1.Item[21].Name);
        }
        [TestMethod]
        public void CloneTest_13()
        {
            Field f1 = new Field();
            Field f2 = (Field)f1.Clone();
            Assert.AreNotSame(f2.Tree, f1.Tree);
        }
        [TestMethod]
        public void CloneTest_14()
        {
            Field f1 = new Field();
            f1.Bullet.Add(new Bullet(1, 1, 1, 1, "a", 1, 1, "b"));
            Field f2 = (Field)f1.Clone();
            Assert.AreNotSame(f2.Bullet[0].a, f1.Bullet[0].a);
        }
        //[TestMethod]
        //public void SendTest_1()
        //{
        //    int port = 904; // порт для прослушивания подключений
        //    TcpListener server = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
        //    server.Start();
        //    TcpClient tcpClient = server.AcceptTcpClient();
        //    Client client = new Client(tcpClient);
        //    ConcurrentQueue<Client> queue = new ConcurrentQueue<Client>();
        //    queue.Enqueue(client);
        //    Sender2 sender = new Sender2(queue);
        //}
        [TestMethod]
        public void ConnectTest()
        {
            int port = 904; // порт для прослушивания подключений
            TcpListener server = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
            server.Start();
            TcpClient tcpClient = server.AcceptTcpClient();
        }
    }

}
