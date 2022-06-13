using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsDeleter : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        PlayerPrefs.DeleteAll();
    }

    
}
