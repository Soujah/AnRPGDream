using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public Slider musicSlider;
    public Slider SFXSlider;

    public float musicVolume;
    public float SFXVolume;


    public AudioSource[] MusicSources;
    public AudioSource[] SFXSources;


    private void Start()
    {
        LoadVolumeSettings();

        musicSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        SFXSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });


    }

    public void SaveVolumeSettings()
    {
        //save volume settings to player prefs

        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("SFXVolume", SFXVolume);

    }

    public void LoadVolumeSettings()
    {
        //load volume settings when game starts
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0f);
        SFXVolume = PlayerPrefs.GetFloat("SFXVolume", 0f);

        SetAudioSources();

        musicSlider.value = musicVolume;
        SFXSlider.value = SFXVolume;

    }

    public void ValueChangeCheck()
    {
        musicVolume = musicSlider.value;
        SFXVolume = SFXSlider.value;

        SetAudioSources();

        SaveVolumeSettings();
    }

    private void SetAudioSources()
    {
        foreach (AudioSource item in MusicSources)
        {
            item.volume = musicVolume;
        }

        foreach (AudioSource item in SFXSources)
        {
            item.volume = SFXVolume;
        }
    }
}
