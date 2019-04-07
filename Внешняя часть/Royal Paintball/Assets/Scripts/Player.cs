using UnityEngine;

public class Player : MonoBehaviour
{
    void Update()
    {
        var mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition); //положение мыши из экранных в мировые координаты
     
    }

}