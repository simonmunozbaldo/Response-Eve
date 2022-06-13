using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraZoomZone : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public Transform cameraPoint;
    public float zoom;
    public bool doesZoom;
    public float time;
    public DeathController deathController;
    private Coroutine zoomCoroutine;

    private float currentZoom => virtualCamera.m_Lens.OrthographicSize;
    private float defaultZoom;

    private void Awake()
    {
        doesZoom = true;
    }

    protected virtual void Start()
    {
        defaultZoom = currentZoom;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && doesZoom)
        {
            if (zoomCoroutine != null)
            {
                StopCoroutine(zoomCoroutine);
            }
            zoomCoroutine = StartCoroutine(ZoomIn());
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && doesZoom)
        {
            if (zoomCoroutine != null)
            {
                StopCoroutine(zoomCoroutine);
            }
            zoomCoroutine = StartCoroutine(ZoomOut());
        }
    }

    private IEnumerator ZoomIn()
    {
        if(time == 0f || deathController.isDead)
        {
            virtualCamera.m_Lens.OrthographicSize = zoom;
            zoomCoroutine = null;
            yield break;
        }


        float zoomSpeed = Mathf.Abs(zoom - currentZoom) / time;

        if(zoom > currentZoom)
        {
            while(currentZoom < zoom)
            {
                virtualCamera.m_Lens.OrthographicSize = currentZoom + zoomSpeed * Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            while (currentZoom > zoom)
            {
                virtualCamera.m_Lens.OrthographicSize = currentZoom - zoomSpeed * Time.deltaTime;
                yield return null;
            }
        }

        virtualCamera.m_Lens.OrthographicSize = zoom;
        zoomCoroutine = null;
    }

    private IEnumerator ZoomOut()
    {
        if (deathController.isDead)
        {
            while (deathController.playerCollider.enabled == false)
            {
                yield return null;
            }
            virtualCamera.m_Lens.OrthographicSize = defaultZoom;
            zoomCoroutine = null;
            yield break;
        }

        float zoomSpeed = Mathf.Abs(defaultZoom - currentZoom) / time;
        if (defaultZoom > currentZoom)
        {
            while (currentZoom < defaultZoom)
            {
                virtualCamera.m_Lens.OrthographicSize = currentZoom + zoomSpeed * Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            while (currentZoom > defaultZoom)
            {
                virtualCamera.m_Lens.OrthographicSize = currentZoom - zoomSpeed * Time.deltaTime;
                yield return null;
            }
        }

        virtualCamera.m_Lens.OrthographicSize = defaultZoom;
        zoomCoroutine = null;
    }
}
