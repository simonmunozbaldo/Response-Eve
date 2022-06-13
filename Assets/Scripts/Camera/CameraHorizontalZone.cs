using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraHorizontalZone : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public Transform cameraPoint;
    public Transform referencePoint;
    public DeathController deathController;
    public float transitionTime;

    private bool active = false;
    private Coroutine coroutine;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            coroutine = StartCoroutine(MoveCameraIn());
        }
    }

    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            coroutine = StartCoroutine(MoveCameraOut());
        }
    }


    private void Update()
    {
        if (active)
        {
            cameraPoint.transform.position = new Vector2(deathController.transform.position.x, referencePoint.position.y);
        }
            
            
    }

    private IEnumerator MoveCameraIn()
    {
        if (active)
        {
            cameraPoint.transform.position = new Vector2(cameraPoint.transform.position.x, referencePoint.position.y);
            yield break;
        }

        if (transitionTime == 0f || deathController.isDead)
        {
            active = true;
            cameraPoint.transform.position = new Vector2(cameraPoint.transform.position.x, referencePoint.position.y);
            yield break;
        }

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        float yPoint;
        float timer = 0f;
        float initialY = cameraPoint.transform.position.y;

        while(timer <= transitionTime)
        {
            yPoint = Mathf.Lerp(initialY, referencePoint.position.y, timer / transitionTime);
            cameraPoint.transform.position = new Vector2(cameraPoint.transform.position.x, yPoint);
            yield return null;
            timer += Time.deltaTime;
        }

        active = true;
    }

    private IEnumerator MoveCameraOut()
    {
        if (deathController.isDead)
        {
            while(deathController.playerCollider.enabled == false)
            {
                yield return null;
            }
            cameraPoint.transform.localPosition = Vector2.zero;
            active = false;
            yield break;
        }

        float yPoint;
        float timer = 0f;
        float initialY = cameraPoint.transform.localPosition.y;

        while (timer <= transitionTime)
        {
            yPoint = Mathf.Lerp(initialY, 0f, timer / (transitionTime + 0.01f));
            cameraPoint.transform.localPosition = new Vector2(cameraPoint.transform.localPosition.x, yPoint);
            yield return null;
            timer += Time.deltaTime;
        }
        cameraPoint.transform.localPosition = Vector2.zero;
        active = false;
        
    }

    public void StopCurrentCoroutine()
    {
        StopCoroutine(coroutine);
        active = false;
    }



}
