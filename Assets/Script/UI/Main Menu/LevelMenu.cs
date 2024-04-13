using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    [SerializeField]
    private Button[] levelButtons;
    [SerializeField]
    private GameObject LevelHandler;
    

    private void Awake()
    {
        ButtonsToArray();
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        for(int i = 0; i < levelButtons.Length; i++)
        {
            levelButtons[i].interactable = false;
        }
        for (int i = 0; i < unlockedLevel; i++)
        {
            levelButtons[i].interactable = true;
        }
    }

    public void OpenLevel(int levelID)
    {
        SceneManager.LoadScene(levelID + 1);
    }

    private void ButtonsToArray()
    {
        int childCount = gameObject.transform.childCount;
        levelButtons = new Button[childCount];
        for (int i = 0; i < childCount; i++)
        {
            levelButtons[i] = gameObject.transform.GetChild(i).gameObject.GetComponent<Button>();
        }
    }


}


