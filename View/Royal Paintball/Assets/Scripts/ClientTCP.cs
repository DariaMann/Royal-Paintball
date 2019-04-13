using System.Collections.Generic;
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
          //   playerSocket.BeginConnect("127.0.0.1", 904, new AsyncCallback(ConnectCallback), playerSocket);
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
    
    public void SendFirstMessage(Dictionary<string,string> clientData)//отправка сообщения со всеми данными
    {
        string message = JsonConvert.SerializeObject(clientData);
        byte[] buffer = Encoding.ASCII.GetBytes(message);
        myStream = playerSocket.GetStream();
        myStream.Write(buffer, 0, buffer.Length);
    }
    public void Send(string Id, string dasha, string Dir, string shoot, string weapon, string wound,string liftItem,string reload, Dictionary<string, string> clientData,string[] startPos, string[] endPos)//отправка сообщения со всеми данными
    {
        var jsonData1 = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(dasha);
        
        jsonData1[Id]["dir"] = Dir;
       
        jsonData1[Id]["shoot"] = shoot;
       
        jsonData1[Id]["weapon"] = weapon;

        jsonData1[Id]["wound"] = wound;

        jsonData1[Id]["liftItem"] = liftItem;

        jsonData1[Id]["reload"] = reload;
        Debug.Log(startPos.Length);
        jsonData1[Id]["startX"] = startPos[0];
        jsonData1[Id]["startY"] = startPos[1];
        jsonData1[Id]["startZ"] = startPos[2];

        jsonData1[Id]["endX"] = endPos[0];
        jsonData1[Id]["endY"] = endPos[1];
        jsonData1[Id]["endZ"] = endPos[2];

        clientData = jsonData1[Id];
        string message = JsonConvert.SerializeObject(clientData);
        byte[] buffer = Encoding.ASCII.GetBytes(message);
        myStream.Write(buffer, 0, buffer.Length);
       
    }
    //private void ConnectCallback(IAsyncResult ar)
    //{
    //    try
    //    {
    //        playerSocket.EndConnect(ar);
    //        if(playerSocket.Connected == false)
    //        {
    //            connected = false;
    //            connecting = false;
    //            return;
    //        }
    //        else
    //        {
    //            playerSocket.NoDelay = true;
    //            myStream = playerSocket.GetStream();
    //           myStream.BeginRead(asyncBuff, 0, 8192, OnReceive, null);
    //            connected = true;
    //            connecting = false;
    //            Debug.Log("Успешное подключение к серверу");
    //        }
    //    }
    //    catch
    //    {
    //        connecting = false;
    //        connected = false;
    //        Debug.Log("Невозможно подключится к серверу");
    //    }
    //}
    //private void OnReceive(IAsyncResult ar)//при получении
    //{
    //    try
    //    {
    //        int byteAmt = myStream.EndRead(ar);
    //        byte[] myBytes = new byte[byteAmt];
    //        Buffer.BlockCopy(asyncBuff, 0, myBytes, 0, byteAmt);
    //        if (byteAmt == 0) return;
    //    }
    //    catch { }
    //}
}
