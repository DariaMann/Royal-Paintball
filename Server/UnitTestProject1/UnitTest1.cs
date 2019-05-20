using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Concurrent;
using Server;
using GameLibrary;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void WeapWoundTest()
        {
            string weapon = "Gun";
            int exeption = 3;
            Field f = new Field();
            ConcurrentQueue<Player> queue = new ConcurrentQueue<Player>();
            ConcurrentQueue<string> dataForSend = new ConcurrentQueue<string>();
            Consumer consumer = new Consumer(f, queue, dataForSend);
        }
    }
}
