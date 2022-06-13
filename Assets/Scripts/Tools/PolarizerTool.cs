using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolarizerTool : Tool
{
    public LineRenderer laserRenderer;
    public float timeForLaser;
    public Transform launchPoint;
    public AudioClip audioClip;
    private Animator animator;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    public override void UseTool(Player playerCtx, RaycastHit2D rayHit)
    {
        base.UseTool(playerCtx, rayHit);
        Polarizer polarizerScript = rayHit.collider.gameObject.GetComponent<Polarizer>();
        polarizerScript.ChangePolarization();

        animator.SetTrigger("Activate");
        toolAudioSource.pitch = Random.Range(1f,1.3f);
        toolAudioSource.clip = audioClip;
        toolAudioSource.Play();
        playerCtx.StartCoroutine(MakeLaserEffect(rayHit));
    }

    private IEnumerator MakeLaserEffect(RaycastHit2D rayHit)
    {
        laserRenderer.gameObject.SetActive(true);
        float timer = 0f;

        while(timer < timeForLaser)
        {
            laserRenderer.SetPosition(0, launchPoint.position);
            laserRenderer.SetPosition(1,Vector2.Lerp(launchPoint.position, rayHit.point, timer / timeForLaser));
            timer += Time.deltaTime;
            yield return null;
        }
        laserRenderer.SetPosition(1, rayHit.point);
        yield return null;
        yield return null;
        laserRenderer.gameObject.SetActive(false);
    }
}
