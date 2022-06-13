using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitToMenuConf : MonoBehaviour
{
    public PauseController pauseController;
    public GameObject mainPausePanel;
    public GameObject confirmationPanel;

    public void ClickExit()
    {
        pauseController.enabled = false;
        mainPausePanel.SetActive(false);
        confirmationPanel.SetActive(true);
    }

    public void ClickYes()
    {
        pauseController.TogglePauseMenu();
        Cursor.visible = true;
        SceneManager.LoadScene("MainMenu");
    }

    public void ClickNo()
    {
        pauseController.enabled = true;
        mainPausePanel.SetActive(true);
        confirmationPanel.SetActive(false);
    }
}
