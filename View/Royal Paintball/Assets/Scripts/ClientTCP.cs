using UnityEngine;
using System.Text;
using System.Net.Sockets;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using System;
using System.Threading;
using System.Collections.Concurrent;
using System.Collections.Generic;
using GameLibrary;


public class Sender
{
    private readonly ConcurrentQueue<Player> dataForSend;
    private readonly ConcurrentQueue<string> dataForSendName;
    private Thread thread;
    private volatile bool stopped;
    public TcpClient playerSocket;
    public static NetworkStream stream;
    private bool Game;

    public Sender(ConcurrentQueue<Player> dataForSend, TcpClient playerSocket, ConcurrentQueue<string> dataForSendName)
    {
        this.stopped = true;
        this.dataForSend = dataForSend;
        this.playerSocket = playerSocket;
        this.dataForSendName = dataForSendName;
        Game = false;
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
        stream = playerSocket.GetStream();
        while (!stopped)
        {
            Thread.Sleep(50);
            string name;
            if (this.dataForSendName.TryDequeue(out name))
            {
                string meesage = JsonConvert.SerializeObject(name, Formatting.Indented);
                string msg = "%" + meesage + "&";
                byte[] messageBytes = Encoding.ASCII.GetBytes(msg); // a UTF-8 encoder would be 'better', as this is the standard for network communications
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

                }
            }
              
            Player player;
            if (this.dataForSend.TryDequeue(out player))
            {
                string meesage = JsonConvert.SerializeObject(player, Formatting.Indented);
                string msg = "%" + meesage + "&";
                byte[] messageBytes = Encoding.ASCII.GetBytes(msg); // a UTF-8 encoder would be 'better', as this is the standard for network communications
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

                }
            }
        }
    }
}

public class Recipient
{
    public TcpClient playerSocket;
    public static NetworkStream stream;
    public bool Game = false;
    public ConcurrentQueue<Field> queue;
    public ConcurrentQueue<int> queueCount;

    private Thread thread;

    private volatile bool stopped;

    public Recipient(ConcurrentQueue<Field> queue, TcpClient playerSocket, ConcurrentQueue<int> queueCount)
    {
        this.playerSocket = playerSocket;
        this.stopped = true;
        this.queue = queue;
        this.queueCount = queueCount;
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

    private byte[] ReadBytes(int count)
    {
        stream = playerSocket.GetStream();

        byte[] bytes = new byte[count]; // buffer to fill (and later return)
        int readCount = 0; // bytes is empty at the start
        while (readCount < count)// while the buffer is not full
        {
            int left = count - readCount;// ask for no-more than the number of bytes left to fill our byte[] // we will ask for `left` bytes
            int r = stream.Read(bytes, readCount, left); // but we are given `r` bytes (`r` <= `left`)
            if (r == 0)
            {
                throw new Exception("Lost Connection during read");// I lied, in the default configuration, a read of 0 can be taken to indicate a lost connection
            }
            readCount += r; // advance by however many bytes we read
        }
        return bytes;
    }

    private string ReadMessage()
    {
        byte[] lengthBytes = ReadBytes(sizeof(int));// read length bytes, and flip if necessary // int is 4 bytes
        if (System.BitConverter.IsLittleEndian)
        {
            Array.Reverse(lengthBytes);
        }
        int length = System.BitConverter.ToInt32(lengthBytes, 0);// decode length
        byte[] messageBytes = ReadBytes(length);// read message bytes
        string message = System.Text.Encoding.ASCII.GetString(messageBytes);// decode the message

        return message;
    }

    public void Process()
    {
        stream = playerSocket.GetStream();
        while (!stopped)
        {
            string message = ReadMessage();
            message = message.Substring((message.IndexOf("%") + 1), (message.IndexOf("&") - 1));
            if (Game == false)
            {
                Debug.Log("wail");
                try
                {
                    int count = JsonConvert.DeserializeObject<int>(message);
                    if (queueCount.Count < 1)
                    { this.queueCount.Enqueue(count); }
                }
                catch (Newtonsoft.Json.JsonReaderException e)
                {
                    Debug.Log(e);
                    Game = true;
                }
            }
            else
            {
                Debug.Log("play");
                Field field = JsonConvert.DeserializeObject<Field>(message);

                if (queue.Count < 1)
                { this.queue.Enqueue(field); }
            }
        }
    }
}

public class ClientTCP
{
    public TcpClient playerSocket;
    private bool connecting;
    private bool connected;
    public static NetworkStream myStream;
    private byte[] asyncBuff;

