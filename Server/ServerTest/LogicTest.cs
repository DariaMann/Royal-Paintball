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
        public void LiftItemInGameTest_2()
        {
            Player player = new Player() { X = 5, Y = 5 };
            Item item = new Item("Kit", 5, 0, 0, 0);
            bool actual = player.LiftItemInGame(item);
            bool expected = false;
            Assert.AreEqual(expected, actual);
        }
    }

    [TestClass]
    public class CommunicationTests
    {
        [TestMethod]
        public void ConnectTest()
        {
            int port = 904; // порт для прослушивания подключений
            TcpListener server = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
            server.Start();
            TcpClient tcpClient = server.AcceptTcpClient();
        }
        [TestMethod]
        public void ConnectTest_1()
        {
            int port = 904; // порт для прослушивания подключений
            TcpListener server = new TcpListener(IPAddress.Parse("192.168.31.163"), port);
            server.Start();
            TcpClient tcpClient = server.AcceptTcpClient();
        }
    }

}
