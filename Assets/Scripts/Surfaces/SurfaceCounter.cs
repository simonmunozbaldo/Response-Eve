using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SurfaceCounter : MonoBehaviour
{
    public int counter;
    [Header ("References")]
    public TextMeshProUGUI textMeshCounter;

    void Start()
    {
        textMeshCounter.text = counter + "";
    }
    public void SubtractToCounter(int quantity)
    {
        counter -= quantity;
        textMeshCounter.text = counter + "";
    }
}
