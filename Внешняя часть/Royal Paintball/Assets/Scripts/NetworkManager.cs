using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;


public class NetworkManager : MonoBehaviour {

    private ClientTCP clientTCP = new ClientTCP();
    [SerializeField]
    public GameObject playerPref;
    private Dictionary<int, GameObject> playerList = new Dictionary<int, GameObject>();
    public int MyIndex=0;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start () {
        clientTCP.Connect();
     //   clientTCP.GetPos();
       // Pos(clientTCP.GetPos());
        InstantiatePlayer(MyIndex, clientTCP.GetPos());
    }
    public void Pos(string str)
    {
        string[] words = str.Split(new char[] { ',' });
        float x= Convert.ToSingle(words[0]);
        float y= Convert.ToSingle(words[1]); 
        float z= Convert.ToSingle(words[2]);
        Vector3 v = new Vector3(x, y, z);
        playerPref.transform.position=v;
    }
    private void Update()
    {
       // clientTCP.Message(MyIndex);
        //clientTCP.Place(playerPref);
    }
    public void InstantiatePlayer(int index, string str)
    {
        string[] words = str.Split(new char[] { ',' });
        float x = Convert.ToSingle(words[0]);
        float y = Convert.ToSingle(words[1]);
        float z = Convert.ToSingle(words[2]);
        Vector3 v = new Vector3(x, y, z);
      //  playerPref.transform.position = v;
        GameObject temp = Instantiate(playerPref,v, Quaternion.identity);
        temp.name = "Player: " + index;
        playerList.Add(index, temp);
        MyIndex++;
    }

}
