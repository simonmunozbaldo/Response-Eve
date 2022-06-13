using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockableForLevel : MonoBehaviour
{
    Level level;

    private void Awake()
    {
        level = GetComponent<Level>();
    }

    private void Update()
    {
        if (Input.GetKeyDown("d"))
        {
            level.SetUnlocked(1);
            level.UpdateLevelState();
        }
    }
}
