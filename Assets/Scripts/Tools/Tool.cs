using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour
{
    public string tagName;
    public Color colorForScope;
    protected AudioSource toolAudioSource;

    protected virtual void Awake()
    {
        toolAudioSource = GetComponentInParent<AudioSource>();
    }

    public virtual void UseTool(Player playerCtx,RaycastHit2D rayHit)
    {

    }

    protected virtual void OnEnable()
    {
        
    }
}
