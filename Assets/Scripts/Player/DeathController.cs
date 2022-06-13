using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathController : MonoBehaviour
{
    public Rigidbody2D piece1, piece2, piece3, piece4, piece5;
    public Vector2 pt1, pt2, pt3, pt4, pt5;
    public float piecesSpeed;
    public GameObject arm;
    public SpriteRenderer playerSprite;
    public Collider2D playerCollider;
    public GameObject deathSparks;
    public Checkpoint lastCheckpoint;
    public List<GameObject> listOfRemainingObjects;
    public bool skipSpawn = false;
    [HideInInspector] public bool isDead;
    private Player playerController;
    private float gravityScaleBackUp;
    private int numberOfDeaths = 0;
    private AudioSource eletricityAudioSource;

    public int GetNumberOfDeaths()
    {
        return numberOfDeaths;
    }

    private void Awake()
    {
        playerController = transform.parent.GetComponent<Player>();
        eletricityAudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        gravityScaleBackUp = playerController.rbody.gravityScale;
        if(!skipSpawn)
            StartCoroutine(InitialPlayerSpawn());

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("ElectricDamage") && playerController.currentMaterialState != Player.materialState.Gum)
        {
            StartCoroutine(DieElectrified());

        }else if (collision.CompareTag("FallDamage"))
        {
            StartCoroutine(DieFromFall());
        }
    }

    public IEnumerator DieElectrified()
    {
        eletricityAudioSource.Play();
        numberOfDeaths++;
        isDead = true;
        ActivatePlayer(false);
        LaunchPieces();
        deathSparks.SetActive(true);
        deathSparks.transform.position = playerController.transform.position;

        yield return new WaitForSeconds(1f);

        playerCollider.enabled = true;
        deathSparks.SetActive(false);
        yield return null;
        yield return null;
        lastCheckpoint.Respawn();
        yield return null;
        yield return null;
        isDead = false; // El isDead se utiliza para las zonas de mov de camara
        
    }

    private IEnumerator DieFromFall()
    {
        numberOfDeaths++;
        isDead = true;
        ActivatePlayer(false);

        yield return new WaitForSeconds(1f);

        playerCollider.enabled = true;
        deathSparks.SetActive(false);
        lastCheckpoint.Respawn();
        yield return null;
        yield return null;

        isDead = false;
    }

    private void LaunchPieces()
    {
        piece1.gameObject.SetActive(true);

        piece1.velocity = new Vector2(-1, 1).normalized * piecesSpeed;
        piece1.angularVelocity = 90f;
        piece1.transform.rotation = Quaternion.Euler(0f,0f,0f);
        piece1.transform.position = (Vector2)playerController.transform.position + pt1;

        piece2.gameObject.SetActive(true);
        piece2.velocity = new Vector2(1, 1).normalized * piecesSpeed;
        piece2.angularVelocity = -90f;
        piece1.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        piece2.transform.position = (Vector2)playerController.transform.position + pt2;

        piece3.gameObject.SetActive(true);
        piece3.velocity = new Vector2(-1, -1).normalized * piecesSpeed;
        piece3.angularVelocity = 90f;
        piece1.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        piece3.transform.position = (Vector2)playerController.transform.position + pt3;

        piece4.gameObject.SetActive(true);
        piece4.velocity = new Vector2(1, -1).normalized * piecesSpeed;
        piece4.transform.position = (Vector2)playerController.transform.position + pt4;
        piece1.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        piece3.angularVelocity = -90f;


        piece5.gameObject.SetActive(true);
        piece5.velocity = new Vector2(1, 0).normalized * piecesSpeed;
        piece5.transform.position = (Vector2)playerController.transform.position + pt5;
    }

    public void ActivatePlayer(bool activate)
    {
        playerController.enabled = activate;
        playerSprite.enabled = activate;
        playerCollider.enabled = activate;
        arm.SetActive(activate);

        if (!activate)
        {
            playerController.rbody.gravityScale = 0f;
            playerController.rbody.velocity = Vector2.zero;
            DeactivateAllRemainingObjects();
        }
        else{
            playerController.rbody.gravityScale = gravityScaleBackUp;
            playerController.currentMovementState = Player.movementState.Normal;
        }
    }

    private void DeactivateAllRemainingObjects()
    {
        foreach(GameObject g in listOfRemainingObjects)
        {
            g.SetActive(false);
        }
    }

    private IEnumerator InitialPlayerSpawn()
    {
        isDead = true;
        ActivatePlayer(false);
        playerCollider.enabled = true;
        playerController.transform.position = lastCheckpoint.respawnPoint.position;
        yield return new WaitForSeconds(1f);
        lastCheckpoint.Respawn();

        yield return null;
        isDead = false;
    }

    
}
