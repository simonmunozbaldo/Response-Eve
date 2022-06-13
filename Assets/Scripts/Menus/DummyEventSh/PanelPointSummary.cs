using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelPointSummary : MonoBehaviour
{
    private PointSummaryController ptSC;
    private void Awake()
    {
        ptSC = this.GetComponentInParent<PointSummaryController>();
    }

    public void StartDeathCounter()
    {
        ptSC.StartDeathCounter();
    }
}
