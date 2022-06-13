using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetPullTool : Tool
{
    public float pullPropulsionSpeed;

    [Header("Audio")]
    public AudioClip audioClip;
    public float higherPitch;
    public float lowerPitch;
    

    private Animator animator;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    public override void UseTool(Player playerCtx, RaycastHit2D rayHit)
    {
        base.UseTool(playerCtx, rayHit);

        if (CheckForCounter(rayHit))
        {
            PullPlayer(playerCtx);
        }

        
    }

    private bool CheckForCounter(RaycastHit2D rayHit)
    {
        SurfaceCounter surfaceCounterScript = rayHit.collider.GetComponent<SurfaceCounter>();
        if (surfaceCounterScript != null)
        {
            if (surfaceCounterScript.counter > 0)
            {
                surfaceCounterScript.SubtractToCounter(1);
                return true;
            }

            return false;
        }

        return true;
    }

    private void PullPlayer(Player playerCtx)
    {
        playerCtx.rbody.velocity = playerCtx.distanceFromScope.normalized * pullPropulsionSpeed;
        animator.SetTrigger("Retract");
        toolAudioSource.pitch = Random.Range(lowerPitch,higherPitch);
        toolAudioSource.clip = audioClip;
        toolAudioSource.Play();
    }

}
