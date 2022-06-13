using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateOrangeAura : MonoBehaviour
{
    public float distanceToTravel;

    public float goTime;
    private Vector3 scaleToGoForParent;
    private SpriteRenderer spriteR;

    private void Awake()
    {
        spriteR = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {

        CalculateScaleToGo();
        StartCoroutine(LoopOut());
    }

    IEnumerator LoopOut()
    {
        float timer = 0f;
        
        while(timer <= goTime)
        {
            this.transform.localScale = Vector3.Lerp(new Vector3(1f, 1f, 1f), scaleToGoForParent, timer / goTime);
            timer += Time.deltaTime;
            yield return null;
        }
        this.transform.localScale = scaleToGoForParent;

        StartCoroutine(LoopIn());
    }

    IEnumerator LoopIn()
    {
        float timer = 0f;

        while (timer <= goTime)
        {
            this.transform.localScale = Vector3.Lerp(scaleToGoForParent, new Vector3(1f, 1f, 1f), timer / goTime);
            timer += Time.deltaTime;
            yield return null;
        }
        this.transform.localScale = new Vector3(1f, 1f, 1f);

        StartCoroutine(LoopOut());
    }


    private void CalculateScaleToGo()
    {
        float sizeOfHalfWidth = (spriteR.bounds.size.x / 2) ;

        float xProportion = (sizeOfHalfWidth + distanceToTravel) / sizeOfHalfWidth;

        float sizeOfHalfHeight = (spriteR.bounds.size.y / 2);

        float yProportion = (sizeOfHalfHeight + distanceToTravel) / sizeOfHalfHeight;

        scaleToGoForParent = new Vector3(xProportion, yProportion, 1f);

    }
}
