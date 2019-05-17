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
    public class Producer//Producer
    {
        static public int ID = 555;
        public TcpClient client;
        public Field field;//поле игры

        private readonly ConcurrentQueue<Player> queue;
        private readonly ConcurrentQueue<string> dataForSend;

        private Thread thread;
        private volatile bool stopped;

        public Producer(TcpClient tcpClient, Field Field, ConcurrentQueue<Player> queue, ConcurrentQueue<string> dataForSend)
        {
            this.dataForSend = dataForSend;
            this.queue = queue;
            this.field = Field;
            this.client = tcpClient;
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

        public string PlayerData1()//создание игрока 
        {
            string color = First();//генерация ID игрока
            field.Player.Add(
                ID,
                new Player()
                {
                    ID = ID,
                    Color = color
                });

            string json = JsonConvert.SerializeObject(field, Formatting.Indented);
            return json;
        }
        public string First()
        {
            Random rn = new Random(); // объявление переменной для генерации чисел
            int id = rn.Next(0, 1000);
            ID = id;
            int col = rn.Next(0, field.Colors.Count);
            string color = field.Colors[col];
            field.Colors.Remove(color);
            return color;
        }

        public void Process()
        {
            NetworkStream stream = null;

            stream = client.GetStream();
            byte[] data = new byte[256]; // буфер для получаемых данных
            while (true)
            {
                // получаем сообщение
                StringBuilder builder = new StringBuilder();
                int bytes = 0;

                do
                {
                    bytes = stream.Read(data, 0, data.Length);
                    builder.Append(Encoding.UTF8.GetString(data, 0, bytes));
                }
                while (stream.DataAvailable);


                string message = builder.ToString();

                    Console.WriteLine("Клиент: " + message);
                Player jsonData1 = new Player();
                try
                {
                     jsonData1 = JsonConvert.DeserializeObject<Player>(message);

                }catch(Exception e)
                {
                    Console.WriteLine(e);
                }

                if(jsonData1 == null)
                {
                    Stop();
                }
                {
                    if (jsonData1.ID == -1)
                    // отправляем обратно сообщение 
                    {
                        message = PlayerData1();
                        Console.WriteLine("СЕРВЕР: " + message);
                        data = Encoding.UTF8.GetBytes(message);
                        stream.Write(data, 0, data.Length);
                    }
                    else
                    {
                        this.queue.Enqueue(jsonData1);
                        if (this.dataForSend.TryDequeue(out string mess))
                        {
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

    }

    