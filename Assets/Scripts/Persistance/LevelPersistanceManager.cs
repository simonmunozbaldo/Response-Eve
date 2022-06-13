using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPersistanceManager : MonoBehaviour
{
    public string levelUnlocksName;

    public void SaveLevel(string levelName,int score)
    {
        /*if(!PlayerPrefs.HasKey(levelName + "Passed"))
        {
            PlayerPrefs.SetInt(levelName + "Passed", 1);
        }*/
        
        if(score > PlayerPrefs.GetInt(levelName + "HighScore",0))
        {
            PlayerPrefs.SetInt(levelName + "HighScore", score);
        }
        
        if(levelUnlocksName != "")
        {
            PlayerPrefs.SetInt(levelUnlocksName + "Unlocked", 1);
        }

        PlayerPrefs.Save();
    }
    
}
