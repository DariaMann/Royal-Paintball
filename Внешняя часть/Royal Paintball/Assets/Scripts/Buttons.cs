using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour {
    public void PlayButton()
    {
        SceneManager.LoadScene("Game");
    }
    public void Exit()
    {
        //SceneManager.UnloadScene();
        Application.Quit();
    }
}
