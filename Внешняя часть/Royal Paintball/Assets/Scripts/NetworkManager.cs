using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Newtonsoft.Json;


public class NetworkManager : MonoBehaviour {

    private ClientTCP clientTCP = new ClientTCP();
    [SerializeField]
    public GameObject playerPref;
    public GameObject headPref;
    public GameObject weaponPref;
    public Dictionary<int, GameObject> playerList = new Dictionary<int, GameObject>();
    public Text Lifes;
    public int my_ID;
    public string Dir="N";
    public string life = "30" ;
    public string weapon = "Pistol";
    public string shoot = "F";
    public bool IsItFirstMessage = true;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start () {
        //clientTCP.Connect();
        //if (IsItFirstMessage)
        //{
        //    FirstMessage(clientTCP.GetPos());
        //    IsItFirstMessage = false;
        //}
        //else
        //{
        //    InstantiatePlayer(Convert.ToString(my_ID), clientTCP.GetPos());
        //}
     // InstantiatePlayer("333", clientTCP.GetPos());
        //clientTCP.SendMess();
        //FirstMessage(clientTCP.GetPos());
    }
    public void Pos(string str)
    {
        string[] words = str.Split(new char[] { ',' });
        float x= Convert.ToSingle(words[0]);
        float y= Convert.ToSingle(words[1]); 
        float z= Convert.ToSingle(words[2]);
        float xW = Convert.ToSingle(words[3]);
        float yW = Convert.ToSingle(words[4]);
        Vector3 v = new Vector3(x, y, z);
        playerPref.transform.position=v;
        Vector3 v2 = new Vector3(xW, yW, z);
        weaponPref.transform.position = v;
    }
    private void Update()
    {
        Debug.Log(IsItFirstMessage);
        clientTCP.Connect();
        //if (IsItFirstMessage)
        //{
        //    FirstMessage(clientTCP.GetPos());
        //    IsItFirstMessage = false;
        //}
        //else
        {
            InstantiatePlayer(Convert.ToString(my_ID), clientTCP.GetPos());
        }
        // MovePlayer(Convert.ToString(my_ID), clientTCP.GetPos());

        // clientTCP.Message(MyIndex);
        //foreach(GameObject p in playerList.Values)
        // {
        //     int i;
        //     if(p == playerPref)
        //     {
        //         i = playerList[playerPref];
        //     }
        // }
        //string pl = playerPref.name;
        //  Debug.Log(clientTCP.shoot);
        //clientTCP.Pl(333);

        //if (clientTCP.shoot)
        //{
        //    clientTCP.Shoot(Convert.ToInt32(pl), playerPref);
        //    Debug.Log("SHOOT");
        //}

        //  clientTCP.SendMessage(Convert.ToString(my_ID), clientTCP.GetPos(),Dir,life,ref shoot,weapon);
        // clientTCP.shoot = false;
    }
    public void FirstMessage(string str)//первое сообщение с моим ID
    {
        var jsonData1 = JsonConvert.DeserializeObject<string>(str);
        string id = jsonData1;
        my_ID = Convert.ToInt32(id);
        Debug.Log("ID: "+my_ID);
       // return Convert.ToString(id);
    }
    public void InstantiatePlayer(string ID, string str)//Создание игрока
    {
        Debug.Log("STR: " + str);
        var jsonData1 = JsonConvert.DeserializeObject<Dictionary<string,Dictionary<string, string> >> (str);
        if (!jsonData1.ContainsKey(Convert.ToString(my_ID)))
        {
            int id = Convert.ToInt32(jsonData1[ID]["id"]);
            float x = Convert.ToSingle(jsonData1[ID]["pos_x"]);
            float y = Convert.ToSingle(jsonData1[ID]["pos_y"]);
            float z = Convert.ToSingle(jsonData1[ID]["pos_z"]);
            float rotX = Convert.ToSingle(jsonData1[ID]["rot_x"]);
            float roty = Convert.ToSingle(jsonData1[ID]["rot_y"]);
            float rotz = Convert.ToSingle(jsonData1[ID]["rot_z"]);
            float posWx = Convert.ToSingle(jsonData1[ID]["posW_x"]);
            float posWy = Convert.ToSingle(jsonData1[ID]["posW_y"]);
            float posWz = Convert.ToSingle(jsonData1[ID]["posW_z"]);
            int life = Convert.ToInt32(jsonData1[ID]["life"]);
            Vector3 v = new Vector3(x, y, z);
            GameObject temp = Instantiate(playerPref, v, Quaternion.identity);
            temp.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x - 90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            //  playerPref = temp;

            temp.name = Convert.ToString(id);
            playerList.Add(id, temp);
            Lifes.text = Convert.ToString(life);
        }
    }
    public void InstantiateOther(string ID, Dictionary<string, Dictionary<string, string>> str)//Создание игрока
    {
        foreach(string s in str.Keys)
        {
            if (s!=ID)
            {
                float x = Convert.ToSingle(str[s]["pos_x"]);
                float y = Convert.ToSingle(str[s]["pos_y"]);
                float z = Convert.ToSingle(str[s]["pos_z"]);
                Vector3 v = new Vector3(x, y, z);
                GameObject temp = Instantiate(playerPref, v, Quaternion.identity);
                temp.name = Convert.ToString(s);
                playerList.Add(Convert.ToInt32(s), temp);
            }
        }
    }
    public void MovePlayer(string ID, string str)//ответ сервера, смена позиции при W,S,A,D
    {
        var jsonData1 = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(str);
        if (jsonData1.Count>1)
        {
            InstantiateOther(ID, jsonData1);
        }
        //int id = Convert.ToInt32(jsonData1[ID]["id"]);
        //float x = Convert.ToSingle(jsonData1[ID]["pos_x"]);
        //float y = Convert.ToSingle(jsonData1[ID]["pos_y"]);
        //float z = Convert.ToSingle(jsonData1[ID]["pos_z"]);
        //float rotX = Convert.ToSingle(jsonData1[ID]["rot_x"]);
        //float rotY = Convert.ToSingle(jsonData1[ID]["rot_y"]);
        //float rotZ = Convert.ToSingle(jsonData1[ID]["rot_z"]);
        //float posWx = Convert.ToSingle(jsonData1[ID]["posW_x"]);
        //float posWy = Convert.ToSingle(jsonData1[ID]["posW_y"]);
        //float posWz = Convert.ToSingle(jsonData1[ID]["posW_z"]);
        //Vector3 v = new Vector3(x, y, z);
        //playerPref.transform.position = v;
        //playerPref.transform.rotation = Quaternion.Euler(rotX,rotY,rotZ);
    }
      
}
