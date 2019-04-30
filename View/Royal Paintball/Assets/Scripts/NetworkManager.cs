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
    public GameObject treePref;
    public GameObject wallPref;
    public GameObject circlePref;
    public GameObject cur;
    public Dictionary<int, GameObject> playerList = new Dictionary<int, GameObject>();
    public Dictionary<int, GameObject> bulletList = new Dictionary<int, GameObject>();
    public Dictionary<int, GameObject> treeList = new Dictionary<int, GameObject>();
    public Dictionary<int, GameObject> wallList = new Dictionary<int, GameObject>();
    public Dictionary<int, GameObject> itemList = new Dictionary<int, GameObject>();
    public Text Lifes;
    public Text CountBul;
    public Text Magazine;
    public Text Timer;
    public int my_ID = 111;

    public string weapon = "Pistol";

    static public bool IsItFirstMessage = false;
    static public Dictionary<string, Dictionary<string, string>> dasha = new Dictionary<string, Dictionary<string, string>>();
    static public Dictionary<string, string> clientData = new Dictionary<string, string>();
    public string mess;
    public GameObject gun;
    public GameObject shotgun;
    public GameObject bomb;
    public GameObject pistol;
    public GameObject wall;
    public GameObject magazineP;
    public GameObject magazineS;
    public GameObject magazineG;
    public GameObject magazineB;

    public Player player = new Player();
    public Field field = new Field();

    private void Awake()
    {
        if (GameObject.FindGameObjectsWithTag("NetworkManager").Length == 1)
            DontDestroyOnLoad(this);
        else
            Destroy(this);

    }

    void Start() {
        clientTCP.Connect();//коннект с сервером                                     

        player.ID = -1;
        clientTCP.SendFirstMessage(player);//отправка первого сообщения серверу            

        mess = clientTCP.GetPos();//данные с сервера  
        
        Field jsonData1 = JsonConvert.DeserializeObject<Field>(mess);
        field = jsonData1;

        MyId();
        InstantiatePlayer();//создание играков
        InstantiateTree();
        InstantiateWall();
        InstantiateCircle();

        IsItFirstMessage = true;
    }

    private void Update()
    {

        if (IsItFirstMessage)
        {
            clientTCP.Send(field.Player[my_ID]);
            mess = clientTCP.GetPos();//данные с сервера  

          Field jsonData1 = JsonConvert.DeserializeObject<Field>(mess); ;
            field = jsonData1;
            DelBull();
            if (field.Bullet.Count>0)
            {
                MoveBull();
            }
            if (field.Player.Count != 0)
            {
                Actoin();//метод отслеживающий нажатие клавишь 
                InstantiatePlayer();
                MovePlayer();//движение
          
                if (field.Player.Count > 1)//для других играков
                {
                    InstantiateWeaponOther();
                    InstantiateBulletOther();//стрельба других играков
                }
            }
        }
        Debug.Log("Plauerlist: " + playerList.Count);
    }
    public void DelBull()
    {
        if (field.Bullet.Count < bulletList.Count)
        {
            while (field.Bullet.Count != bulletList.Count)
            {
                GameObject bul = bulletList[bulletList.Count - 1];
                bulletList.Remove(bulletList.Count - 1);
                Destroy(bul);
            }
        }
    }
    public void KeyMoveDown()//движение пользователя
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            field.Player[my_ID].Direction = "W";
        }
        else
        {
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {

                field.Player[my_ID].Direction = "S";
            }
            else
            {
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                {
                    field.Player[my_ID].Direction = "A";
                }
                else
                {
                    if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                    {
                        field.Player[my_ID].Direction = "D";
                    }
                    else
                    {
                        field.Player[my_ID].Direction = "N";
                    }
                }
            }
        }

    }
    public void Actoin()//изменение данных, отправляемых на сервер при действии пользователя
    {
        
        KeyMoveDown();
        if (Input.GetKey(KeyCode.F))//поднятие предметов
        {
            field.Player[my_ID].LiftItem = true;
        }
        if (Input.GetKey(KeyCode.R))//перезарядка оружия
        {
            field.Player[my_ID].Reload = true;
        }
        else
        {
            field.Player[my_ID].Reload = false;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            field.Player[my_ID].Weapon = "Pistol";
            InstantiateWeapon(pistol, field.Player[my_ID].Weapon);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            field.Player[my_ID].Weapon = "Shotgun";
            InstantiateWeapon(shotgun, field.Player[my_ID].Weapon);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            field.Player[my_ID].Weapon = "Gun";
            InstantiateWeapon(gun, field.Player[my_ID].Weapon);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            field.Player[my_ID].Weapon = "Bomb";
            InstantiateWeapon(bomb, field.Player[my_ID].Weapon);
        }
         CountBul.text = Convert.ToString(field.Player[my_ID].Weap.CountBullets);
         Magazine.text = Convert.ToString(field.Player[my_ID].Weap.CountMagazine);
        Lifes.text = Convert.ToString(field.Player[my_ID].Life);


        if (field.Player[my_ID].Life== 0)
        {
            InstantiateMagazine(my_ID);

        }
        MousePos();
        SizeCircle();
        
    }
    private void OnMouseUp()
    {
        field.Player[my_ID].Shoot = false;
    }
    private void OnMouseDrag()
    {
        field.Player[my_ID].Shoot = false;
    }
    void OnMouseDown()//нажатие мыши для стрельбы
    {
        if (field.Player[my_ID].Weap.CountBullets > 0)
        {
            field.Player[my_ID].Shoot = true;
            InstantiateBullet();
        }
        
    }
   
    public void MousePos()
    {
        var mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition); //положение мыши из экранных в мировые координаты
        field.Player[my_ID].MousePos[0] = mousePosition.x;
        field.Player[my_ID].MousePos[1] = mousePosition.y;
        field.Player[my_ID].MousePos[2] = mousePosition.z;
    }

    public void InstantiateWeapon(GameObject weap, string nameWeap)
    {
        GameObject Player = GameObject.Find(Convert.ToString(my_ID));
        Transform w = Player.transform.GetChild(0);
        weaponPref = w.gameObject;
        Vector2 v = w.position;
        Destroy(weaponPref);
        weaponPref = Instantiate(weap, v, Quaternion.identity);
        weapon = nameWeap;
        weaponPref.transform.parent = GameObject.Find(Convert.ToString(my_ID)).transform;
    }
    public void InstantiateWeaponOther()
    {
        GameObject weap = pistol;
        foreach (int c in field.Player.Keys)
        {
            if (field.Player[c].ID != my_ID)
            {
                string nameWeap = field.Player[c].Weapon;
                switch (nameWeap)
                {

                    case "Pistol":
                        {
                            field.Player[c].Weap = field.Player[c].P;
                            weap = pistol; break;
                        }
                    case "Shotgun":
                        {
                            field.Player[c].Weap = field.Player[c].S;
                            weap = shotgun;
                            break;
                        }
                    case "Gun":
                        {
                            field.Player[c].Weap = field.Player[c].G;
                            weap = gun;
                            break;
                        }
                    case "Bomb":
                        {
                            field.Player[c].Weap = field.Player[c].B;
                            weap = bomb;
                            break;
                        }
                }

                GameObject Player = GameObject.Find(Convert.ToString(field.Player[c].ID));
                weaponPref = Player.transform.GetChild(0).gameObject;
                Vector2 v = weaponPref.transform.position;
                Destroy(weaponPref);
                weaponPref = Instantiate(weap, v, Quaternion.identity);
                weapon = nameWeap;
                weaponPref.transform.parent = GameObject.Find(Convert.ToString(field.Player[c].ID)).transform;
            }
        }

    }

    public void InstantiateTree()
    {
        for (int i = 0; i < field.Tree.Count; i++)
        {
            Vector2 vec = new Vector2(field.Tree[i].X, field.Tree[i].Y);
            GameObject temp = Instantiate(treePref, vec, Quaternion.identity);
            temp.transform.rotation = Quaternion.Euler(-90, 0, 0);
            treeList.Add(i, temp);
        }
    }
    public void InstantiateWall()
    {
        for (int i = 0; i < field.Wall.Count; i++)
        {
            Vector2 vec = new Vector2(field.Wall[i].X, field.Wall[i].Y);
            GameObject temp = Instantiate(wallPref, vec, Quaternion.identity);
            temp.transform.rotation = Quaternion.Euler(-90, 0, 0);
       //     treeList.Add(i, temp);
        }

    }
   
    public void InstantiateBullet()
    {
            Vector2 v = new Vector2(field.Player[my_ID].X + 1.2f, field.Player[my_ID].Y);
            var pos = Input.mousePosition;
            pos = Camera.main.ScreenToWorldPoint(pos);

            field.Player[my_ID].End = new float[2] { pos.x, pos.y };
            field.Player[my_ID].Start = new float[2] { v.x, v.y };

            cur = GameObject.Instantiate(bulletPref, v, bulletPref.transform.rotation) as GameObject;
        cur.name = Convert.ToString(bulletList.Count);
        bulletList.Add(bulletList.Count,cur);
    }
    public void InstantiateBulletOther()
    {
        foreach (int c in field.Player.Keys)
        {
            if (field.Player[c].ID != my_ID)
            {
                if (field.Player[c].Shoot == true)
                {
                    Vector2 v = new Vector2(field.Player[c].X + 0.8f, field.Player[c].Y);
                    cur = GameObject.Instantiate(bulletPref, v, bulletPref.transform.rotation) as GameObject;
                   // bulletList.Add(bulletList.Count, cur);
                }
            }
        }
    }
    public void InstantiateMagazine(int ID)
    {
        if(field.Item.Count>0)
        {
            for(int i=0;i<field.Item.Count;i++)
            {
                switch (field.Item[i].Name)
                {
                    case "Pistol": {
                            Vector2 v = new Vector2(player.X, player.Y+1.5f);
                            GameObject temp = Instantiate(magazineP, v, Quaternion.identity);
                            temp.transform.rotation = Quaternion.Euler(player.XRot, player.YRot, 0);
                            temp.name = field.Item[i].Name;
                            itemList.Add(field.Item[i].Count, temp);
                            break; }
                    case "Shotgun":
                        {
                            Vector2 v = new Vector2(player.X, player.Y - 1.5f);
                            GameObject temp = Instantiate(magazineS, v, Quaternion.identity);
                            temp.transform.rotation = Quaternion.Euler(player.XRot, player.YRot, 0);
                            temp.name = field.Item[i].Name;
                            itemList.Add(field.Item[i].Count, temp);
                            break;
                        }
                    case "Gun":
                        {
                            Vector2 v = new Vector2(player.X - 1.5f, player.Y);
                            GameObject temp = Instantiate(magazineG, v, Quaternion.identity);
                            temp.transform.rotation = Quaternion.Euler(player.XRot, player.YRot, 0);
                            temp.name = field.Item[i].Name;
                            itemList.Add(field.Item[i].Count, temp);
                            break;
                        }
                    case "Bomb":
                        {
                            Vector2 v = new Vector2(player.X + 1.5f, player.Y);
                            GameObject temp = Instantiate(magazineB, v, Quaternion.identity);
                            temp.transform.rotation = Quaternion.Euler(player.XRot, player.YRot, 0);
                            temp.name = field.Item[i].Name;
                            itemList.Add(field.Item[i].Count, temp);
                            break;
                        }
                }
            }
           
        }
        if (field.Player[my_ID].Weap.CountBullets > 0)
        {
            Vector2 v = new Vector2(field.Player[my_ID].X, field.Player[my_ID].Y + 1F);
            GameObject mag = GameObject.Instantiate(magazineP, v, bulletPref.transform.rotation) as GameObject;
            mag.name = Convert.ToString(field.Player[my_ID].Weap.CountBullets);
        }
    }
   
    public void InstantiateCircle()//Создание игрока
    {
        Vector2 v = new Vector2(field.circle.X,field.circle.Y);
        GameObject temp = Instantiate(circlePref, v, Quaternion.identity);

        temp.transform.rotation = Quaternion.Euler(90,180, 0);
        temp.transform.localScale = new Vector3(field.circle.Size[0],1, field.circle.Size[1]);

    }
    public void SizeCircle()
    {
       GameObject temp = GameObject.FindGameObjectWithTag("Circle");
        temp.transform.localScale = new Vector3(field.circle.Size[0], 1, field.circle.Size[1]);
    }

   public void MyId()
    {
        //Field jsonData1 = JsonConvert.DeserializeObject<Field>(mess);
        //field = jsonData1;
       //Debug.Log(field.Player.Count);
        foreach (int c in field.Player.Keys)
        {
            my_ID = field.Player[c].ID;
        }
    }
    public void InstantiatePlayer()//Создание игроков
    {
        foreach (int c in field.Player.Keys)
        {
            if (!playerList.ContainsKey(c))
            {
                player = field.Player[c];
                Vector2 v = new Vector2(player.X, player.Y);
                GameObject temp = Instantiate(playerPref, v, Quaternion.identity);

                temp.transform.rotation = Quaternion.Euler(player.XRot, player.YRot, 0);
                temp.name = Convert.ToString(c);
                Lifes.text = Convert.ToString(player.Life);
                playerList.Add(c, temp);
            }
        }
    }
    
    public void MovePlayer()//двжение игрока
    {

            foreach (int c in field.Player.Keys)
            {

                GameObject player = GameObject.Find(Convert.ToString(field.Player[c].ID));
                Vector2 v = new Vector2(field.Player[c].X, field.Player[c].Y);
                player.transform.position = v;
                player.transform.rotation = Quaternion.Euler(field.Player[c].XRot, field.Player[c].YRot, 0);

            }
        
    }
    public void MoveBull()
    {
        if (playerList.Count==1)
     //   for (int i = 0; i < bulletList.Count; i++)
     foreach(int i in bulletList.Keys)
        {
                Vector2 v = new Vector2(field.Bullet[i].X, field.Bullet[i].Y);
                Debug.Log(bulletList.Count);
                bulletList[i].transform.position = v;
        }
        else
        {
            //for (int i = 0; i < field.Bullet.Count; i++)
            foreach(Bullet bul in field.Bullet)
            {
                foreach (int j in bulletList.Keys)
                {
                    //Vector2 v = new Vector2(field.Bullet[i].X, field.Bullet[i].Y);
                    Vector2 v = new Vector2(bul.X,bul.Y);
                    bulletList[j].transform.position = v;

                }
            }
        }
    }
}
