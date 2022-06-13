using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SliderNumberUpdater : MonoBehaviour
{
    public TextMeshProUGUI textM;
    public Slider slider;
    public AudioSource audioSource;

    private void Awake()
    {
       
    }

    public void UpdateTextFromSlider()
    {
        textM.text = slider.value + "";
    }

    public void PlaySliderAudio()
    {
        audioSource.Play();
    }
    
}
