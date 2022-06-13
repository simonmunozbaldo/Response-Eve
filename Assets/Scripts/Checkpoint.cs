using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Transform respawnPoint;
    public DeathController deathController;
    public Player playerController;
    public GameObject portal1;
    public GameObject portal2;
    public AudioSource checkpointAquiredAudio;
    private Animator animator;
    private Animator lightAnimator;
    private bool activated = false;

    bool respawn = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
        lightAnimator = transform.Find("CheckPointLight").GetComponent<Animator>();
    }

    public void Respawn()
    {
        respawn = true;
    }

    void Update()
    {
        //Necesitamos sacar esta función de la corutina para poder controlar el orden de ejecucion con respecto al CameraHorizontalZone (que se ejecuta con Update)
        //Ya que las corrutinas se ejecutan después de todos los updates no podia controlar que se ejecutase el metodo Respawn antes que el Update de CamHZ (en el mismo frame)
        if (respawn)
        {
            playerController.transform.position = respawnPoint.position;
            animator.SetTrigger("Eject");
            respawn = false;
        }
    }

    void RespawnPlayer()
    {
        deathController.ActivatePlayer(true);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(activated == false)
            {
                activated = true;
                deathController.lastCheckpoint = this;
                lightAnimator.enabled = true;
                if(portal1 != null && portal2 != null)
                {
                    portal1.SetActive(false);
                    portal2.SetActive(false);
                }
                checkpointAquiredAudio.Play();
            }
            
        }
    }



}
