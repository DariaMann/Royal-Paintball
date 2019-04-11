using UnityEngine;

public class CamMove : MonoBehaviour {

     public GameObject player;
    private Vector3 offset;
    
    void Start()
    {
       // player = GameObject.FindGameObjectWithTag("Weapon");
        //  player = GameObject.Find(net.my_ID);
        //Debug.Log("Name: " + net.my_ID);
      //  offset = this.transform.position - player.transform.position;
    }
    void LateUpdate()
    {
     //   player = GameObject.FindGameObjectWithTag("Weapon");
        //player = GameObject.Find("Player: 0");
        //player = GameObject.FindGameObjectWithTag("Player");
        //  player = GameObject.Find(net.my_ID);
        // Debug.Log("Name: " + net.my_ID);
        ////Debug.Log("Name: " + net.my_ID);
      //  this.transform.position = player.transform.position + offset;
    }
}
