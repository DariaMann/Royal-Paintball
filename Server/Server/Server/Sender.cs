using System;
using System.Threading;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    class Sender
    {
        private readonly ConcurrentQueue<Field> dataForSend;

        private Thread thread;
        private volatile bool stopped;
        List<TcpClient> clientTSP;

        public Sender( ConcurrentQueue<Field> dataForSend, List<TcpClient> clientTSP)
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
               // Console.WriteLine("Sender: " + dataForSend.Count);
               
                    //while (dataForSend.Count != 0)
                    {
                        if (this.dataForSend.TryDequeue(out Field f))
                        {
                            for (int i = 0; i < clientTSP.Count; i++)
                            {
                                NetworkStream stream = null;
                                stream = clientTSP[i].GetStream();
                                byte[] data = new byte[256];

                                var mess = JsonConvert.SerializeObject(f, Formatting.Indented);
                               // Console.WriteLine("СЕРВЕР: " + mess);
                                data = Encoding.UTF8.GetBytes(mess);
                                stream.Write(data, 0, data.Length);
                                stream.Flush();
                            }

                        }
                    }
            }
        }
    }
}
