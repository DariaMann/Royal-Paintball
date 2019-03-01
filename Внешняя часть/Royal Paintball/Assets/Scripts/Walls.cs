using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walls : MonoBehaviour {

    float XSize = 9f;
    float ZSize = 9f;
    [SerializeField]
    GameObject foodPrefab;
    GameObject curFood;
    Vector3 curPos;
    [SerializeField]
    int countFood;

    float x;
    float z;
    public GameObject Plane;

 
    public float seconds = 1f;
    public float timer = 10f;

    public void Start()
    {
       // AddNewFood();
        x = Plane.transform.localScale.x;
        z = Plane.transform.localScale.z;
    }
    void AddNewFood()//Добавление еды
    {
        countFood = Random.Range(7, 20);
        int j = countFood;
        for (int i = 0; i < j; i++)
        {
            RandomPos();
            curFood = GameObject.Instantiate(foodPrefab, curPos, Quaternion.identity) as GameObject;
            countFood++;
        }
    }

    void RandomPos()//Рандомная позиция
    {
        curPos = new Vector3(Random.Range(z * -1, z), 0.25f, Random.Range(x * -1, x));
    }
}
