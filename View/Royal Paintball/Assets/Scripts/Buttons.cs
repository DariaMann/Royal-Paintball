using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour {

    private ClientTCP clientTCP = new ClientTCP();
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
