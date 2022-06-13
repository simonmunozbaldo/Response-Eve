using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricPosts : MonoBehaviour
{
    public float electricSpriteRepeatDistance;
    [Header("References")]
    public Transform point1;
    public Transform point2;
    public GameObject electricity;
    public LineRenderer auraLine;
    public BoxCollider2D hitboxCollider;

    private float distanceBetweenPts => Vector2.Distance(point1.position, point2.position);
    // Start is called before the first frame update
    void Start()
    {
        PutElectricity();
        PutAuraLine();
        PutHitbox();
    }
    
    void PutElectricity()
    {
        electricity.transform.position = point1.position;
        electricity.transform.localRotation = Quaternion.FromToRotation(electricity.transform.right, point2.position - point1.position) * electricity.transform.localRotation;
        int numberOfReps = Mathf.FloorToInt(distanceBetweenPts / electricSpriteRepeatDistance);
        float sizeWithoutStretch = numberOfReps * electricSpriteRepeatDistance;
        electricity.GetComponent<SpriteRenderer>().size = new Vector2(sizeWithoutStretch, 2.56f);
        electricity.transform.localScale = new Vector3( distanceBetweenPts / sizeWithoutStretch, 0.4103171f, 1f);
    }

    void PutAuraLine()
    {
        auraLine.SetPosition(0, point1.position);
        auraLine.SetPosition(1, point2.position);
    }

    void PutHitbox()
    {
        hitboxCollider.transform.position = point1.position;
        hitboxCollider.transform.localRotation = Quaternion.FromToRotation(hitboxCollider.transform.up, point2.position - point1.position) * hitboxCollider.transform.localRotation;
        hitboxCollider.size = new Vector2(0.3749752f, distanceBetweenPts);
        hitboxCollider.offset = new Vector2(0f, distanceBetweenPts / 2f);
    }
}
