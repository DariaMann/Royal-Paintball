using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Concurrent;
using Server;
using GameLibrary;
using System.Net.Sockets;
using System.Collections.Generic;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void MovePlayerTest_1()
        {
            Player player = new Player() {X = 0,Y=0};
            player.MovePlayer("W");
            float expected = 0.4f;
            Assert.AreEqual(expected,player.Y);
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
        [TestMethod]
        public void CreateBulletTest()
        {
            Player player = new Player();
            player.Weap = player.G;
            Bullet bullet = new Bullet(0, 0, 0, 0, "Gun", 1, 0.1f, "black", player.Weap);
            Weapons expected = player.G;
            Assert.AreEqual(expected, bullet.weapon);
            Assert.AreSame(expected, bullet.weapon);
        }
        [TestMethod]
        public void PlayerOutOfCircleTest_1()
        {
            Field f = new Field();
            Player player = new Player() { X=70,Y=70 };
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
            Player player = new Player() { X = 5, Y = 5 };
            Bullet bullet = new Bullet(0, 0, 1, 0, "Gun", 1, 0.1f, "black", player.Weap);
            Wall wall = new Wall(0,0);
            bool actual =  bullet.BulletInObject(wall);
            bool expected = true;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void BulletInObjectTest_2()
        {
            Player player = new Player() { X = 5, Y = 5 };
            Bullet bullet = new Bullet(0,0, 5, 0, "Gun", 1, 0.1f, "black", player.Weap);
            Wall wall = new Wall(0, 0);
            bool actual = bullet.BulletInObject(wall);
            bool expected = false;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void BulletInObjectTest_3()
        {
            Player player = new Player() { X = 0, Y = 0 };
            Bullet bullet = new Bullet(0, 0, 0, 0, "Gun", 1, 0.1f, "black", player.Weap);
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
            Player player = new Player() { ID=0,X = 5, Y = 5, Life = 1};
            Field f = new Field();
            f.Player.Add(0, player);
            f.time = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            f.inpulse = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            f.DecreaseInLives();
            int actual = f.Player[player.ID].Life;
            int expected = 0;
            Assert.AreEqual(f.time.Seconds, f.inpulse.Second-1);
            Assert.AreEqual(expected, actual);
        }
    }
}
