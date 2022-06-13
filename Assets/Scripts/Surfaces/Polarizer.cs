using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Polarizer:MonoBehaviour
{
    public Transform polarSurfaceParentTrans;
    private List<Tilemap> childrenTilemaps;
    public Color blue;
    public Color red;
    private Animator animator;

    void Awake()
    {
        childrenTilemaps = new List<Tilemap>();
        for(int i = 0; i < polarSurfaceParentTrans.childCount; i++)
        {
            childrenTilemaps.Add(polarSurfaceParentTrans.GetChild(i).GetComponent<Tilemap>());
        }
        animator = GetComponent<Animator>();
    }

    public void ChangePolarization()
    {
        foreach(Tilemap tm in childrenTilemaps)
        {
            if (tm.CompareTag("PullSurface"))
            {
                tm.color = red;
                tm.tag = "PushSurface";
            }
            else
            {
                tm.color = blue;
                tm.tag = "PullSurface";
            }
        }

        animator.SetTrigger("Change");
    }

}
