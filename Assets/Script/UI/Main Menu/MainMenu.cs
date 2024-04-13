using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject settingsGameObject;
    [SerializeField] private GameObject mainMenuGameObject;
   
    private void Start()
    {
        mainMenuGameObject.SetActive(true);
        settingsGameObject.SetActive(false);
    }

    public void LoadStageMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadSettingsMenu()
    {
        mainMenuGameObject.SetActive(false);
        settingsGameObject.SetActive(true);
    }

    public void LoadBackMainMenuMenu()
    {
        mainMenuGameObject.SetActive(true);
        settingsGameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