    public void Connect()
    {
        try
        {
            Debug.Log("connect");
            playerSocket = new TcpClient();
            playerSocket.ReceiveBufferSize = 4096;//размер буфера приема
            playerSocket.SendBufferSize = 4096;//размер буфера отправки
            playerSocket.NoDelay = false;
            asyncBuff = new byte[8192];
            playerSocket.Connect("127.0.0.1", 904);
            connecting = true;
        }
        catch { };
    }

    public void Disconnect()
    {
        try
        {
            Debug.Log("disconnect");
            playerSocket.GetStream().Close();
            playerSocket.Close();
            connecting = false;
        }
        catch { };
    }

    private byte[] ReadBytes(int count)
    {
        myStream = playerSocket.GetStream();

        byte[] bytes = new byte[count]; // buffer to fill (and later return)
        int readCount = 0; // bytes is empty at the start
        while (readCount < count)// while the buffer is not full
        {
            int left = count - readCount;// ask for no-more than the number of bytes left to fill our byte[] // we will ask for `left` bytes
            int r = myStream.Read(bytes, readCount, left); // but we are given `r` bytes (`r` <= `left`)
            if (r == 0)
            {
                throw new Exception("Lost Connection during read");// I lied, in the default configuration, a read of 0 can be taken to indicate a lost connection
            }
            readCount += r; // advance by however many bytes we read
        }
        return bytes;
    }

    private string ReadMessage()
    {
        byte[] lengthBytes = ReadBytes(sizeof(int));// read length bytes, and flip if necessary // int is 4 bytes
        if (System.BitConverter.IsLittleEndian)
        {
            Array.Reverse(lengthBytes);
        }
        int length = System.BitConverter.ToInt32(lengthBytes, 0);// decode length
        byte[] messageBytes = ReadBytes(length);// read message bytes
        string message = System.Text.Encoding.ASCII.GetString(messageBytes);// decode the message

        return message;
    }

    public string GetPos()//получение данных с сервера
    {
        string message = ReadMessage();
        Debug.Log("mess: " + message);
        string command = message.Substring((message.IndexOf("%") + 1), (message.IndexOf("&") - 1));
        Debug.Log(command);
        return command;
    }

    public void Send(string name)//отправка сообщения со всеми данными
    {
        myStream = playerSocket.GetStream();
        string message = JsonConvert.SerializeObject(name, Formatting.Indented);
        string msg = "%" + message + "&";
        byte[] messageBytes = Encoding.ASCII.GetBytes(msg); // a UTF-8 encoder would be 'better', as this is the standard for network communications
        int length = messageBytes.Length;// determine length of message
        byte[] lengthBytes = System.BitConverter.GetBytes(length);// convert the length into bytes using BitConverter (encode)
        if (System.BitConverter.IsLittleEndian)// flip the bytes if we are a little-endian system: reverse the bytes in lengthBytes to do so
        {
            Array.Reverse(lengthBytes);
        }
        myStream.Write(lengthBytes, 0, lengthBytes.Length);// send length
        myStream.Write(messageBytes, 0, length);// send message
                                                //    Debug.Log("Лег сервер");
                                                //    Disconnect();
                                                //    SceneManager.LoadScene("Play");
    }

    public void Send(Player player)//отправка сообщения со всеми данными
    {
        string message = JsonConvert.SerializeObject(player, Formatting.Indented);
        string msg = "%" + message + "&";
        byte[] messageBytes = Encoding.ASCII.GetBytes(msg); // a UTF-8 encoder would be 'better', as this is the standard for network communications
        int length = messageBytes.Length;  // determine length of message
        byte[] lengthBytes = System.BitConverter.GetBytes(length);// convert the length into bytes using BitConverter (encode)
        if (System.BitConverter.IsLittleEndian)// flip the bytes if we are a little-endian system: reverse the bytes in lengthBytes to do so
        {
            Array.Reverse(lengthBytes);
        }
        try
        {
            myStream.Write(lengthBytes, 0, lengthBytes.Length);// send length
            myStream.Write(messageBytes, 0, length);// send message
        }
        catch
        {
            Debug.Log("Лег сервер");
            Disconnect();
            SceneManager.LoadScene("Play");
        }
    }
}



