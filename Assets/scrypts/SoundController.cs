using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    [SerializeField] public List<AudioSource> music = new List<AudioSource>();
    [SerializeField] public List<AudioSource> Sound = new List<AudioSource>();
    [SerializeField] public SettingsSave settings;
    [SerializeField] Slider volumeSlider;
    [SerializeField] Slider MusicSlider;
    [SerializeField] Slider FXSlider;
    
    void Start()
    {
        load();
        volumeSlider.value = settings.volume;
        MusicSlider.value = settings.musicVolume;
        FXSlider.value = settings.FXVolume;
    }

    public void setVolume(Slider volume)
    {
        settings.volume = volume.value;
        load();
    }

    public void setMusicVolume(Slider volume)
    {
        settings.musicVolume = volume.value;
        load();
    }

    public void setFXVolume(Slider volume)
    {
        settings.FXVolume = volume.value;
        load();
    }

    private void load()
    {
        foreach (AudioSource i in music)
        {
            i.volume = settings.musicVolume * settings.volume;
        }foreach (AudioSource i in Sound)
        {
            i.volume = settings.FXVolume * settings.volume;
        }
    }
}
