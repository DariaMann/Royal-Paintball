using System;
using System.Threading;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using GameLibrary;

namespace Server
{
    class Sender
    {
        private readonly ConcurrentQueue<Field> dataForSend;
        private Thread thread;
        private volatile bool stopped;
        List<Client> clients;

        public Sender(ConcurrentQueue<Field> dataForSend, List<Client> clients)
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

        public void Process()
        {
            while (!stopped)
            {
                Thread.Sleep(50);
                if (this.dataForSend.TryDequeue(out Field mess))
                {
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
                        string meesage = JsonConvert.SerializeObject(mess, Formatting.Indented);
                        //Console.WriteLine(meesage);
                        string msg = "%" + meesage + "&";
                        //Console.WriteLine("Sender: " + msg);
                        NetworkStream stream = null;
                        stream = clients[i].client.GetStream();
                        byte[] messageBytes = Encoding.ASCII.GetBytes(msg); // a UTF-8 encoder would be 'better', as this is the standard for network communications
                        int length = messageBytes.Length;// determine length of message
                        byte[] lengthBytes = System.BitConverter.GetBytes(length);// convert the length into bytes using BitConverter (encode)
                        if (System.BitConverter.IsLittleEndian)// flip the bytes if we are a little-endian system: reverse the bytes in lengthBytes to do so
                        {
                            Array.Reverse(lengthBytes);
                        }
                        try {
                            stream.Write(lengthBytes, 0, lengthBytes.Length);// send length
                            stream.Write(messageBytes, 0, length);// send message
                        }
                        catch
                        {
                            //if (clientTSP.Count != 0)
                            //{
                            //    clientTSP.Remove(clientTSP[i]);
                            //    clientTSP[i].Close();
                            //}
                        }
                    }
                }
            }
        }
    }
}