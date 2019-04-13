using UnityEngine;

public class CamMove : MonoBehaviour {

     public GameObject player;
    private Vector3 offset;
    
    void Start()
    {
       offset = this.transform.position - player.transform.position;
    }
    void LateUpdate()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        this.transform.position = player.transform.position + offset;
    }
}
