using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour {
    public void PlayButton()
    {
        SceneManager.LoadScene("Game");
    }
    public void Exit()
    {
        Application.Quit();
    }

    public void ExitInGame()
    {
        SceneManager.LoadScene("Play");
    }
   
}
