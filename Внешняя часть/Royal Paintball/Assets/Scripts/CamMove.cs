using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class MyClass
{
    public int level;
    public float timeElapsed;
    public string playerName;
}
public class CamMove : MonoBehaviour {

     public GameObject player;
    private Vector3 offset;

     GameObject weapon;
    public GameObject gun;
    public GameObject shotgun;
    public GameObject bomb;
    public GameObject pistol;
    NetworkManager net = new NetworkManager();

    private void Awake()
    {
         //player = GameObject.FindGameObjectWithTag("Player");
       //player = GameObject.Find(Convert.ToString(net.my_ID));
    }
    void Start()
    {
       // player = GameObject.Find("Player: 0");
     //  offset = this.transform.position - player.transform.position;
    }
    void LateUpdate()
    {
        //player = GameObject.Find("Player: 0");
        //player = GameObject.FindGameObjectWithTag("Player");
       // player = GameObject.Find(Convert.ToString(net.my_ID));
       // this.transform.position = player.transform.position + offset;
    }
    private void Update()
    {
        

        weapon = GameObject.FindGameObjectWithTag("Weapon");
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Vector3 v = weapon.transform.position;
            Destroy(weapon);
            weapon = Instantiate(pistol, v, Quaternion.identity);
            net.weapon = "Pistol";
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Vector3 v = weapon.transform.position;
            Destroy(weapon);
            weapon = Instantiate(shotgun, v, Quaternion.identity);
            net.weapon = "Shotgun";
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Vector3 v = weapon.transform.position;
            Destroy(weapon);
            weapon = Instantiate(gun, v, Quaternion.identity);
            net.weapon = "Gun";
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Vector3 v = weapon.transform.position;
            Destroy(weapon);
            weapon = Instantiate(bomb, v, Quaternion.identity);
            net.weapon = "Bomb";
        }
    }
}
