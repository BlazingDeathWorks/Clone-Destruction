using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    const string sceneLevel1 = "Level 1";
    const string sceneMainMenu = "Main Menu";
    
    public void PlayAgain()
    {
        SceneManager.LoadScene(sceneLevel1);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(sceneMainMenu);
    }
}
