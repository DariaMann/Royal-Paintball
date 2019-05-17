using System;
using System.Threading;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;
using System.Text;
using GameLibrary;

namespace Server
{
    class Sender
    {
        private readonly ConcurrentQueue<string> dataForSend;

        private Thread thread;
        private volatile bool stopped;
        List<TcpClient> clientTSP;

        public Sender(ConcurrentQueue<string> dataForSend, List<TcpClient> clientTSP)
        {
            this.stopped = true;
            this.dataForSend = dataForSend;
            this.clientTSP = clientTSP;
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
                    if (this.dataForSend.TryDequeue(out string mess))
                    {
                        for (int i = 0; i < clientTSP.Count; i++)
                        {
                            NetworkStream stream = null;
                            stream = clientTSP[i].GetStream();
                            byte[] data = new byte[256];
                       
                             Console.WriteLine("СЕРВЕР: " + mess);
                            data = Encoding.UTF8.GetBytes(mess);
                            stream.Write(data, 0, data.Length);
                            stream.Flush();
                        }

                    }
                
            }
        }
    }
}