using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PointSummaryController : MonoBehaviour
{
    [Header("References")]
    public DeathController deathController;
    public GameObject summaryPanel;
    public TextMeshProUGUI deathCounterTextM;
    public TextMeshProUGUI timeCounterTextM;
    public Image medalImage;
    public List<Sprite> listOfMedalSprites;
    public LevelPersistanceManager levelPersManager;

    [Header("LevelObjectives")]
    public float timeToComplete1;
    public float timeToComplete2;
    public float timeToComplete3;
    public float deathsToComplete1;
    public float deathsToComplete2;
    public float deathsToComplete3;

    [Header("Other Values")]
    public float deathsPerSecond;
    public int debugDeaths;
    public float debugTime;

    [Header("Audio")]
    public AudioSource deathCounterAudio;

    private float timeSinceStarted;
    private Animator summaryPanelAnimator;
    private int averageScore;
    private GameObject scope;

    private void Awake()
    {
        summaryPanelAnimator = summaryPanel.GetComponent<Animator>();
        scope = GameObject.Find("Scope");
    }

    void Update()
    {
        //No hace falta comprobar si está pausado ya que la pausa afecta al delta time
        timeSinceStarted += Time.deltaTime;
    }

    public void StartRecount()
    {
        summaryPanel.SetActive(true);
        PutTime();
        SetMedalImageAndCheckHighScore();
        Cursor.visible = true;
        scope.SetActive(false);
    }

    public void StartDeathCounter()
    {
        StartCoroutine(StartDeathCounterCoroutine());
    }

    private IEnumerator StartDeathCounterCoroutine()
    {
        float timer = 0f;
        int deaths = 0;
        int totalDeaths = deathController.GetNumberOfDeaths();

        //--------------------
        //totalDeaths = debugDeaths;
        //------------------------
        int deathCounterBefore = 0;

        while (true)
        {
            timer += Time.deltaTime;
            deaths = Mathf.FloorToInt(deathsPerSecond * timer);
            if(deaths != deathCounterBefore)
            {
                deathCounterAudio.Play();
            }
            deathCounterBefore = deaths;
            if(deaths < totalDeaths)
            {
                deathCounterTextM.text = deaths + "";
            }
            else
            {
                break;
            }
            yield return null;
        }
        deathCounterTextM.text = totalDeaths + "";
        summaryPanelAnimator.SetTrigger("DeathCounterEnded");
        
    }

    private void PutTime()
    {
        int timeSinceStartedInt = Mathf.FloorToInt(timeSinceStarted);
        int minutes = timeSinceStartedInt / 60;
        int seconds = timeSinceStartedInt % 60;
        
        string minutesStr = "";
        if(minutes < 10)
        {
            minutesStr += "0";
        }
        minutesStr += minutes + "";

        string secondsStr = "";
        if (seconds < 10)
        {
            secondsStr += "0";
        }
        secondsStr += seconds + "";

        timeCounterTextM.text = minutesStr + ":" + secondsStr;
    }

    private void SetMedalImageAndCheckHighScore()
    {
        int deathScore;
        int deaths = deathController.GetNumberOfDeaths();
        //deaths = debugDeaths;

        if(deaths > deathsToComplete1)
        {
            deathScore = 0;

        }else if(deaths > deathsToComplete2)
        {
            deathScore = 1;

        }else if(deaths > deathsToComplete3)
        {
            deathScore = 2;
        }
        else
        {
            deathScore = 3;
        }

        int timeScore;

        if(timeSinceStarted > timeToComplete1)
        {
            timeScore = 0;

        }
        else if(timeSinceStarted > timeToComplete2)
        {
            timeScore = 1;

        }
        else if(timeSinceStarted > timeToComplete3)
        {
            timeScore = 2;
        }
        else
        {
            timeScore = 3;
        }

        averageScore = Mathf.FloorToInt((timeScore + deathScore) / 2f);

        medalImage.sprite = listOfMedalSprites[averageScore];

    }


    public void ClickContinueToMenu()
    {
        levelPersManager.SaveLevel(SceneManager.GetActiveScene().name, averageScore + 1 /*(porque el 0 es no pasado)*/);
        SceneManager.LoadScene("MainMenu");
    }
}
