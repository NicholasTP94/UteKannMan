using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }
    public void QuitGame()
    {
        Debug.Log("Game has stopped.");
        Application.Quit();
    }
    public void Controls()
    {
        SceneManager.LoadScene("Controls");
    }
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
