using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour {

   [Range(0,1), SerializeField] private float MasterVolume = 1;
   [Range(0, 1), SerializeField] private float SFXVolume = 1;
   [Range(0, 1), SerializeField] private float MusicVolume = 1;

   public AudioSource SFXSource;
   public AudioSource MusicSource;

    public void Awake() {
        SoundEffectProfile.am = this;
        MasterVolume = PlayerPrefs.GetFloat("MasterVol", 1);
        SFXVolume = PlayerPrefs.GetFloat("SFXVolume", 1);
        MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1);
    }

    public float getMasterVolume()
    {
        return MasterVolume;
    }

    public float getSFXVolume()
    {
        return  SFXVolume;
    }

    public float getMusicVolume()
    {
        return MusicVolume;
    }

    public float getScaleSFXVolume() {
        return MasterVolume * SFXVolume;
    }

    public float getScaledMusicVolume()
    {
        return MasterVolume * MusicVolume;
    }

    public void setMasterVolume(float volume) {
        MasterVolume = volume;
        PlayerPrefs.SetFloat("MasterVol", volume);
        MusicSource.volume = getScaledMusicVolume();
    }

    public void setMusicVolume(float volume)
    {
        MusicVolume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
        MusicSource.volume = getScaledMusicVolume();
    }

    public void setSFXVolume(float volume)
    {
        PlayerPrefs.SetFloat("SFXVol", volume);
        SFXVolume = volume;
    }
}