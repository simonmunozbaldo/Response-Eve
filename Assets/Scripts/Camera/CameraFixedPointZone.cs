using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFixedPointZone : CameraZoomZone
{
    public Transform point;
    private Transform parentBackup;
    private Coroutine coroutine;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        parentBackup = cameraPoint.transform.parent;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.CompareTag("Player"))
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            coroutine = StartCoroutine(MoveCameraIn());
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);

        if (collision.CompareTag("Player"))
        {
            if(coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            coroutine = StartCoroutine(MoveCameraOut());
        }
    }

    private IEnumerator MoveCameraIn()
    {

        cameraPoint.transform.parent = null;
        float timer = 0f;
        Vector2 initialPosition = cameraPoint.transform.position;
        while (timer < this.time)
        {
            cameraPoint.transform.position = Vector2.Lerp(initialPosition, point.transform.position, timer / this.time);
            timer += Time.deltaTime;
            yield return null;
        }

        cameraPoint.transform.position = point.position;
        coroutine = null;

    }

    private IEnumerator MoveCameraOut()
    {
        float timer = 0f;
        Vector2 initialPosition = cameraPoint.transform.position;
        while (timer < this.time)
        {
            cameraPoint.transform.position = Vector2.Lerp(initialPosition, parentBackup.position, timer / this.time);
            timer += Time.deltaTime;
            yield return null;
        }

        cameraPoint.transform.position = parentBackup.position;
        cameraPoint.transform.parent = parentBackup;
        coroutine = null;

    }
}
