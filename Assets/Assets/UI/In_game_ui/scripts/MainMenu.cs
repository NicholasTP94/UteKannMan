using System.Collections;
using System.Collections.Generic;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject controlsMenuPanel;

    private void Start()
    {
        mainMenuPanel.SetActive(true);
        controlsMenuPanel.SetActive(false);
    }
    public void Controls()
    {
        mainMenuPanel.SetActive(false);
        controlsMenuPanel.SetActive(true);
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }
    public void QuitGame()
    {
        Debug.Log("Game has stopped.");
        Application.Quit();
    }
    
    public void ReturnToMainMenu()
    {
        controlsMenuPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}
