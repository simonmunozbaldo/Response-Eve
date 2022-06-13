using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MistKiller : MonoBehaviour
{
    public DeathController deathController;
    public float timeToDie;
    float timeEntered;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Mist"))
        {
            timeEntered = Time.time;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (deathController.isDead)
        {
            return;
        }
        if (collision.CompareTag("Mist"))
        {
            if(Time.time - timeEntered > timeToDie)
            {
                deathController.StartCoroutine(deathController.DieElectrified());
            }
        }
    }

}
