using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scope : MonoBehaviour
{
    [HideInInspector] public static Scope instance;
    public Camera mainCamera;

    [Header ("Visuals")]
    public List<SpriteRenderer> listOfAuras;
    public SpriteRenderer mistSprite;

    private string lastTag;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = (Vector2) mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    public void ChangeColor(string tag,Tool tool)
    {
        mistSprite.enabled = (tag == "Mist");

        if(tool == null)
        {
            TurnOffAuras();
        }
        else
        {
            PutColorOnAuras(tag, tool.colorForScope);
        }

    }

    private void TurnOffAuras()
    {
        foreach (SpriteRenderer aura in listOfAuras)
        {
            aura.enabled = false;
        }
    }

    private void PutColorOnAuras(string tag,Color color)
    {
        foreach (SpriteRenderer aura in listOfAuras)
        {
            aura.enabled = true;
            aura.color = color;
        }
        
    }

    
}
