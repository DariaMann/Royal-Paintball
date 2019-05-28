using System;
using System.Threading;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;

namespace Server
{
    class SenderGame
    {
        private readonly ConcurrentQueue<string> dataForSend;
        private Thread thread;
        private volatile bool stopped;
        public List<Client> clients;

        public SenderGame(ConcurrentQueue<string> dataForSend, List<Client> clients)
        {
            this.stopped = true;
            this.dataForSend = dataForSend;
            this.clients = clients;
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

        public void Send(string message,NetworkStream stream)
        {
            byte[] messageBytes = Encoding.ASCII.GetBytes(message); // a UTF-8 encoder would be 'better', as this is the standard for network communications
            int length = messageBytes.Length;// determine length of message
            byte[] lengthBytes = System.BitConverter.GetBytes(length);// convert the length into bytes using BitConverter (encode)
            if (System.BitConverter.IsLittleEndian)// flip the bytes if we are a little-endian system: reverse the bytes in lengthBytes to do so
            {
                Array.Reverse(lengthBytes);
            }
            try
            {
                stream.Write(lengthBytes, 0, lengthBytes.Length);// send length
                stream.Write(messageBytes, 0, length);// send message
            }
            catch
            {
                Console.WriteLine("MISTACEN");
            }
        }

        public void Process()
        {
            while (!stopped)
            {
                Thread.Sleep(50);
                if (this.dataForSend.TryDequeue(out string field))
                {
                    Field mess = JsonConvert.DeserializeObject<Field>(field);

                    Console.WriteLine("clients.Count  "+ clients.Count);
                    for (int i = 0; i < clients.Count; i++)
                    {
                        foreach (Player player in mess.Player.Values)
                        {
                            if (player.ID == clients[i].ID)
                            {
                                player.Me = true;
                            }
                            else player.Me = false;
                        }
                        try
                        {
                            NetworkStream stream = clients[i].client.GetStream();
                            string message = JsonConvert.SerializeObject(mess, Formatting.Indented);
                            Console.WriteLine(message);
                           // Console.WriteLine("LOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOL");
                            string msg = "%" + message + "&";
                            Send(msg, stream);
                        }
                        catch(System.InvalidOperationException e)
                        {
                            Console.WriteLine(e);
                            clients.Remove(clients[i]);
                        }
                        foreach (Player player in mess.Player.Values)
                        {
                            if (player.Death)
                            {
                                for (int k = 0; k < clients.Count; k++)
                                {
                                    if (player.ID == clients[k].ID)
                                    {
                                        clients.Remove(clients[k]);
                                    }
                                }
                            }
                            if (player.Win)
                            {
                                Stop();
                            }
                        }
                    }
                }
            }
        }
    }
}