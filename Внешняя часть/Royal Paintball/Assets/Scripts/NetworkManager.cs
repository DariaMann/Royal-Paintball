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
    public GameObject weaponPref;
    public GameObject bulletPref;
    public GameObject cur;
    public Dictionary<int, GameObject> playerList = new Dictionary<int, GameObject>();
    public Dictionary<int, GameObject> bulletList = new Dictionary<int, GameObject>();
    public Text Lifes;
    public Text CountBul;
    public string my_ID = "111";

    public string Dir = "N";
    
    public string life = "30";
    public string weapon = "Pistol";
    public string shoot = "F";
    static public bool IsItFirstMessage = false;
    static public Dictionary<string, Dictionary<string, string>> dasha = new Dictionary<string, Dictionary<string, string>>();
    static public Dictionary<string, string> clientData = new Dictionary<string, string>();
    public string mess;
    public GameObject gun;
    public GameObject shotgun;
    public GameObject bomb;
    public GameObject pistol;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start () {
        clientTCP.Connect();//коннект с сервером                                     конект

        clientData.Add("id", "-1"); clientData.Add("pos_x", ""); clientData.Add("pos_y", ""); clientData.Add("pos_z", "");//позиция игрока
        clientData.Add("rot_x", ""); clientData.Add("rot_y", ""); clientData.Add("rot_z", "");//вращение игрока
        clientData.Add("life", ""); clientData.Add("dir", ""); clientData.Add("shoot", ""); clientData.Add("weapon", "");

        clientTCP.SendFirstMessage(clientData);//отправка первого сообщения серверу               отправляю сообщение
        mess = clientTCP.GetPos();//данные с сервера                                 принимаю сообщение с сервера
        InstantiatePlayer(Convert.ToString(my_ID), mess);//создание меня             создаю себя
        IsItFirstMessage = true;
    }

    private void Update()
    {
        if (IsItFirstMessage)
        {
                clientTCP.Send(my_ID, mess, Dir, life,  shoot, weapon, clientData);
                mess = clientTCP.GetPos();//данные с сервера  
                var jsonData1 = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(mess);
                dasha = jsonData1;
                Actoin();//метод отслеживающий нажатие клавишь 
                MovePlayer(my_ID, mess);//мое движение
                if (dasha.Count > 1)//для других играков
                {
                    InstantiateOther(my_ID, dasha);//создание других играков
                    InstantiateBulletOther();//стрельба других играков
                    MoveOther(my_ID, mess);//движение других играков
                }
        }
    }
    public void Actoin()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            Dir = "W";
        }
        else
        {
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {

                Dir = "S";
            }
            else
            {
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                {
                    Dir = "A";
                }
                else
                {
                    if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                    {
                        Dir = "D";
                    }
                    else
                    {
                        Dir = "N";
                    }
                }
            }
        }
        if (Input.GetKey(KeyCode.F))//поднятие предметов
        {
           
        }
        if (Input.GetKey(KeyCode.R))//перезарядка оружия
        {

        }
        weaponPref = GameObject.FindGameObjectWithTag("Weapon");

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Vector3 v = weaponPref.transform.position;
            Destroy(weaponPref);
            weaponPref = Instantiate(pistol, v, Quaternion.identity);
            weapon = "Pistol";
            CountBul.text = dasha[my_ID]["bulP"];
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Vector3 v = weaponPref.transform.position;
            Destroy(weaponPref);
            weaponPref = Instantiate(shotgun, v, Quaternion.identity);
            weapon = "Shotgun";
            CountBul.text = dasha[my_ID]["bulS"];
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Vector3 v = weaponPref.transform.position;
            Destroy(weaponPref);
            weaponPref = Instantiate(gun, v, Quaternion.identity);
            weapon = "Gun";
            CountBul.text = dasha[my_ID]["bulG"];
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Vector3 v = weaponPref.transform.position;
            Destroy(weaponPref);
            weaponPref = Instantiate(bomb, v, Quaternion.identity);
            weapon = "Bomb";
            Debug.Log("ID: " + my_ID);
            Debug.Log(dasha[my_ID]["bulB"]);
            CountBul.text = dasha[my_ID]["bulB"];
        }
        if (Input.GetKeyDown(KeyCode.Q))
        { 
           
            //InstantiateBullet();
            switch (dasha[my_ID]["weapon"])
            {
                case "Pistol":
                    {
                        if (Convert.ToInt32(dasha[my_ID]["bulP"]) > 0)
                        { InstantiateBullet(); Debug.Log("PisB: " + Convert.ToInt32(dasha[my_ID]["bulP"])); shoot = "T"; }
                        break;
                    }
                case "Shotgun":
                    {
                        if (Convert.ToInt32(dasha[my_ID]["bulS"]) > 0)
                        { InstantiateBullet(); shoot = "T"; }
                        break;
                    }
                case "Gun":
                    {
                        if (Convert.ToInt32(dasha[my_ID]["bulG"]) > 0)
                        { InstantiateBullet(); shoot = "T"; }
                        break;
                    }
                case "Bomb":
                    {
                        if (Convert.ToInt32(dasha[my_ID]["bulB"]) > 0)
                        { InstantiateBullet(); shoot = "T"; }
                        break;
                    }
            }

            Debug.Log("SHOOT: " + shoot);
        }
        else
        {
            shoot = "F";
        }
        switch(dasha[my_ID]["weapon"])
        {
            case "Pistol": { CountBul.text = dasha[my_ID]["bulP"]; break; }
            case "Shotgun": { CountBul.text = dasha[my_ID]["bulS"]; break; }
            case "Gun": { CountBul.text = dasha[my_ID]["bulG"]; break; }
            case "Bomb": { CountBul.text = dasha[my_ID]["bulB"]; break; }
        }

    } 
    public void InstantiateBullet()
    {
        Vector3 v = new Vector3(Convert.ToSingle(dasha[my_ID]["pos_x"])+0.8f, Convert.ToSingle(dasha[my_ID]["pos_y"]), Convert.ToSingle(dasha[my_ID]["pos_z"]));
        cur = GameObject.Instantiate(bulletPref, v, bulletPref.transform.rotation) as GameObject;
          
    }
    public void InstantiateBulletOther()  
    {
        foreach (string s in dasha.Keys)
        {
            if (s != my_ID)
            {
                if (dasha[s]["shoot"] == "T")
                {
                    Vector3 v = new Vector3(Convert.ToSingle(dasha[s]["pos_x"]) + 0.8f, Convert.ToSingle(dasha[s]["pos_y"]), Convert.ToSingle(dasha[s]["pos_z"]));
                    cur = GameObject.Instantiate(bulletPref, v, bulletPref.transform.rotation) as GameObject;
                }
            }
        }
    }
    public void InstantiatePlayer(string ID, string str)//Создание игрока
    {
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
            int life = Convert.ToInt32(jsonData1[ID]["life"]);
            Vector3 v = new Vector3(x, y, z);
            GameObject temp = Instantiate(playerPref, v, Quaternion.identity);

            //создание оружия
            //float xW = Convert.ToSingle(jsonData1[ID]["pos_xW"]);
            //Vector3 v2 = new Vector3(xW, y, z);
            //GameObject temp2 = Instantiate(weaponPref, v2, Quaternion.identity);
            //temp2.transform.rotation = Quaternion.Euler(rotX, rotY, rotZ);
            //temp2.name = ID + ":" + dasha[ID]["weapon"];

            temp.transform.rotation = Quaternion.Euler(rotX, rotY, rotZ);
            temp.name = Convert.ToString(id);
            if (!playerList.ContainsKey(id))
            playerList.Add(id, temp);
            Lifes.text = Convert.ToString(life);
            CountBul.text = jsonData1[ID]["bulP"];
        }
    }
    public void InstantiateOther(string ID, Dictionary<string, Dictionary<string, string>> str)//Создание других играков
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

               // создание оружий других игроков
                //float xW = Convert.ToSingle(str[s]["pos_xW"]);
                //Vector3 v2 = new Vector3(xW, y, z);
                //GameObject temp2 = Instantiate(weaponPref, v2, Quaternion.identity);
                //temp2.transform.rotation = Quaternion.Euler(rotX, rotY, rotZ);
                //temp2.name = s + ":" + dasha[s]["weapon"];

                Vector3 v = new Vector3(x, y, z);
                GameObject temp = Instantiate(playerPref, v, Quaternion.identity);
                temp.transform.rotation = Quaternion.Euler(rotX, rotY, rotZ);
                temp.name = Convert.ToString(s);
                playerList.Add(Convert.ToInt32(s), temp);
            }
        }
    }
    public void MoveOther(string ID, string str)//движение других играков
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
    public void MovePlayer(string ID, string str)//двжение игрока
    {
        var jsonData1 = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(str);
        if (jsonData1.Count>1)
        {
            InstantiateOther(ID, jsonData1);
        }
        GameObject player = GameObject.Find(ID);
        float x = Convert.ToSingle(jsonData1[ID]["pos_x"]);
        float y = Convert.ToSingle(jsonData1[ID]["pos_y"]);
        float z = Convert.ToSingle(jsonData1[ID]["pos_z"]);
        float rotX = Convert.ToSingle(jsonData1[ID]["rot_x"]);
        float rotY = Convert.ToSingle(jsonData1[ID]["rot_y"]);
        float rotZ = Convert.ToSingle(jsonData1[ID]["rot_z"]);
        Vector3 v = new Vector3(x, y, z);
        player.transform.position = v;
        player.transform.rotation = Quaternion.Euler(rotX, rotY, rotZ);

        GameObject weapon = GameObject.Find(ID+":"+dasha[ID]["weapon"]);


    }
      
}
