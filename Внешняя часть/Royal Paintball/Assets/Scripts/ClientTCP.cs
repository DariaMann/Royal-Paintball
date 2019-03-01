using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Net.Sockets;
using System.Net;

public class ClientTCP  {

    public TcpClient playerSocket;
    private bool connecting;
    private bool connected;
    public static NetworkStream myStream;
    private byte[] asyncBuff;


   // static Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    public void Connect()
    {
        playerSocket = new TcpClient();
        playerSocket.ReceiveBufferSize = 4096;//размер буфера приема
        playerSocket.SendBufferSize = 4096;//размер буфера отправки
        playerSocket.NoDelay = false;
        asyncBuff = new byte[8192];
        // playerSocket.BeginConnect("127.0.0.1", 904, new AsyncCallback(ConnectCallback), playerSocket);
        playerSocket.Connect("127.0.0.1", 904);
        connecting = true;
        
    }
    public string GetPos ()
    {

        byte[] data = new byte[256];
        StringBuilder response = new StringBuilder();
       // playerSocket.NoDelay = true;
        myStream = playerSocket.GetStream();
        do
        {
            int bytes = myStream.Read(data, 0, data.Length);
            response.Append(Encoding.UTF8.GetString(data, 0, bytes));
        }
        while (myStream.DataAvailable); // пока данные есть в потоке

        Console.WriteLine(response.ToString());

        return response.ToString();

    }
    public void Message(int i)
    {
        try
        {
            //socket.Connect("127.0.0.1", 904);
            string message = Convert.ToString(i);
            byte[] buffer = Encoding.ASCII.GetBytes(message);
            myStream.Write(buffer, 0, buffer.Length);
        }
        catch { }
        //playerSocket.Send(buffer);
    }
    public void Place(GameObject player)
    {
        try
        {
            float x = player.transform.position.x;
            float y = player.transform.position.y;
            float z = player.transform.position.z;
            //socket.Connect("127.0.0.1", 904);
            string message = Convert.ToString(x + "," + y+","+z);
            byte[] buffer = Encoding.ASCII.GetBytes(message);
            myStream.Write(buffer, 0, buffer.Length);
        }
        catch { }
        //playerSocket.Send(buffer);
    }
    private void ConnectCallback(IAsyncResult ar)
    {
        try
        {
            playerSocket.EndConnect(ar);
            if(playerSocket.Connected == false)
            {
                connected = false;
                connecting = false;
                return;
            }
            else
            {
                playerSocket.NoDelay = true;
               // myStream = playerSocket.GetStream();
               // myStream.BeginRead(asyncBuff, 0, 8192, OnReceive, null);
                connected = true;
                connecting = false;
                Debug.Log("Успешное подключение к серверу");
            }
        }
        catch
        {
            connecting = false;
            connected = false;
            Debug.Log("Невозможно подключится к серверу");
        }
    }
    private void OnReceive(IAsyncResult ar)
    {
        try
        {
            int byteAmt = myStream.EndRead(ar);
            byte[] myBytes = new byte[byteAmt];
            Buffer.BlockCopy(asyncBuff, 0, myBytes, 0, byteAmt);
            if (byteAmt == 0) return;
        }
        catch { }
    }
}
