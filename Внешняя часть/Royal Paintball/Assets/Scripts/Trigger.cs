using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trigger : MonoBehaviour {

    //[SerializeField]
    //Transform figure;
    float speed = 50f;
    //Button But;

    public GameObject cur;
    public GameObject bullet;
    public GameObject player;
    Vector3 f;
    private NetworkManager net = new NetworkManager();
    public List<GameObject> bul = new List<GameObject>();

    public Color[] colors;





    private float curTimeout;

    private void Update()
    {
        DelTail();
       
    }
    void OnMouseDown()
    {
      //  player = GameObject.FindGameObjectWithTag("Weapon");

      //  var mousePosition = Input.mousePosition;
      //  mousePosition = Camera.main.ScreenToWorldPoint(mousePosition); //положение мыши из экранных в мировые координаты
      //                                                                 //f = mousePosition;
      //  Vector3 p = new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * Time.deltaTime * speed;
      //  //player = GameObject.FindGameObjectWithTag("Player");

      //  float x = player.transform.position.x + 0.8f;
      //  float y = player.transform.position.y;
      //  float z = player.transform.position.z;
      //  Vector3 v = new Vector3(x, y, z);
      //  Vector3 mp = new Vector3(mousePosition.x, mousePosition.y, -2.77f);
      ////  cur = GameObject.Instantiate(bullet, v, Quaternion.LookRotation(mp)) as GameObject;
      //  cur = GameObject.Instantiate(bullet, v, bullet.transform.rotation) as GameObject;//появление новой пули
      //  int l = Random.Range(0, colors.Length);
      //  cur.GetComponent<Renderer>().material.color = colors[l];




        //bul.Add(cur);

        //var look_dir = f - cur.transform.position;
        //look_dir.y = 0;
        //cur.transform.rotation = Quaternion.Slerp(cur.transform.rotation, Quaternion.LookRotation(look_dir), speed * Time.deltaTime);
        //cur.transform.position += cur.transform.forward * speed * Time.deltaTime;
        // Vector3 v = new Vector3(0, 90, 0);
        // cur.transform.rotation.y = new Quaternion(this.transform.rotation.x + 20, this.transform.rotation.y, this.transform.rotation.z, this.transform.rotation.w);
        //cur.GetComponent<Bullet>().SetTarget(f);
    }

    public void DelTail()
    {
        for (int i = 1; i < bul.Count; i++)
        {
            if (bul[i].transform.position.x > 45f || bul[i].transform.position.x < -33f || bul[i].transform.position.y > 21f || bul[i].transform.position.y < -25f)
            {
                Destroy(bul[i]);
                Debug.Log("Yes");
            }
            else
                Debug.Log("NO");
        }
        bul.Clear();
    }
}
