using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetPushTool : Tool
{
    public float pushPropulsionSpeed;
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
            PushPlayer(playerCtx);
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

    private void PushPlayer(Player playerCtx)
    {
        playerCtx.rbody.velocity = -playerCtx.distanceFromScope.normalized * pushPropulsionSpeed;
        animator.SetTrigger("Retract");
        toolAudioSource.pitch = Random.Range(lowerPitch, higherPitch);
        toolAudioSource.clip = audioClip;
        toolAudioSource.Play();
    }
}
