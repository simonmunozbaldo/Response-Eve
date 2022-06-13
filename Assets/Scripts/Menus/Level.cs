using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Level : MonoBehaviour
{
    public string sceneName;
    public bool unlockedByDefault;

    private int unlocked;
    
    private int score;

    private LevelContainer levelContainer;
    private Animator animator;
    private Image scoreMedalImage;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        //RectTransform thisRect = GetComponent<RectTransform>();
        scoreMedalImage = this.transform.Find("ScoreMedal").GetComponent<Image>();
        levelContainer = this.GetComponentInParent<LevelContainer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        //SetUnlocked
        if (unlockedByDefault)
        {
            unlocked = 1;
        }
        else
        {
            unlocked = PlayerPrefs.GetInt(sceneName + "Unlocked", 0);
        }

        //SetHighScore And Image
        if(unlocked == 1)
        {
            score = PlayerPrefs.GetInt(sceneName + "HighScore", 0);
            if (score > 0)
            {
                scoreMedalImage.enabled = true;
                scoreMedalImage.sprite = levelContainer.medalList[score - 1];
            }
        }

        animator.SetBool("Unlocked", unlocked == 1 ? true : false);
        animator.SetTrigger("Normal");
    }

    public void LevelClick()
    {
        if(unlocked == 1 && sceneName != "")
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    private void OnEnable()
    {
        Start();
    }

    public void UpdateLevelState()
    {
        animator.SetBool("Unlocked", unlocked == 1 ? true : false);
        animator.SetTrigger("Normal");

        if (unlocked == 1)
        {
            PlayerPrefs.SetInt(sceneName + "Unlocked", unlocked);
        }
    }

    public void SetUnlocked(int value)
    {
        unlocked = value;
    }
}
