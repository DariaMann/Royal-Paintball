﻿using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Net.Sockets;
using Newtonsoft.Json;

public class ClientTCP  {

    public TcpClient playerSocket;
    private bool connecting;
    private bool connected;
    public static NetworkStream myStream;
    private byte[] asyncBuff;

    // static Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
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
        string message = JsonConvert.SerializeObject(clientData, Formatting.Indented);
        byte[] buffer = Encoding.ASCII.GetBytes(message);
        myStream = playerSocket.GetStream();
        myStream.Write(buffer, 0, buffer.Length);
    }
    public void Send(Player player)//отправка сообщения со всеми данными
    {
        string message = JsonConvert.SerializeObject(player, Formatting.Indented);
        byte[] buffer = Encoding.ASCII.GetBytes(message);
        myStream.Write(buffer, 0, buffer.Length);
    }
}
