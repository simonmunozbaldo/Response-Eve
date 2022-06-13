using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject otherPortal;
    public Transform LP1;
    public Transform LP2;
    public Player player;

    [HideInInspector]public bool isPortalBlocked = false;
    private Portal otherPortalScript;
    private Collider2D otherPortalCollider;
    private Vector2 LP1Relative => LP1.position - this.transform.position;
    private Vector2 LP2Relative => LP2.position - this.transform.position;
    private Vector2 velocityFromFrameBefore;
    private Animator animator;
    private AudioSource audioTpSource;

    void Awake()
    {
        otherPortalScript = otherPortal.GetComponent<Portal>();
        otherPortalCollider = otherPortal.GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        audioTpSource = GetComponent<AudioSource>();
    }

    public void SpawnPortal(Vector2 position,Vector2 facingDirection,Vector2 surfaceLimitPoint1,Vector2 surfaceLimitPoint2)
    {
        this.gameObject.SetActive(false);
        this.gameObject.SetActive(true);
        
        //this.transform.rotation = Quaternion.FromToRotation(this.transform.up, facingDirection) * this.transform.rotation;
        this.transform.rotation = Quaternion.Euler(0, 0, VectorUtils.ZAngleOfVectorInWorldSpace(facingDirection));
        
        DeterminePositionOfSpawn(position, surfaceLimitPoint1, surfaceLimitPoint2);
        DetermineScale();

        if (otherPortal.activeSelf)
        {
            otherPortalCollider.enabled = false;
            otherPortalCollider.enabled = true;
        }
    }

    private void DeterminePositionOfSpawn(Vector2 position, Vector2 surfaceLimitPoint1, Vector2 surfaceLimitPoint2)
    {
        this.transform.position = position;
        Vector2 realSurfaceLimitPoint1;
        Vector2 realSurfaceLimitPoint2;

        Vector2 vSLP1SLP2 = surfaceLimitPoint2 - surfaceLimitPoint1;

        //Establecemos las parejas de puntos a comparar
        if(Vector2.Dot(LP2Relative,vSLP1SLP2) > 0)
        {
            realSurfaceLimitPoint1 = surfaceLimitPoint1;
            realSurfaceLimitPoint2 = surfaceLimitPoint2;
        }
        else
        {
            realSurfaceLimitPoint1 = surfaceLimitPoint2;
            realSurfaceLimitPoint2 = surfaceLimitPoint1;
        }

        float auxDiff; 
        if((auxDiff = VectorUtils.GetProjectionModule(LP1Relative, realSurfaceLimitPoint1 - (Vector2)this.transform.position) - LP1Relative.magnitude) < 0)
        {
            this.transform.position = (Vector2) this.transform.position + LP1Relative.normalized * auxDiff;
            return;
        }

        if((auxDiff = VectorUtils.GetProjectionModule(LP2Relative, realSurfaceLimitPoint2 - (Vector2)this.transform.position) - LP2Relative.magnitude) < 0)
        {
            this.transform.position = (Vector2)this.transform.position + LP2Relative.normalized * auxDiff;
            return;
        }
    }

    private void DetermineScale()
    {
        if(Quaternion.FromToRotation(Vector2.up, this.transform.up).eulerAngles.z > 179 )
        {
            this.transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            this.transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private void TeleportPlayer()
    {
        // Con realismo
        /*Vector2 positionRelativeToFirstPortal = this.transform.InverseTransformPoint(player.transform.position);
        player.transform.position = otherPortal.transform.TransformPoint(new Vector2(positionRelativeToFirstPortal.x * -1,positionRelativeToFirstPortal.y));
        player.rbody.velocity = otherPortal.transform.TransformVector(this.transform.InverseTransformVector(velocityFromFrameBefore * -1));*/
        

        player.transform.position = otherPortal.transform.TransformPoint(this.transform.InverseTransformPoint(player.transform.position));
        Vector2 velocityRelativeToFirstPortal = this.transform.InverseTransformVector(velocityFromFrameBefore);
        player.rbody.velocity = otherPortal.transform.TransformVector(new Vector2(velocityRelativeToFirstPortal.x,velocityRelativeToFirstPortal.y * -1));
        audioTpSource.Play();
    }

    private int frameCountAfterTp = 0;

    void Update()
    {
        if (isPortalBlocked == true)
        {
            frameCountAfterTp++;
            if(frameCountAfterTp > 3)
            {
                isPortalBlocked = false;
                frameCountAfterTp = 0;
            }
        }

        //Para que al teletransportarse no se teletranporte con velocidad 0 por haberse chocado con la superficie
        velocityFromFrameBefore = player.rbody.velocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!isPortalBlocked)
            {
                if (otherPortal.activeSelf)
                {
                    otherPortalScript.isPortalBlocked = true;
                    TeleportPlayer();
                }
            }
            else
            {
                isPortalBlocked = false;
            }
            
        }
    }

  

}
