using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMove : MonoBehaviour {

     public GameObject player;
    private Vector3 offset;

     GameObject weapon;
    public GameObject gun;
    public GameObject shotgun;
    public GameObject bomb;
    public GameObject pistol;

    private void Awake()
    {
         //player = GameObject.FindGameObjectWithTag("Player");
       // player = GameObject.Find("Player: 0");
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

        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Vector3 v = weapon.transform.position;
            Destroy(weapon);
            weapon = Instantiate(shotgun, v, Quaternion.identity);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Vector3 v = weapon.transform.position;
            Destroy(weapon);
            weapon = Instantiate(gun, v, Quaternion.identity);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Vector3 v = weapon.transform.position;
            Destroy(weapon);
            weapon = Instantiate(bomb, v, Quaternion.identity);
        }
    }
}
