using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using GameLibrary;

public class Waiting : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        clientTCP.Connect();//коннект с сервером                                     

        player.ID = -1;

        clientTCP.SendFirstMessage(player);//отправка первого сообщения серверу            

        mess = clientTCP.GetPos();//данные с сервера  

        Field jsonData1 = JsonConvert.DeserializeObject<Field>(mess);
        field = jsonData1;
        foreach (int c in field.Player.Keys)
        {
            my_ID = field.Player[c].ID;
        }
        IsItFirstMessage = false;
    }
    private ClientTCP clientTCP = new ClientTCP();
    string mess;
    public Player player = new Player();
    public Field field = new Field();
    public int my_ID;
    static public bool IsItFirstMessage = true;
    // Update is called once per frame
    void Update()
    {
        if (!IsItFirstMessage)
        {
            clientTCP.Send(field.Player[my_ID]);
            mess = clientTCP.GetPos();//данные с сервера  

            Field jsonData1 = new Field();
            try
            {
                jsonData1 = JsonConvert.DeserializeObject<Field>(mess);
            }
            catch { }
            field = jsonData1;
            if (field.Player.Count < 2)
            {

            }
            else
            {
                SceneManager.LoadScene("Game");
            }
        }
    }
}
