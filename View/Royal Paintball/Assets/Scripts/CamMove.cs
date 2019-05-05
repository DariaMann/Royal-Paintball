using UnityEngine;

public class CamMove : MonoBehaviour {

     public GameObject player;
    private Vector3 offset;
    private Vector3 comPos;

    void Start()
    {
       offset = this.transform.position - player.transform.position;
    }
    void LateUpdate()
    {
        try
            {
            player = GameObject.FindGameObjectWithTag("Player");
            this.transform.position = player.transform.position + offset;
        }
        catch { }
    }
}
