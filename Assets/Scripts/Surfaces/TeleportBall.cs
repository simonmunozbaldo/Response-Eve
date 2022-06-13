using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportBall : MonoBehaviour
{
    [HideInInspector] public Vector2 direction;
    public float speed;
    public Transform firePoint;
 
    private Rigidbody2D _rbody;
    private void Awake()
    {
        _rbody = this.GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        Fire();
    }

    void OnEnable()
    {
        Fire();
    }

    private void Fire()
    {
        this.transform.position = firePoint.position;
        _rbody.velocity = direction.normalized * speed;
        this.transform.rotation = this.transform.rotation * Quaternion.FromToRotation(this.transform.right, direction);
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        this.gameObject.SetActive(false);
    }

}
