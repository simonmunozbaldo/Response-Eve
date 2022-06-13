using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("References")]
    public LayerMask laserTargetLayer;
    public PhysicsMaterial2D normalPhysicsM;
    public PhysicsMaterial2D bouncyPhysicsM;
    public SpriteRenderer ArmSpriteR;
    public Color gumColor;
    public float maxFallSpeed;
    public Camera mainCamera;

    [Header("Audio")]
    public AudioSource collisionAudio;
    public AudioSource gumAudio;

    [HideInInspector] public Rigidbody2D rbody;
    [HideInInspector] public Vector2 distanceFromScope => Scope.instance.transform.position - this.transform.position;
    [HideInInspector] public enum materialState { Normal, Gum };
    [HideInInspector] public ToolsController toolsController;
    [HideInInspector] public enum movementState { Normal,Hooking}
    [HideInInspector] public movementState currentMovementState = movementState.Normal;
    [HideInInspector] public materialState currentMaterialState = materialState.Normal;
    [HideInInspector] public Animator animator;

    private Transform armTransform;
    private SpriteRenderer spriteR;
    private bool facingRight = true;
    private RaycastHit2D currentSurfaceHit;


    void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        spriteR = GetComponent<SpriteRenderer>();
        armTransform = this.transform.Find("Arm");
        toolsController = GetComponent<ToolsController>();
        animator = GetComponent<Animator>();
        Application.targetFrameRate = 70;
    }
    
    void Start()
    {
        
    }

    void Update()
    {
        FacePlayer();
        MoveArm();
        
        if(currentMovementState != movementState.Hooking)
        {
            UseRaycast();
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            //ShootRay();
            if (CheckSecondEffects())
            {
                if (toolsController.currentActiveTool != null && currentSurfaceHit)
                {
                    toolsController.currentActiveTool.UseTool(this, currentSurfaceHit);
                }
            }
            
        }
    }

    private void FixedUpdate()
    {
        if(rbody.velocity.y < -maxFallSpeed)
        {
            rbody.velocity = new Vector2(rbody.velocity.x, -maxFallSpeed);
        }
    }

    private void LateUpdate()
    {
        
        animator.SetBool("StayStill",rbody.velocity.magnitude > 0.1f );
        
    }

    private void FacePlayer()
    {
        if( (facingRight && distanceFromScope.x < 0) || (!facingRight && distanceFromScope.x >= 0) ) 
        {
            facingRight = distanceFromScope.x >= 0;
            transform.localScale = new Vector3(transform.localScale.x * -1f,transform.localScale.y , 1f);
            armTransform.localScale = new Vector3(armTransform.localScale.x * -1f, armTransform.localScale.y, 1f);
        }
    }

    private void MoveArm()
    {
        armTransform.rotation = Quaternion.FromToRotation(armTransform.right, distanceFromScope) * armTransform.rotation;
    }


    private void UseRaycast()
    {
        currentSurfaceHit = Physics2D.Raycast(this.transform.position, distanceFromScope, Mathf.Infinity, laserTargetLayer);

        if (currentSurfaceHit && CheckPointIsInCamera(currentSurfaceHit.point))
        {
            toolsController.SwitchTool(currentSurfaceHit.collider.tag);
        }
        else
        {
            toolsController.SwitchTool("a");
        }
    }

    private bool CheckPointIsInCamera(Vector2 point)
    {
        Vector2 vectorDistance = point - (Vector2)mainCamera.transform.position;

        if(Mathf.Abs(vectorDistance.x) <= mainCamera.orthographicSize * mainCamera.aspect && Mathf.Abs(vectorDistance.y) <= mainCamera.orthographicSize)
        {
            return true;
        }
        return false;
    }

    private bool CheckSecondEffects()
    {
        if(((TpGunTool)toolsController.GetToolByTag("TeleportBallSurface")).CheckTp(this,currentSurfaceHit))
        {
            return false;
        }

        if(currentMovementState == movementState.Hooking)
        {
            ((HookTool)toolsController.GetToolByTag("HookSurface")).StopHook();
            return false;
        }

        return true;
    }

    public void ChangeMaterialState(materialState nextState)
    {
        switch (currentMaterialState)
        {
            case materialState.Normal:
                break;

            case materialState.Gum:
                spriteR.color = new Color(1f, 1f, 1f, 1f);
                ArmSpriteR.color = new Color(1f, 1f, 1f, 1f);
                break;
        }

        switch (nextState)
        {
            case materialState.Normal:
                rbody.sharedMaterial = normalPhysicsM;
                break;

            case materialState.Gum:
                spriteR.color = gumColor;
                ArmSpriteR.color = gumColor;
                rbody.sharedMaterial = bouncyPhysicsM;
                break;
        }

        currentMaterialState = nextState;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(currentMaterialState == materialState.Gum)
        {
            StartCoroutine(QuitGumState());
            gumAudio.Play();
        }
        else
        {
            collisionAudio.Play();
        }

        
    }

    private IEnumerator QuitGumState()
    {
        yield return new WaitForSeconds(0.15f);
        ChangeMaterialState(materialState.Normal);
    }



}
