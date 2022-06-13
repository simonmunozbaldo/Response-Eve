using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject levelsMenuPanel;
    public GameObject optionsMenuPanel;

    private Animator animatorMainMenu;
    private Animator animatorLevelsMenu;
    private Animator animatorOptionsMenu;

    private GameObject nextMenuGameObject;
    private GameObject lastMenuGameObject;

    private void Awake()
    {
        animatorMainMenu = mainMenuPanel.GetComponent<Animator>();
        animatorLevelsMenu = levelsMenuPanel.GetComponent<Animator>();
        animatorOptionsMenu = optionsMenuPanel.GetComponent<Animator>();
    }

    public void TransitionToNext()
    {
        nextMenuGameObject.SetActive(true);
    }

    public void ExitTransitionEnded()
    {
        lastMenuGameObject.SetActive(false);
    }

    //Button Click Events

    public void ClickPlay()
    {
        animatorMainMenu.SetTrigger("ExitMenu");
        lastMenuGameObject = mainMenuPanel;
        nextMenuGameObject = levelsMenuPanel;
    }

    public void ClickOptions()
    {
        animatorMainMenu.SetTrigger("ExitMenu");
        lastMenuGameObject = mainMenuPanel;
        nextMenuGameObject = optionsMenuPanel;
    }

    public void ClickExit()
    {
        Application.Quit();
    }

    public void ClickBackLevels()
    {
        animatorLevelsMenu.SetTrigger("ExitMenu");
        lastMenuGameObject = levelsMenuPanel;
        nextMenuGameObject = mainMenuPanel;
    }

    public void ClickBackOptions()
    {
        animatorOptionsMenu.SetTrigger("ExitMenu");
        lastMenuGameObject = optionsMenuPanel;
        nextMenuGameObject = mainMenuPanel;
    }

    

    
}
