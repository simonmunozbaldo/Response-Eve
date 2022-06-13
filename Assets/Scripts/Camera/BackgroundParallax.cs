using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    public Transform pipes;
    public float pipesParallax;
    public Transform begginingPoint;
    public Transform cameraPoint;
    public Transform airDucts;
    public float airDuctsParallax;
    

    private float distanceX => cameraPoint.position.x - begginingPoint.position.x;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        pipes.localPosition = new Vector2(distanceX * pipesParallax, 0f);
        airDucts.localPosition = new Vector2(distanceX * airDuctsParallax, 0f);
    }
}
