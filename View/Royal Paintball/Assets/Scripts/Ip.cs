using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Net.Sockets;
using Newtonsoft.Json;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class Ip : MonoBehaviour {

    public InputField inpuiField;
    public string IP;

    public void ConfermIP()
    {
        IP = inpuiField.text;
        this.name = IP;
        SceneManager.LoadScene("Game");
    }

	void Start () {
        DontDestroyOnLoad(this.gameObject);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
