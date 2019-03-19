using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public GameObject decal;
    public GameObject cur;
    public int count;
    public Vector3 lastPos;
    public int Speed;
    public GameObject Player;

    Vector3 target;

    private void Start()
    {
        lastPos = transform.position;
    }
    public void SetTarget(Vector3 MousePos)
    {
        target = MousePos;
    }
    private void Move()
    {
        
    }
    void Update () {
        var mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition); //положение мыши из экранных в мировые координаты
      //  f = mousePosition;

        transform.Translate(Vector3.forward /*mousePosition*/ * Speed * Time.deltaTime);
        RaycastHit hit;
        if (Physics.Linecast(lastPos, transform.position, out hit))
        {
            GameObject d = Instantiate<GameObject>(decal);
            d.transform.position = hit.point + hit.normal * 0.001f;
            d.transform.rotation = Quaternion.LookRotation(-hit.normal);
            Destroy(d, 10);
            Destroy(gameObject);
            Debug.Log(hit.transform.name);
            if (hit.transform == GameObject.FindGameObjectWithTag("Player"))
                {
                Debug.Log("LOSE");
            }
            if (hit.transform.name == "Pistol")//(hit.transform == GameObject.FindGameObjectWithTag("Weapon"))
            {
                Debug.Log("LOSE1");
            }
        }
        lastPos = transform.position;

       

    }
}
