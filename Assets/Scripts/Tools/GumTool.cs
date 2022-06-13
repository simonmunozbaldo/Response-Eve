using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GumTool : Tool
{
    public AudioClip suckAudio;
    private Animator animator;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    public override void UseTool(Player playerCtx, RaycastHit2D rayHit)
    {
        base.UseTool(playerCtx, rayHit);
        playerCtx.ChangeMaterialState(Player.materialState.Gum);

        animator.SetTrigger("Absorb");
        toolAudioSource.pitch = Random.Range(1f,1.2f);
        toolAudioSource.clip = suckAudio;
        toolAudioSource.Play();
    }
}
