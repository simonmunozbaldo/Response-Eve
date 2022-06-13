using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public GameObject pausePanel;
    public DeathController deathController;
    public Player playerController;

    private GameObject scopeSR;

    private void Awake()
    {
        scopeSR = GameObject.Find("Scope");
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            TogglePauseMenu();
        }
    }

    public void TogglePauseMenu()
    {
        if (pausePanel.activeSelf)
        {
            playerController.enabled = true;
            Cursor.visible = false;
            pausePanel.SetActive(false);
            AudioListener.pause = false;
            scopeSR.SetActive(true);
            Time.timeScale = 1f;
        }
        else
        {
            playerController.enabled = false;
            Cursor.visible = true;
            pausePanel.SetActive(true);
            AudioListener.pause = true;
            scopeSR.SetActive(false);
            Time.timeScale = 0f;
        }
    }

    public void ClickContinue()
    {
        TogglePauseMenu();
    }

    public void ClickSuicide()
    {
        TogglePauseMenu();
        deathController.StartCoroutine(deathController.DieElectrified());
    }

    public void ClickRestartLevel()
    {
        TogglePauseMenu();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }
    
}
