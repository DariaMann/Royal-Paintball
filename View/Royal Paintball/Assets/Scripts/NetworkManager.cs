using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviour
{

    private ClientTCP clientTCP = new ClientTCP();
    [SerializeField]
    public GameObject playerPref;
    public GameObject weaponPref;
    public GameObject bulletPref;
    public GameObject OakPref;
    public GameObject FirPref;
    public GameObject PoplarPref;
    public GameObject wallPref;
    public GameObject circlePref;
    public GameObject camera;
    public GameObject kitPref;
    public GameObject Finish;
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
    public Text FinishMess;
    public int my_ID;


    public string weapon = "Pistol";

    private Vector3 offset;

    static public bool IsItFirstMessage = true;
    static public Dictionary<string, Dictionary<string, string>> dasha = new Dictionary<string, Dictionary<string, string>>();
    static public Dictionary<string, string> clientData = new Dictionary<string, string>();
    string mess;
    public GameObject gun;
    public GameObject shotgun;
    public GameObject bomb;
    public GameObject pistol;
    public GameObject wall;
    public GameObject magazineP;
    public GameObject magazineS;
    public GameObject magazineG;
    public GameObject magazineB;

    public Image arrow;

    public Player player = new Player();
    public Field field = new Field();

    public Color[] Colors = new Color[8];
    public void Exit()
    {
        Destroy(playerList[my_ID]);
        playerList.Remove(my_ID);
        Debug.Log("Лег сервер");
        clientTCP.Disconnect();
        SceneManager.LoadScene("Play");

        //SceneManager.UnloadScene();
        Application.Quit();
    }
    public void Menu()
    {
        SceneManager.LoadScene("Play");
    }
    void Start()
    {
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

        offset = camera.transform.position - playerList[my_ID].transform.position;

        IsItFirstMessage = false;
    }
    private void Update()
    {

        if (!IsItFirstMessage)
        {
            if (field.Player.ContainsKey(my_ID))
            {
                clientTCP.Send(field.Player[my_ID]);
                mess = clientTCP.GetPos();//данные с сервера  

                Field jsonData1 = JsonConvert.DeserializeObject<Field>(mess);
                field = jsonData1;
            }
            CamMove();
            DelBull();
            DelPlayer();
            DelMgazine();
            if (field.Bullet.Count > 0)
            {
                InBul();
                MoveBull();
            }
            if (field.Player.Count != 0)
            {
                Actoin();//метод отслеживающий нажатие клавишь 
                InstantiatePlayer();
                MovePlayer();//движение
                PlayerRotation();
                ArrowRotation();
                InstantiateMagazine();


                if (field.Player.Count > 1)//для других играков
                {
                    InstantiateWeaponOther();
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
            field.Player[my_ID].Weap = field.Player[my_ID].P;
            InstantiateWeapon(pistol, field.Player[my_ID].Weapon);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            field.Player[my_ID].Weapon = "Shotgun";
            field.Player[my_ID].Weap = field.Player[my_ID].S;
            InstantiateWeapon(shotgun, field.Player[my_ID].Weapon);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            field.Player[my_ID].Weapon = "Gun";
            field.Player[my_ID].Weap = field.Player[my_ID].G;
            InstantiateWeapon(gun, field.Player[my_ID].Weapon);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            field.Player[my_ID].Weapon = "Bomb";
            field.Player[my_ID].Weap = field.Player[my_ID].B;
            InstantiateWeapon(bomb, field.Player[my_ID].Weapon);
        }
        CountBul.text = Convert.ToString(field.Player[my_ID].Weap.CountBullets);
        Magazine.text = Convert.ToString(field.Player[my_ID].Weap.CountMagazine);
        Lifes.text = Convert.ToString(field.Player[my_ID].Life);
        Timer.text = field.time.Minutes.ToString() + ":" + field.time.Seconds.ToString();

        if (Input.GetKeyDown(KeyCode.F))
        {
            field.Player[my_ID].LiftItem = true;
        }
        else { field.Player[my_ID].LiftItem = false; }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        MousePos();
        SizeCircle();
    }

    public void PlayerRotation()
    {
        foreach (int c in field.Player.Keys)
        {
            Vector3 mousePosition = new Vector3(field.Player[c].MousePos[0], field.Player[c].MousePos[1], field.Player[c].MousePos[2]);
            var angle = Vector2.Angle(Vector2.right, mousePosition - playerList[c].transform.position);//угол между вектором от объекта к мыше и осью х
            //playerList[c].transform.eulerAngles = new Vector3(0f, 0f, playerList[c].transform.position.y < mousePosition.y ? angle : -angle);//немного магии на последок
            playerList[c].transform.eulerAngles = new Vector3(playerList[c].transform.position.y > mousePosition.y ? angle : -angle, 90f, -90f);//немного магии на последок
        }

    }
    public void ArrowRotation()
    {
        Vector3 circlePos = new Vector3(field.circle.X, field.circle.Y, 0);
        var angle = Vector2.Angle(Vector2.left, circlePos - playerList[my_ID].transform.position);//угол между вектором от объекта 
        arrow.transform.eulerAngles = new Vector3(0f, 0f, playerList[my_ID].transform.position.y > circlePos.y ? angle : -angle);//немного магии на последок
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
    public void DelPlayer()
    {
        int i = 0;
        if (field.Player.Count < playerList.Count)
        {
            while (field.Player.Count != playerList.Count)
            {
                foreach (int c in playerList.Keys)
                {
                    if (!field.Player.ContainsKey(Convert.ToInt32(playerList[c].name)))
                    {
                        int name = Convert.ToInt32(playerList[c].name);
                        GameObject pl = playerList[c];
                        i = c;
                        Destroy(pl);

                        if (name == my_ID)
                        {
                            Finish.SetActive(true);
                            clientTCP.Disconnect();
                            Destroy(this);
                        }
                    }
                }
                playerList.Remove(i);

            }
        }
    }
    public void DelMgazine()
    {
        List<int> itemIDs = new List<int>();
        foreach (int i in itemList.Keys)
        {
            itemIDs.Add(i);
        }
        foreach (int i in itemIDs)
        {
            if (!field.Item.ContainsKey(i))
            {
                GameObject item = itemList[i];
                itemList.Remove(i);
                Destroy(item);
                Debug.Log("!!!");

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
                        if (field.Player.ContainsKey(my_ID))
                            field.Player[my_ID].Direction = "N";
                    }
                }
            }
        }

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
        Transform w = Player.transform.Find("Weapon"); //Player.transform.GetChild(0);
        weaponPref = w.gameObject;
        Vector2 v = w.position;
        Destroy(weaponPref);
        weaponPref = Instantiate(weap, v, Quaternion.identity);
        weaponPref.name = "Weapon";
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
                weaponPref = Player.transform.Find("Weapon").gameObject;
                Vector2 v = weaponPref.transform.position;
                Destroy(weaponPref);
                weaponPref = Instantiate(weap, v, Quaternion.identity);
                weaponPref.name = "Weapon";
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
            switch (field.Tree[i].Type)
            {
                case "Oak":
                    {

                        GameObject temp = Instantiate(OakPref, vec, Quaternion.identity);
                        temp.transform.rotation = Quaternion.Euler(-90, 0, 0);
                        treeList.Add(i, temp);
                        break;
                    }
                case "Fir":
                    {

                        GameObject temp = Instantiate(FirPref, vec, Quaternion.identity);
                        temp.transform.rotation = Quaternion.Euler(-90, 0, 0);
                        treeList.Add(i, temp);
                        break;
                    }
                case "Poplar":
                    {

                        GameObject temp = Instantiate(PoplarPref, vec, Quaternion.identity);
                        temp.transform.rotation = Quaternion.Euler(-90, 0, 0);
                        treeList.Add(i, temp);
                        break;
                    }
            }
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
        Vector2 v = new Vector2(field.Player[my_ID].X, field.Player[my_ID].Y);
        var pos = Input.mousePosition;
        pos = Camera.main.ScreenToWorldPoint(pos);

        field.Player[my_ID].End = new float[2] { pos.x, pos.y };
        field.Player[my_ID].Start = new float[2] { v.x, v.y };
    }
    public void InBul()
    {
        if (field.Bullet.Count > 0)
        {
            for (int i = 0; i < field.Bullet.Count; i++)
            {
                if (!bulletList.ContainsKey(i))
                {
                    Vector2 v = new Vector2(field.Bullet[i].X, field.Bullet[i].Y);
                    cur = GameObject.Instantiate(bulletPref, v, bulletPref.transform.rotation) as GameObject;
                    cur.name = Convert.ToString(i);
                    Color(field.Bullet[i].Color, cur, "Bullet");
                    bulletList.Add(i, cur);
                }
            }
        }
    }
    public void InstantiateMagazine()
    {
        if (field.Item.Count > 0)
        {
            foreach (int i in field.Item.Keys)
            {
                if (!itemList.ContainsKey(i))
                {
                    switch (field.Item[i].Name)
                    {
                        case "Pistol":
                            {
                                // Vector2 v = new Vector2(player.X, player.Y + 1.5f);
                                Vector2 v = new Vector2(field.Item[i].X, field.Item[i].Y);
                                GameObject temp = Instantiate(magazineP, v, Quaternion.identity);
                                temp.transform.rotation = Quaternion.Euler(player.XRot, player.YRot, 0);
                                temp.name = field.Item[i].Name;
                                itemList.Add(i, temp);
                                break;
                            }
                        case "Shotgun":
                            {
                                Vector2 v = new Vector2(field.Item[i].X, field.Item[i].Y);
                                GameObject temp = Instantiate(magazineS, v, Quaternion.identity);
                                temp.transform.rotation = Quaternion.Euler(player.XRot, player.YRot, 0);
                                temp.name = field.Item[i].Name;
                                itemList.Add(i, temp);
                                break;
                            }
                        case "Gun":
                            {
                                Vector2 v = new Vector2(field.Item[i].X, field.Item[i].Y);
                                GameObject temp = Instantiate(magazineG, v, Quaternion.identity);
                                temp.transform.rotation = Quaternion.Euler(player.XRot, player.YRot, 0);
                                temp.name = field.Item[i].Name;
                                itemList.Add(i, temp);
                                break;
                            }
                        case "Bomb":
                            {
                                Vector2 v = new Vector2(field.Item[i].X, field.Item[i].Y);
                                GameObject temp = Instantiate(magazineB, v, Quaternion.identity);
                                temp.transform.rotation = Quaternion.Euler(player.XRot, player.YRot, 0);
                                temp.name = field.Item[i].Name;
                                itemList.Add(i, temp);
                                break;
                            }
                        case "Kit":
                            {
                                // Vector2 v = new Vector2(player.X, player.Y + 1.5f);
                                Vector2 v = new Vector2(field.Item[i].X, field.Item[i].Y);
                                GameObject temp = Instantiate(kitPref, v, Quaternion.identity);
                                temp.transform.rotation = Quaternion.Euler(player.XRot, player.YRot, 0);
                                temp.name = field.Item[i].Name;
                                itemList.Add(i, temp);
                                break;
                            }
                    }
                }
            }

        }

    }
    public void InstantiateCircle()//Создание 
    {
        Vector2 v = new Vector2(field.circle.X, field.circle.Y);
        GameObject temp = Instantiate(circlePref, v, Quaternion.identity);

        temp.transform.rotation = Quaternion.Euler(90, 180, 0);
        temp.transform.localScale = new Vector3(Convert.ToSingle(field.circle.Size[0]), 1, Convert.ToSingle(field.circle.Size[1]));

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
                Color(player.Color, temp, "Player");
                Lifes.text = Convert.ToString(player.Life);
                playerList.Add(c, temp);
            }
        }
    }

    public void SizeCircle()
    {
        GameObject temp = GameObject.FindGameObjectWithTag("Circle");
        temp.transform.localScale = new Vector3(Convert.ToSingle(field.circle.Size[0]), 1, Convert.ToSingle(field.circle.Size[1]));
        temp.transform.position = new Vector2(field.circle.X, field.circle.Y);
    }
    public void MyId()
    {
        foreach (int c in field.Player.Keys)
        {
            my_ID = field.Player[c].ID;
        }
    }
    public void Color(string color, GameObject temp, string name)
    {
        switch (color)
        {

            case "blue":
                {
                    if (name == "Player")
                        temp.transform.Find("Player").transform.GetComponent<SkinnedMeshRenderer>().materials[1].color = Colors[0];
                    else
                        temp.transform.GetComponent<Renderer>().material.color = Colors[0];
                    break;
                }
            case "red":
                {
                    if (name == "Player")
                        temp.transform.Find("Player").transform.GetComponent<SkinnedMeshRenderer>().materials[1].color = Colors[1];
                    else
                        temp.transform.GetComponent<Renderer>().material.color = Colors[1];
                    break;
                }
            case "yellow":
                {
                    if (name == "Player")
                        temp.transform.Find("Player").transform.GetComponent<SkinnedMeshRenderer>().materials[1].color = Colors[2];
                    else
                        temp.transform.GetComponent<Renderer>().material.color = Colors[2];
                    break;
                }
            case "orange":
                {
                    if (name == "Player")
                        temp.transform.Find("Player").transform.GetComponent<SkinnedMeshRenderer>().materials[1].color = Colors[3];
                    else
                        temp.transform.GetComponent<Renderer>().material.color = Colors[3];
                    break;
                }
            case "pink":
                {
                    if (name == "Player")
                        temp.transform.Find("Player").transform.GetComponent<SkinnedMeshRenderer>().materials[1].color = Colors[4];
                    else
                        temp.transform.GetComponent<Renderer>().material.color = Colors[4];
                    break;
                }
            case "green":
                {
                    if (name == "Player")
                        temp.transform.Find("Player").transform.GetComponent<SkinnedMeshRenderer>().materials[1].color = Colors[5];
                    else
                        temp.transform.GetComponent<Renderer>().material.color = Colors[5];
                    break;
                }
            case "black":
                {
                    if (name == "Player")
                        temp.transform.Find("Player").transform.GetComponent<SkinnedMeshRenderer>().materials[1].color = Colors[6];
                    else
                        temp.transform.GetComponent<Renderer>().material.color = Colors[6];
                    break;
                }
            case "white":
                {
                    if (name == "Player")
                        temp.transform.Find("Player").transform.GetComponent<SkinnedMeshRenderer>().materials[1].color = Colors[7];
                    else
                        temp.transform.GetComponent<Renderer>().material.color = Colors[7];
                    break;
                }
        }
    }
    public void CamMove()
    {
        camera.transform.position = playerList[my_ID].transform.position + offset;
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
        foreach (int i in bulletList.Keys)
        {
            Vector2 v = new Vector2(field.Bullet[i].X, field.Bullet[i].Y);

            bulletList[i].transform.position = v;
        }
    }
}
