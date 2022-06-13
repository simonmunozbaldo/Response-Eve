using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class VoidEvent : UnityEvent { }


public class SliderEventHandler : MonoBehaviour, IPointerUpHandler
{
    public SliderNumberUpdater sliderNumberUpdater;
    public void OnPointerUp(PointerEventData eventData)
    {
        sliderNumberUpdater.PlaySliderAudio();
    }

    
}
