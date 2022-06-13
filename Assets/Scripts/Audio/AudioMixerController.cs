using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioMixerController : MonoBehaviour
{
    //string _volumeParameter = "Master";
    public AudioMixer musicMixer;
    public AudioMixer sfxMixer;

    public void SetSFXVolume(float percentage)
    {
        sfxMixer.SetFloat("Volume", LinearToDecibel(percentage / 100f));
    }

    public void SetMusicVolume(float percentage)
    {
        musicMixer.SetFloat("Volume", LinearToDecibel(percentage/100f));
        
    }


    private float LinearToDecibel(float linear)
    {
        float dB;
        if (linear != 0)
            dB = 20.0f * Mathf.Log10(linear);
        else
            dB = -80.0f;
        return dB;
    }
}
