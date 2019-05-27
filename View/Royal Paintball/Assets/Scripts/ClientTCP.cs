using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Net.Sockets;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using System;

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
        catch {
            Debug.Log("not connect");
        };

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
        catch {
            Debug.Log("not disconnect");

        };

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
        string command = message.Substring((message.IndexOf("%") + 1), (message.IndexOf("&") - 1));
        Debug.Log(command);
        return command;
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
            Debug.Log("Лег сервер");
            Disconnect();
            SceneManager.LoadScene("Play");
        }
    }
    //    public void Send(Player player)//отправка сообщения со всеми данными
    //    {
    //        string message = JsonConvert.SerializeObject(player, Formatting.Indented);
    //        byte[] buffer = Encoding.ASCII.GetBytes(message);
    //        try
    //        {
    //            myStream.Write(buffer, 0, buffer.Length);
    //        }
    //        catch
    //        {
    //            Debug.Log("Лег сервер");
    //            Disconnect();
    //            SceneManager.LoadScene("Play");
    //        }
    //    }
    //}


}