using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour {

    ClientTCP clientTCP = new ClientTCP();
    public void PlayButton()
    {
        // SceneManager.LoadScene("Game");
        SceneManager.LoadScene("Waiting");
    }
    public void Exit()
    {
        Debug.Log("Лег сервер");
        clientTCP.Disconnect();
        SceneManager.LoadScene("Play");
    
        //SceneManager.UnloadScene();
        Application.Quit();
    }
    public void Menu()
    {
        SceneManager.LoadScene("Play");
    }

}
