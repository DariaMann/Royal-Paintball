using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using GameLibrary;

namespace Server
{
    class Waiting
    {
        private ConcurrentQueue<TcpClient> Wishing;
        private Thread thread;
        private volatile bool stopped;
        private int GamersCount = 2;
        private List<Game> Games;
        private Dictionary<TcpClient, Waiter> Waiters;
        private List<Waiter> waitPerson;

        public Waiting(ConcurrentQueue<TcpClient> Wishing)
        {
            this.Wishing = Wishing;
            Games = new List<Game>();
            waitPerson = new List<Waiter>();
            Waiters = new Dictionary<TcpClient, Waiter>();
            this.stopped = true;
        }

        public void Start()
        {
            if (stopped)
            {
                thread = new Thread(Process);

                stopped = false;

                thread.Start();
            }
        }

        public void Stop()
        {
            if (!stopped)
            {
                this.stopped = true;

                this.thread.Join();
            }

        }
        
        private byte[] ReadBytes(int count,NetworkStream networkStream)
        {

            byte[] bytes = new byte[count]; // buffer to fill (and later return)
            int readCount = 0; // bytes is empty at the start

            // while the buffer is not full
            while (readCount < count)
            {
                // ask for no-more than the number of bytes left to fill our byte[]
                int left = count - readCount; // we will ask for `left` bytes
                int r = networkStream.Read(bytes, readCount, left); // but we are given `r` bytes (`r` <= `left`)

                if (r == 0)
                { // I lied, in the default configuration, a read of 0 can be taken to indicate a lost connection
                    throw new Exception("Lost Connection during read");
                }

                readCount += r; // advance by however many bytes we read
            }

            return bytes;
        }

        private string ReadMessage(NetworkStream networkStream)
        {
            // read length bytes, and flip if necessary
            byte[] lengthBytes = ReadBytes(sizeof(int),  networkStream); // int is 4 bytes
            if (System.BitConverter.IsLittleEndian)
            {
                Array.Reverse(lengthBytes);
            }

            // decode length
            int length = System.BitConverter.ToInt32(lengthBytes, 0);

            // read message bytes
            byte[] messageBytes = ReadBytes(length,  networkStream);

            // decode the message
            string message = System.Text.Encoding.ASCII.GetString(messageBytes);

            return message;
        }

        public void SendMessage(NetworkStream stream)
        {
            var waiters = JsonConvert.SerializeObject(waitPerson, Formatting.Indented);
            string waiters1 = "%" + waiters + "&";
            byte[] messageBytes = Encoding.ASCII.GetBytes(waiters1); // a UTF-8 encoder would be 'better', as this is the standard for network communications
            // determine length of message
            int length = messageBytes.Length;
            // convert the length into bytes using BitConverter (encode)
            byte[] lengthBytes = System.BitConverter.GetBytes(length);
            // flip the bytes if we are a little-endian system: reverse the bytes in lengthBytes to do so
            if (System.BitConverter.IsLittleEndian)
            {
                Array.Reverse(lengthBytes);
            }
            // send length
            stream.Write(lengthBytes, 0, lengthBytes.Length);
            // send message
            stream.Write(messageBytes, 0, length);
        }

        public void Process()
        {
            NetworkStream stream = null;
            
            while (!stopped)
            {
                if (Waiters.Count >= GamersCount)
                {
                    Dictionary<TcpClient, Waiter> newDic = new Dictionary<TcpClient, Waiter>();
                    newDic = Waiters;
                    Game game = new Game(newDic);
                    game.Process();
                    Games.Add(game);
                    Waiters.Clear();
                    waitPerson.Clear();
                }
                else
                {
                    foreach (TcpClient client in Wishing)
                    {
                        stream = client.GetStream();
                        string message = ReadMessage(stream);
                        string waiterName = message.Substring((message.IndexOf("%") + 1), (message.IndexOf("&") - 1));
                        if (!Waiters.ContainsKey(client)) // отправляем обратно сообщение 
                        {
                            Waiter w = new Waiter(waiterName, DateTime.Now);
                            Waiters.Add(client, w);
                            waitPerson.Add(w);
                            if (waitPerson.Count >= GamersCount)
                            {
                                while (!Wishing.IsEmpty)
                                {
                                    if (this.Wishing.TryDequeue(out TcpClient clientTcp))
                                    {
                                        stream = clientTcp.GetStream();
                                        SendMessage(stream);
                                    }
                                }
                            }
                            else
                            {
                                SendMessage(stream);
                            }
                        }
                        else
                        {
                            SendMessage(stream);
                        }

                    }

                }
            }
        }
    }
}
