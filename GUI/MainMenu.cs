using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        UnitySceneManager.LoadScene(UnitySceneManager.GetActiveScene().buildIndex + 1);

        GameplayController.GameIsOver = false;
    }

    public void QuitGame()
    {
        Debug.Log("[!] QUIT [!]");
        Application.Quit();
    }
}
