using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FromWinToMainMenu : MonoBehaviour
{
    public void FromDeathToMenuScene()
    {
        SceneManager.LoadScene("Menu");
    }
}
