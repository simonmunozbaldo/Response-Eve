using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpcionesPersistanceManager : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;
    public AudioMixerController audioMixerController;
    private float musicPercentValue;
    private float sfxPercentValue;
    // Start is called before the first frame update
    void Start()
    {
        musicPercentValue = PlayerPrefs.GetFloat("musicValue", 100);
        sfxPercentValue = PlayerPrefs.GetFloat("sfxValue", 100);

        if(musicSlider != null) //En menu
        {
            musicSlider.value = musicPercentValue;
            sfxSlider.value = sfxPercentValue;

            UpdateMusicVolume();
            UpdateSFXVolume();
        }
        else //En nivel
        {
            audioMixerController.SetSFXVolume(sfxPercentValue);
            audioMixerController.SetMusicVolume(musicPercentValue);
        }
        
        
    }

    public void SaveOptionsSettings()
    {
        PlayerPrefs.SetFloat("musicValue", musicSlider.value);
        PlayerPrefs.SetFloat("sfxValue", sfxSlider.value);
        PlayerPrefs.Save();
    }

    public void UpdateSFXVolume()
    {
        audioMixerController.SetSFXVolume(sfxSlider.value);
    }

    public void UpdateMusicVolume()
    {
        audioMixerController.SetMusicVolume(musicSlider.value);
    }
}
