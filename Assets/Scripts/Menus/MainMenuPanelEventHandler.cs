using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPanelEventHandler : MonoBehaviour
{
    public MainMenuManager mainMenuManager;
    public void StartNext()
    {
        mainMenuManager.TransitionToNext();
    }

    public void ExitTransitionEnded()
    {
        mainMenuManager.ExitTransitionEnded();
    }

}
