using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfLevel : MonoBehaviour
{
    [Header ("References")]
    public Player playerController;
    public Transform cameraPoint;
    public Transform glass;
    public Transform arm;
    public GameObject eveCopy;
    public Transform glassBotPoint;
    public Transform eveBotPoint;
    public Transform pointOfTheCamera;
    public CameraHorizontalZone horizontalCameraToStop;
    public PointSummaryController pointSummaryController;
    public PauseController pauseController;

    [Header("ExtraData")]
    public float timeToDropDown;
    public float ropePickUpSpeed;
    public float drowningSpeed;
    public float cameraTime;

    [Header("Audio")]
    public AudioSource glassAudioS;
    public AudioSource musicAudioS;
    public AudioSource suckEveAudioS;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(DropGlass());
            StartCoroutine(MoveCamera());
            pauseController.enabled = false;
        }
    }

    private IEnumerator MoveCamera()
    {
        Vector2 initialPos = cameraPoint.position;
        float startTime = Time.time;
        while(Time.time - startTime < cameraTime)
        {
            cameraPoint.position = Vector2.Lerp(initialPos, pointOfTheCamera.position, (Time.time - startTime) / cameraTime);
            yield return null;
        }
    }

    private IEnumerator DropGlass()
    {
        playerController.gameObject.SetActive(false);
        eveCopy.SetActive(true);
        //eveCopy.transform.position = playerController.transform.position;

        eveCopy.transform.position = new Vector2(eveBotPoint.transform.position.x, playerController.transform.position.y);

        float time = 0f;
        Vector2 eveRelativeToGlass = (Vector2) (eveBotPoint.position- glassBotPoint.position);
        Vector2 initialGlassPoint = glass.position;
        bool followsGlass = false;

        musicAudioS.volume = musicAudioS.volume * 0.60f;
        glassAudioS.Play();

        while (time < timeToDropDown)
        {
            glass.transform.position = Vector2.Lerp(initialGlassPoint, glassBotPoint.position, time / timeToDropDown);
            if (!followsGlass && ((Vector2)glass.transform.position + eveRelativeToGlass).y < eveCopy.transform.position.y)
            {
                followsGlass = true;
            }

            if (followsGlass)
            {
                eveCopy.transform.position = (Vector2)glass.transform.position + eveRelativeToGlass;
            }
            
            yield return null;
            time += Time.deltaTime;
        }

        glass.transform.position = glassBotPoint.position;
        eveCopy.transform.position = (Vector2)glass.transform.position + eveRelativeToGlass;
        animator.SetTrigger("End");

        if (horizontalCameraToStop != null)
        {
            horizontalCameraToStop.StopCurrentCoroutine();
        }
    }

    private void PickUpArm()
    {
        StartCoroutine(PickUPArmCoroutine());
    }

    private IEnumerator PickUPArmCoroutine()
    {
        arm.parent = null;

        while (true)
        {
            arm.transform.position = (Vector2)arm.transform.position + (Vector2.up * ropePickUpSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private void DrownGlass()
    {
        StartCoroutine(DrownGlassCoroutine());
    }

    private IEnumerator DrownGlassCoroutine()
    {
        float timer = 0f;

        suckEveAudioS.Play();

        while (timer < 1f)
        {
            eveCopy.transform.position = (Vector2)eveCopy.transform.position + (Vector2.down * drowningSpeed * Time.deltaTime);
            glass.transform.position = (Vector2)glass.transform.position + (Vector2.down * drowningSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }
        pointSummaryController.StartRecount();
    }
}
