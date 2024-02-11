using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PauseMenu : MonoBehaviour
{

    public GameObject pauseMenuUI; // Assign in Inspector
    public bool isGamePaused = false;

    void Update()
    {
        // Check if the pause button was pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Resume the game
        isGamePaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Pause the game
        isGamePaused = true;
        
    }

   

}
