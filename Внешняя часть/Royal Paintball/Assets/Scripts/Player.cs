﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public int Speed;
    public int speedRotation;
    public GameObject cur;
    public GameObject bullet;
    //   public string moveDirection = "N";
    NetworkManager M = new NetworkManager();
    void Start()
    {
        
    }


    void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            gameObject.transform.position += gameObject.transform.forward * Speed * Time.deltaTime;
            M.Dir = "W";
        }
        else
        {
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                gameObject.transform.position -= gameObject.transform.forward * Speed * Time.deltaTime;
                M.Dir = "S";
            }
            else
            {
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                {
                    // gameObject.transform.Rotate(Vector3.down * speedRotation);
                    gameObject.transform.position -= gameObject.transform.right * Speed * Time.deltaTime;
                    M.Dir = "A";
                }
                else
                {
                    if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                    {
                        //gameObject.transform.Rotate(Vector3.up * speedRotation);
                        gameObject.transform.position += gameObject.transform.right * Speed * Time.deltaTime;
                        M.Dir = "D";
                        Debug.Log(M.Dir);
                    }
                    else
                    {
                        M.Dir = "N";
                    }
                }
            }
        }
      
    
        var mousePosition = Input.mousePosition;
        //mousePosition.z = transform.position.z - Camera.main.transform.position.z; // это только для перспективной камеры необходимо
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition); //положение мыши из экранных в мировые координаты
      


    }

}