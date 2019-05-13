using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Net.Sockets;
using Newtonsoft.Json;
using System;
using System.Threading;


public class ClientTCP  {

    public TcpClient playerSocket;
    private bool connecting;
    private bool connected;
    public static NetworkStream myStream;
    private byte[] asyncBuff;

    //private Thread thread;
    //public void Start()
    //{
    //    thread = new Thread(clientTCP.SendFirstMessage(player));

    //    thread.Start();

    //}

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
            
          //  playerSocket.Connect("52.14.61.47", 904);

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
    public string GetPos ()//получение данных с сервера
    {
        try{
            byte[] data = new byte[256];
            StringBuilder response = new StringBuilder();
            myStream = playerSocket.GetStream();
            do
            {
                int bytes = myStream.Read(data, 0, data.Length);
                response.Append(Encoding.UTF8.GetString(data, 0, bytes));
            }
            while (myStream.DataAvailable); // пока данные есть в потоке

            return response.ToString();
        }
        catch
        {
            return "0";
        }
    }
    
    public void SendFirstMessage(Player clientData)//отправка сообщения со всеми данными
    {
        try
        {
            string message = JsonConvert.SerializeObject(clientData, Formatting.Indented);
            byte[] buffer = Encoding.ASCII.GetBytes(message);
            myStream = playerSocket.GetStream();
            myStream.Write(buffer, 0, buffer.Length);
        }
        catch
        {
          
        }
    }

   
    public void Send(Player player)//отправка сообщения со всеми данными
    {
        System.Threading.Thread.Sleep(100);
        string message = JsonConvert.SerializeObject(player, Formatting.Indented);
            byte[] buffer = Encoding.ASCII.GetBytes(message);
            myStream.Write(buffer, 0, buffer.Length);
    }
}
