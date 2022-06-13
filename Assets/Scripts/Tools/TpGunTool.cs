using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpGunTool : Tool
{
    public GameObject tpBall;
    public float timeOfPlayerTransition;
    public GameObject cameraPoint;
    [Header("Audio")]
    public AudioClip audioClipShoot;
    public AudioClip audioClipTp;

    private TeleportBall tpBallScript;
    private Animator animator;
    
    protected override void Awake()
    {
        base.Awake();
        tpBallScript = tpBall.GetComponent<TeleportBall>();
        animator = GetComponent<Animator>();
    }

    public override void UseTool(Player playerCtx, RaycastHit2D rayHit)
    {
        base.UseTool(playerCtx, rayHit);
        
        tpBallScript.direction = playerCtx.distanceFromScope;
        tpBall.SetActive(true);

        animator.SetTrigger("Shoot");
        toolAudioSource.pitch = 2f;
        toolAudioSource.clip = audioClipShoot;
        toolAudioSource.Play();

        return;
    }

    public bool CheckTp(Player playerCtx, RaycastHit2D rayHit)
    {
        if (tpBall.activeSelf)
        {
            if (!rayHit || rayHit.collider.tag == "Ground" || rayHit.collider.tag == "Mist" || rayHit.collider.tag == "TeleportBallSurface")
            {
                //playerCtx.rbody.MovePosition(tpBall.transform.position);
                playerCtx.transform.position = tpBall.transform.position;
                playerCtx.rbody.velocity = Vector2.zero;
                tpBall.SetActive(false);
                playerCtx.StartCoroutine(TpEffectOnPlayer(playerCtx));

                toolAudioSource.pitch = 2f;
                toolAudioSource.clip = audioClipTp;
                toolAudioSource.Play();

                return true;
            }
        }

        return false;

    }

    public IEnumerator TpEffectOnPlayer(Player playerCtx)
    {
        float timer = 0.0001f; // Con 0 no funciona porque se jode el signo (al principio xScale se pondrá en 0 aunque el signo sea negativo y necesitamos que si antes era negativo se mantenga negativo)
        float xScale,yScale;
        Transform aux = cameraPoint.transform.parent;
        cameraPoint.transform.parent = null; //Para que la posicion relativa del camara no cambie mientras dure la animación
        while (timer < timeOfPlayerTransition)
        { 

            xScale = Mathf.Sign(playerCtx.transform.localScale.x) * (timer / timeOfPlayerTransition);
            yScale = Mathf.Sign(playerCtx.transform.localScale.y) * (2-(timer / timeOfPlayerTransition)) ;
            playerCtx.transform.localScale = new Vector3(xScale, yScale, 1f);
            timer += Time.deltaTime;

            yield return null;
        }
        xScale = Mathf.Sign(playerCtx.transform.localScale.x);
        yScale = Mathf.Sign(playerCtx.transform.localScale.y);
        playerCtx.transform.localScale = new Vector3(xScale, yScale, 1f);
        cameraPoint.transform.parent = aux;
        
    }
}
