using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTool : Tool
{
    public GameObject portal1;
    public GameObject portal2;
    public Sprite orangeGun;
    public Sprite blueGun;
    public SpriteRenderer spriteRenderer;
    public LineRenderer portalLine;
    public Transform launchPosition;
    public Color colorBlue;
    public Color colorOrange;

    [Header("Audio")]
    public AudioClip portalAudio;

    private Collider2D portal1SurfaceCollider = null;
    private Collider2D portal2SurfaceCollider = null;
    private bool firstPortalIsNext = true;
    

    protected override void Awake()
    {
        base.Awake();
    }

    public override void UseTool(Player playerCtx, RaycastHit2D rayHit)
    {
        base.UseTool(playerCtx, rayHit);

        GameObject launchingPortal;
        Color lineColor;

        if (firstPortalIsNext)
        {
            firstPortalIsNext = false;
            portal1SurfaceCollider = rayHit.collider;
            launchingPortal = portal1;
            lineColor = colorBlue;

            if (rayHit.collider == portal2SurfaceCollider)
            {
                portal2.SetActive(false);
                portal2SurfaceCollider = null;
            }
            toolAudioSource.pitch = 1f;
        }
        else
        {
            firstPortalIsNext = true;
            portal2SurfaceCollider = rayHit.collider;
            launchingPortal = portal2;
            lineColor = colorOrange;

            if (rayHit.collider == portal1SurfaceCollider)
            {
                portal1.SetActive(false);
                portal1SurfaceCollider = null;
            }
            toolAudioSource.pitch = 1.3f;
        }

        Portal portalScript = launchingPortal.GetComponent<Portal>();
        Vector2 limitPointOfSurface1 = rayHit.collider.transform.Find("LP1").position;
        Vector2 limitPointOfSurface2 = rayHit.collider.transform.Find("LP2").position;
        portalScript.SpawnPortal(rayHit.point, rayHit.normal, limitPointOfSurface1, limitPointOfSurface2);

        UpdateVisual();
        StartCoroutine(LanzarRayo(rayHit, lineColor));

        toolAudioSource.clip = portalAudio;
        toolAudioSource.Play();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        spriteRenderer.sprite = firstPortalIsNext ? blueGun : orangeGun;
    }

    private IEnumerator LanzarRayo(RaycastHit2D rayHit,Color color)
    {
        float startTime = Time.time;
        portalLine.gameObject.SetActive(true);
        portalLine.startColor = color;
        portalLine.endColor = color;


        while (Time.time - startTime < 0.2f)
        {
            portalLine.SetPosition(0, launchPosition.position);
            portalLine.SetPosition(1, rayHit.point);
            yield return null;
        }
        
        portalLine.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        portalLine.gameObject.SetActive(false);
        StopAllCoroutines();
    }
}
