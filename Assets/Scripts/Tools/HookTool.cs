using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookTool : Tool
{
    public GameObject hook;
    public Transform hookPoint;
    public float hookVelocity;
    public float playerToHookVelocity;
    public LineRenderer ropeRenderer;
    public Animator hookAnimator;
    [Header("Audio")]
    public AudioClip hookShootAudio;
    public AudioClip hookHitsAudio;
    public AudioClip playerGoesAudio;

    bool canStopHook;
    Transform oldHookParent;
    RaycastHit2D rayHit;
    Player playerCtx;
    Coroutine hookCoroutine;
    Animator animator;
    float gravityScaleBackUp;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }



    public override void UseTool(Player playerCtx, RaycastHit2D rayHit)
    {
        base.UseTool(playerCtx, rayHit);
        this.playerCtx = playerCtx;
        this.rayHit = rayHit;
        hookCoroutine = StartCoroutine(Hook());
        animator.SetTrigger("Shoot");
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        hook.transform.parent = this.transform;
        hook.transform.position = hookPoint.transform.position;
        hook.transform.localRotation = Quaternion.Euler(0, 0, -90);
        hook.SetActive(true);
    }

    public void StopHook()
    {
        if (canStopHook)
        {
            StopCoroutine(hookCoroutine);
            hookCoroutine = null;
            playerCtx.rbody.velocity = (rayHit.point - (Vector2)playerCtx.transform.position).normalized * playerToHookVelocity * 0.9f;
            // Lo hacemos con player para que la corutina no dependa de que la herramienta esté activa
            playerCtx.StartCoroutine(RetractHook()); 
        }
    }

    public IEnumerator Hook()
    {
        //FASE LANZAR HOOK

        canStopHook = false;
        playerCtx.currentMovementState = Player.movementState.Hooking;
        ropeRenderer.gameObject.SetActive(true);

        Vector2 displacement = rayHit.point - (Vector2)playerCtx.transform.position;
        hook.transform.position = playerCtx.transform.position;
        Vector2 initialHookPosition = hook.transform.position;
        float timer = 0f;
        float hookTime = displacement.magnitude / hookVelocity;

        oldHookParent = hook.transform.parent;
        hook.transform.parent = null;
        hook.SetActive(true);

        hook.transform.rotation = Quaternion.FromToRotation(hook.transform.up, displacement) * hook.transform.rotation;

        toolAudioSource.pitch = Random.Range(1f,1.2f);
        toolAudioSource.clip = hookShootAudio;
        toolAudioSource.Play();

        while (timer < hookTime)
        {
            hook.transform.position = Vector2.Lerp(initialHookPosition, rayHit.point, timer / hookTime);
            timer += Time.deltaTime;
            playerCtx.rbody.velocity = Vector2.zero;
            DrawLine();
            yield return null;
        }
        hook.transform.position = rayHit.point;
        DrawLine();

        hookAnimator.SetTrigger("Hit");
        toolAudioSource.pitch = 1f;
        toolAudioSource.clip = hookHitsAudio;
        toolAudioSource.Play();

        //FASE ESPERAR

        canStopHook = true;
        gravityScaleBackUp = playerCtx.rbody.gravityScale;
        playerCtx.rbody.gravityScale = 0f;
        timer = 0f;
        while (timer < 0.15f)
        {
            playerCtx.rbody.velocity = Vector2.zero;
            timer += Time.deltaTime;
            yield return null;
        }

        //FASE LANZAR PLAYER

        toolAudioSource.pitch = 1f;
        toolAudioSource.clip = playerGoesAudio;
        toolAudioSource.Play();


        timer = 0f;
        float hookPlayerTime = ((rayHit.point - (Vector2)playerCtx.transform.position).magnitude / playerToHookVelocity);
        Vector2 initialPlayerPosition = playerCtx.transform.position;
        while (timer < hookPlayerTime)
        {
            playerCtx.rbody.MovePosition(Vector2.Lerp(initialPlayerPosition, rayHit.point, timer / hookPlayerTime));
            //playerCtx.rbody.velocity = playerToHookVelocity * (rayHit.point - (Vector2)playerCtx.transform.position).normalized;
            timer += Time.deltaTime;

            DrawLine();

            if (playerCtx.currentMaterialState == Player.materialState.Gum)
            {
                if(Vector2.Distance(playerCtx.transform.position,rayHit.point) < 3f)
                {
                    playerCtx.rbody.velocity = (rayHit.point - (Vector2)playerCtx.transform.position).normalized * playerToHookVelocity * 1f;
                    break;
                }
            }

            yield return null;
        }
        //hook.SetActive(false);
        hook.transform.parent = oldHookParent; // El padre (la parte visual) se encargará de desactivarlo
        hook.transform.parent.gameObject.SetActive(false); //Para hacer el OnEnabled de HookToll (tremenda mierda de codigo btw)
        playerCtx.toolsController.UpdateCurrentWeapon();
        playerCtx.currentMovementState = Player.movementState.Normal;
        playerCtx.rbody.gravityScale = gravityScaleBackUp;
        hookCoroutine = null;
        ropeRenderer.gameObject.SetActive(false);

        
        toolAudioSource.Stop();
    }

    public IEnumerator RetractHook()
    {
        playerCtx.currentMovementState = Player.movementState.Normal;
        playerCtx.rbody.gravityScale = gravityScaleBackUp;
        Vector2 displacement = playerCtx.transform.position - hook.transform.position;
        Vector2 initialHookPosition = hook.transform.position;
        float timer = 0f;
        float hookTime = displacement.magnitude / (hookVelocity * 1.15f);

        hook.SetActive(true);

        toolAudioSource.pitch = 2f;
        toolAudioSource.clip = hookHitsAudio;
        toolAudioSource.Play();

        while (timer < hookTime)
        {
            hook.transform.position = Vector2.Lerp(initialHookPosition, playerCtx.transform.position, timer / hookTime);
            timer += Time.deltaTime;
            DrawLine();
            yield return null;
        }

        //hook.SetActive(false);
        hook.transform.parent = oldHookParent;
        hook.transform.parent.gameObject.SetActive(false);
        playerCtx.toolsController.UpdateCurrentWeapon();
        ropeRenderer.gameObject.SetActive(false);
    }

    private void DrawLine()
    {
        ropeRenderer.SetPosition(0, playerCtx.transform.position);
        ropeRenderer.SetPosition(1, hook.transform.position);
    }
}
