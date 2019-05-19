using System;
using System.Threading;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

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
                    string msg = "%" +mess+ "&";
                    for (int i = 0; i < clientTSP.Count; i++)
                    {
                        NetworkStream stream = null;
                        stream = clientTSP[i].GetStream();;
                        byte[] messageBytes = Encoding.ASCII.GetBytes(msg); // a UTF-8 encoder would be 'better', as this is the standard for network communications

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
                }
            }
        }
    }
}