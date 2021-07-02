using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenuController : MonoBehaviour
{

    public GameObject ContinueGameButton;


    private void Start()
    {

        string dir = Application.persistentDataPath + "/SaveData/";

        if (ContinueGameButton != null)
        {
            ContinueGameButton.SetActive(false);
            if (Directory.Exists(dir))
            {
                ContinueGameButton.SetActive(true);
            }
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void NewGame()
    {
        //delete save file then load game scene
        string dir = Application.persistentDataPath + "/SaveData/";

        if (Directory.Exists(dir))
        {
            Directory.Delete(dir, true);
        }
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void AboutScene()
    {
        SceneManager.LoadScene(2);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
