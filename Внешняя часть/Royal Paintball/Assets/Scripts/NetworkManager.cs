using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

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
    public Text Magazine;
    public string my_ID = "111";

    public string Dir = "N";
    
    public string weapon = "Pistol";
    public string shoot = "F";
    public string wound = "F";
    public string liftItem = "F";
    public string reload = "F";
    static public bool IsItFirstMessage = false;
    static public Dictionary<string, Dictionary<string, string>> dasha = new Dictionary<string, Dictionary<string, string>>();
    static public Dictionary<string, string> clientData = new Dictionary<string, string>();
    public string mess;
    public GameObject gun;
    public GameObject shotgun;
    public GameObject bomb;
    public GameObject pistol;

    public GameObject magazineP;
    public GameObject magazineS;
    public GameObject magazineG;
    public GameObject magazineB;
    List<GameObject> countMag = new List<GameObject>();

    public string mousePosX = "N";
    public string mousePosY = "N";
    public string mousePosZ = "N";

    public string[] startPos = new string[3] {"N","N","N"};
    public string[] endPos = new string[3] { "N", "N", "N" };


    public bool TheEnd=false;


    private void Awake()
    {
        if (GameObject.FindGameObjectsWithTag("NetworkManager").Length == 1 )
            DontDestroyOnLoad(this);
        else
            Destroy(this);
       
    }

    void Start () {
        clientTCP.Connect();//коннект с сервером                                     конект

        clientData.Add("id", "-1"); //clientData.Add("pos_x", ""); clientData.Add("pos_y", ""); clientData.Add("pos_z", "");//позиция игрока
        //clientData.Add("rot_x", ""); clientData.Add("rot_y", ""); clientData.Add("rot_z", "");//вращение игрока
        //clientData.Add("life", ""); clientData.Add("dir", ""); clientData.Add("shoot", ""); clientData.Add("weapon", "");
        //clientData.Add("mousePosX", ""); 

        clientTCP.SendFirstMessage(clientData);//отправка первого сообщения серверу               отправляю сообщение
        mess = clientTCP.GetPos();//данные с сервера                                 принимаю сообщение с сервера
        InstantiatePlayer(Convert.ToString(my_ID), mess);//создание меня             создаю себя

        startPos = new string[3] { "N", "N", "N" };
        endPos = new string[3] { "N", "N", "N" };
        IsItFirstMessage = true;
    }

    private void Update()
    {
        if (IsItFirstMessage)
        {
          clientTCP.Send(my_ID, mess, Dir, shoot, weapon, wound,liftItem, reload, clientData,startPos,endPos);
                mess = clientTCP.GetPos();//данные с сервера  
                var jsonData1 = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(mess);
                dasha = jsonData1;
                if (dasha.Count != 0)
                {
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
    }
    public void KeyMoveDown()
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

    }
    public void Actoin()
    {
        KeyMoveDown();
        if (Input.GetKey(KeyCode.F))//поднятие предметов
            {
            liftItem = "T";
            }
            if (Input.GetKey(KeyCode.R))//перезарядка оружия
            {
            reload = "T";
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
            InstantiateWeapon(pistol, dasha[my_ID]["bulP"],"Pistol");
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
            InstantiateWeapon(shotgun, dasha[my_ID]["bulS"],"Shotgun");
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
            InstantiateWeapon(gun, dasha[my_ID]["bulG"],"Gun");
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
            InstantiateWeapon(bomb, dasha[my_ID]["bulB"],"Bomb");
            }
            if (Input.GetKeyDown(KeyCode.Q))//выстрел
            {
                switch (dasha[my_ID]["weapon"])
                {
                    case "Pistol":
                        {
                            if (Convert.ToInt32(dasha[my_ID]["bulP"]) > 0)
                            { InstantiateBullet(); shoot = "T"; }
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
            var mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition); //положение мыши из экранных в мировые координаты

            mousePosX = Convert.ToString(mousePosition.x);
            mousePosY = Convert.ToString(mousePosition.y);
            mousePosZ = Convert.ToString(mousePosition.z);

            Debug.Log("SHOOT: " + shoot);


            }
            else
            {
                shoot = "F";
            }
            switch (weapon)//обновление количества пуль
            {
                case "Pistol": { CountBul.text = dasha[my_ID]["bulP"]; Magazine.text = dasha[my_ID]["magazineP"]; break; }
                case "Shotgun": { CountBul.text = dasha[my_ID]["bulS"]; Magazine.text = dasha[my_ID]["magazineS"]; break; }
                case "Gun": { CountBul.text = dasha[my_ID]["bulG"]; Magazine.text = dasha[my_ID]["magazineG"]; break; }
                case "Bomb": { CountBul.text = dasha[my_ID]["bulB"]; Magazine.text = dasha[my_ID]["magazineB"]; break; }
            }
        if (Input.GetKey(KeyCode.Z))//попадание в меня
        {
            if (Convert.ToInt32(dasha[my_ID]["life"]) >= 0)
            {
                wound = "T";
                Lifes.text = dasha[my_ID]["life"];
            }
        }
        else
        {
            wound = "F";
        }
        if(dasha[my_ID]["life"]=="0")
        {
            InstantiateMagazine();
                

        }

    } 
    public void InstantiateWeapon(GameObject weap,string countBul, string nameWeap)
    {
            weaponPref = GameObject.FindGameObjectWithTag("Weapon");
        
            Vector3 v = weaponPref.transform.position;
            Destroy(weaponPref);
            weaponPref = Instantiate(weap, v, Quaternion.identity);
            weapon = nameWeap;
            weaponPref.transform.parent = GameObject.Find(my_ID).transform;
        CountBul.text = countBul;//dasha[my_ID]["bulP"];


    }
    public void InstantiateBullet()
    {
        
            Vector3 v = new Vector3(Convert.ToSingle(dasha[my_ID]["pos_x"]) + 0.8f, Convert.ToSingle(dasha[my_ID]["pos_y"]), Convert.ToSingle(dasha[my_ID]["pos_z"]));
       // Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));

        var pos = Input.mousePosition;
        pos = Camera.main.ScreenToWorldPoint(pos);

        startPos = new string[3] { Convert.ToString(pos.x), Convert.ToString(pos.y) , Convert.ToString(pos.z) };
        endPos = new string[3] { Convert.ToString(v.x), Convert.ToString(v.y), Convert.ToString(v.z) };

        cur = GameObject.Instantiate(bulletPref, v, Quaternion.LookRotation(pos)/* bulletPref.transform.rotation*/) as GameObject;

    
    }
    public void InstantiateMagazine()
    {
        switch(dasha[my_ID]["weapon"])
        {
            case "Pistol":
                {
                    if (Convert.ToInt32(dasha[my_ID]["bulP"]) > 0)
                    {
                        Vector3 v = new Vector3(Convert.ToSingle(dasha[my_ID]["pos_x"]), Convert.ToSingle(dasha[my_ID]["pos_y"]) + 1F, Convert.ToSingle(dasha[my_ID]["pos_z"]));
                        GameObject mag = GameObject.Instantiate(magazineP, v, bulletPref.transform.rotation) as GameObject;
                        mag.name = dasha[my_ID]["bulP"];
                        countMag.Add(mag);
                    }
                    break;
                }
            case "Shotgun":
                {
                    if (Convert.ToInt32(dasha[my_ID]["bulS"]) > 0)
                    {
                        Vector3 v = new Vector3(Convert.ToSingle(dasha[my_ID]["pos_x"]), Convert.ToSingle(dasha[my_ID]["pos_y"]) - 1F, Convert.ToSingle(dasha[my_ID]["pos_z"]));
                        GameObject mag = GameObject.Instantiate(magazineS, v, bulletPref.transform.rotation) as GameObject;
                        mag.name = dasha[my_ID]["bulS"];
                        countMag.Add(mag);
                    }
                    break;
                }
            case "Gun":
                {
                    if (Convert.ToInt32(dasha[my_ID]["bulG"]) > 0)
                    {
                        Vector3 v = new Vector3(Convert.ToSingle(dasha[my_ID]["pos_x"]) + 1F, Convert.ToSingle(dasha[my_ID]["pos_y"]), Convert.ToSingle(dasha[my_ID]["pos_z"]));
                        GameObject mag = GameObject.Instantiate(magazineG, v, bulletPref.transform.rotation) as GameObject;
                        mag.name = dasha[my_ID]["bulG"];
                        countMag.Add(mag);
                    }
                    break;
                }
            case "Bomb":
                {
                    if (Convert.ToInt32(dasha[my_ID]["bulB"]) > 0)
                    {
                        Vector3 v = new Vector3(Convert.ToSingle(dasha[my_ID]["pos_x"]) - 1F, Convert.ToSingle(dasha[my_ID]["pos_y"]) + 1F, Convert.ToSingle(dasha[my_ID]["pos_z"]));
                        GameObject mag = GameObject.Instantiate(magazineB, v, bulletPref.transform.rotation) as GameObject;
                        mag.name = dasha[my_ID]["bulB"];
                        countMag.Add(mag);
                    }
                    break;
                }
        }
       
    }
    public void InstantiateMagazineOther()
    {

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

        //GameObject weapon = GameObject.Find(ID+":"+dasha[ID]["weapon"]);


    }
      
}
