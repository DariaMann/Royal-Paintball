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
    public string my_ID;
    public string Dir = "N";
    public string life = "30";
    public string weapon = "Pistol";
    public string shoot = "F";
    public bool IsItFirstMessage = true;
    static public Dictionary<string, Dictionary<string, string>> dasha = new Dictionary<string, Dictionary<string, string>>();
    public string mess;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start () {
        clientTCP.Connect();
        clientTCP.SendMess();
        mess = clientTCP.GetPos();
        InstantiatePlayer(Convert.ToString(my_ID), mess);
       // clientTCP.SendMess();
        // clientTCP.SendMessage(clientTCP.GetPos());
    }
   
    private void Update()
    {
        //  Debug.Log("YES");
        //clientTCP.SendMess();
        //mess = clientTCP.GetPos();
        // clientTCP.SendMessage(mess);
      //  MoveOther(my_ID, mess);
        if (dasha.Count > 1)
        {
            InstantiateOther(my_ID, dasha);
        }
    }
    public void FirstMessage(string str)//первое сообщение с моим ID
    {
        var jsonData1 = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(str);
        
            foreach (string s in jsonData1.Keys)
                my_ID = s;
        
    }
    public void InstantiatePlayer(string ID, string str)//Создание игрока
    {
        Debug.Log("STR: " + str);
        var jsonData1 = JsonConvert.DeserializeObject<Dictionary<string,Dictionary<string, string> >> (str);
        dasha = jsonData1;
        foreach (string s in jsonData1.Keys)
            my_ID = s;
        ID = my_ID;
        Debug.Log("ID: " + ID);
        if (jsonData1.ContainsKey(my_ID))
        {
            int id = Convert.ToInt32(jsonData1[ID]["id"]);
            float x = Convert.ToSingle(jsonData1[ID]["pos_x"]);
            float y = Convert.ToSingle(jsonData1[ID]["pos_y"]);
            float z = Convert.ToSingle(jsonData1[ID]["pos_z"]);
            float rotX = Convert.ToSingle(jsonData1[ID]["rot_x"]);
            float rotY = Convert.ToSingle(jsonData1[ID]["rot_y"]);
            float rotZ = Convert.ToSingle(jsonData1[ID]["rot_z"]);
            float posWx = Convert.ToSingle(jsonData1[ID]["posW_x"]);
            float posWy = Convert.ToSingle(jsonData1[ID]["posW_y"]);
            float posWz = Convert.ToSingle(jsonData1[ID]["posW_z"]);
            int life = Convert.ToInt32(jsonData1[ID]["life"]);
            Vector3 v = new Vector3(x, y, z);
            GameObject temp = Instantiate(playerPref, v, Quaternion.identity);
            temp.transform.rotation = Quaternion.Euler(rotX, rotY, rotZ);
            temp.name = Convert.ToString(id);
            if (!playerList.ContainsKey(id))
            playerList.Add(id, temp);
            Lifes.text = Convert.ToString(life);
        }
    }
    public void InstantiateOther(string ID, Dictionary<string, Dictionary<string, string>> str)//Создание игрока
    {
        foreach (string s in str.Keys)
        {
            if (s != ID && !playerList.ContainsKey(Convert.ToInt32(s)))
            {
                float x = Convert.ToSingle(str[s]["pos_x"]);
                float y = Convert.ToSingle(str[s]["pos_y"]);
                float z = Convert.ToSingle(str[s]["pos_z"]);
                float rotX = Convert.ToSingle(str[s]["rot_x"]);
                float rotY = Convert.ToSingle(str[s]["rot_y"]);
                float rotZ = Convert.ToSingle(str[s]["rot_z"]);
                Vector3 v = new Vector3(x, y, z);
                GameObject temp = Instantiate(playerPref, v, Quaternion.identity);
                temp.transform.rotation = Quaternion.Euler(rotX, rotY, rotZ);
                temp.name = Convert.ToString(s);
                playerList.Add(Convert.ToInt32(s), temp);
            }
        }
    }
    public void MoveOther(string ID, string str)
    {
        var jsonData1 = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(str);
        if (jsonData1.Count > 1)
        {
            foreach (string s in jsonData1.Keys)
            {
                if (s != ID)
                {
                    GameObject player = GameObject.Find(s);
                    float x = Convert.ToSingle(jsonData1[s]["pos_x"]);
                    float y = Convert.ToSingle(jsonData1[s]["pos_y"]);
                    float z = Convert.ToSingle(jsonData1[s]["pos_z"]);
                    float rotX = Convert.ToSingle(jsonData1[s]["rot_x"]);
                    float rotY = Convert.ToSingle(jsonData1[s]["rot_y"]);
                    float rotZ = Convert.ToSingle(jsonData1[s]["rot_z"]);
                    Vector3 v = new Vector3(x, y, z);
                    player.transform.position = v;
                    player.transform.rotation = Quaternion.Euler(rotX, rotY, rotZ);
                }
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
