using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Net.Sockets;
using System.Net;
using Newtonsoft.Json;

public class ClientTCP  {

    public TcpClient playerSocket;
    private bool connecting;
    private bool connected;
    public static NetworkStream myStream;
    private byte[] asyncBuff;
    //public bool shoot = false;



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
          //   playerSocket.BeginConnect("127.0.0.1", 904, new AsyncCallback(ConnectCallback), playerSocket);
            playerSocket.Connect("127.0.0.1", 904);
            
            connecting = true;
        }
        catch { };
        
    }
    public string GetPos ()
    {
            byte[] data = new byte[256];
            StringBuilder response = new StringBuilder();
            myStream = playerSocket.GetStream();
            do
            {
                int bytes = myStream.Read(data, 0, data.Length);
                response.Append(Encoding.UTF8.GetString(data, 0, bytes));
            }
            while (myStream.DataAvailable); // пока данные есть в потоке
        Debug.Log("MES: "+response.ToString());
            return response.ToString();
        
    }
    //public static void Message(string ID, string s)
    //{
    //    var jsonData1 = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(s);
    //    Debug.Log("ID: "+ID);
    //    Debug.Log("s: "+s);
    //    Debug.Log("jsonData1: " + jsonData1);
    //    Debug.Log("jsonData1zzzzzzz: " + jsonData1.Count);
    //    Debug.Log("jsonData1: " + jsonData1[ID]);

    //    int id = Convert.ToInt32(jsonData1[ID]["id"]);
    //    float x = Convert.ToSingle(jsonData1[ID]["pos_x"]);
    //    float y = Convert.ToSingle(jsonData1[ID]["pos_y"]);
    //    float z = Convert.ToSingle(jsonData1[ID]["pos_z"]);
    //    float rotX = Convert.ToSingle(jsonData1[ID]["rot_x"]);
    //    float roty = Convert.ToSingle(jsonData1[ID]["rot_y"]);
    //    float rotz = Convert.ToSingle(jsonData1[ID]["rot_z"]);
    //    float posWx = Convert.ToSingle(jsonData1[ID]["posW_x"]);
    //    float posWy = Convert.ToSingle(jsonData1[ID]["posW_y"]);
    //    float posWz = Convert.ToSingle(jsonData1[ID]["posW_z"]);
    //    int life = Convert.ToInt32(jsonData1[ID]["life"]);
    //    Debug.Log(id + "," + x + "," + y + "," + z + "," + rotX + "," + roty + "," + rotz + "," + posWx + "," + posWy + "," + posWz + "," + life);
    //}
    //public void Message(int i)
    //{
    //    try
    //    {
    //        socket.Connect("127.0.0.1", 904);
    //        string message = Convert.ToString(i);
    //        byte[] buffer = Encoding.ASCII.GetBytes(message);
    //        myStream.Write(buffer, 0, buffer.Length);
    //    }
    //    catch { }
    //   // playerSocket.Send(buffer);
    //}
    //public void SendMessage/*(string str)//*/(string Id,string str, string Dir,string lifes,ref string shoot, string weapon)//отправка сообщения со всеми данными
    //{
    //    var jsonData1 = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(str);
    //    Debug.Log("DIRR "+Dir);
    //    //jsonData1[Id]["dir"] = Dir;
    //    //jsonData1[Id]["life"] = lifes;
    //    //jsonData1[Id]["shoot"] = shoot;
    //    //jsonData1[Id]["weapon"] = weapon;
    //    string message = JsonConvert.SerializeObject(jsonData1);
    //    byte[] buffer = Encoding.ASCII.GetBytes(message);
    //    myStream.Write(buffer, 0, buffer.Length);
    //   // shoot = "F";
    //}
    //public void SendFirstMess()//отправка сообщения со всеми данными
    //{
    //    string message = JsonConvert.SerializeObject("Hi");
    //    byte[] buffer = Encoding.ASCII.GetBytes(message);
    //    myStream = playerSocket.GetStream();
    //    myStream.Write(buffer, 0, buffer.Length);
    //}
    public void SendFirstMessage(Dictionary<string,string> clientData)//отправка сообщения со всеми данными
    {
        string message = JsonConvert.SerializeObject(clientData);
        byte[] buffer = Encoding.ASCII.GetBytes(message);
        myStream = playerSocket.GetStream();
        myStream.Write(buffer, 0, buffer.Length);
    }
    public void Send/*(string str)//*/(string Id, string str, /*, string Dir, string lifes, ref string shoot, string weapon,*/ Dictionary<string, string> clientData)//отправка сообщения со всеми данными
    {
        Debug.Log("STR SEND:" + str);
        var jsonData1 = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(str);
        clientData = jsonData1[Id];
        //jsonData1[Id]["dir"] = Dir;
        //jsonData1[Id]["life"] = lifes;
        //jsonData1[Id]["shoot"] = shoot;
        //jsonData1[Id]["weapon"] = weapon;
        string message = JsonConvert.SerializeObject(clientData);
        byte[] buffer = Encoding.ASCII.GetBytes(message);
        myStream.Write(buffer, 0, buffer.Length);
        // shoot = "F";
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
                myStream = playerSocket.GetStream();
               myStream.BeginRead(asyncBuff, 0, 8192, OnReceive, null);
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
    private void OnReceive(IAsyncResult ar)//при получении
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
