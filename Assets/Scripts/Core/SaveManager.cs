using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{

    
    void Start()
    {
        LoadPlayerPreferences();
    }

    public void SavePlayerPreferences()
    {
        PlayerPrefs.SetFloat("MasterVolume", AudioManager.Instance.masterVolume);
        PlayerPrefs.SetFloat("BGMVolume", AudioManager.Instance.bgmVolume);
        PlayerPrefs.SetFloat("SFXVolume", AudioManager.Instance.sfxVolume);
        Debug.Log("player preferences saved");

    }

    public void LoadPlayerPreferences()
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            AudioManager.Instance.masterVolume = PlayerPrefs.GetFloat("MasterVolume");
            AudioManager.Instance.OnMasterVolumeChange(AudioManager.Instance.masterVolume);
        }
        if (PlayerPrefs.HasKey("BGMVolume"))
        {
            AudioManager.Instance.bgmVolume = PlayerPrefs.GetFloat("BGMVolume");
            AudioManager.Instance.OnBGMVolumeChange(AudioManager.Instance.bgmVolume);
        }
        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            AudioManager.Instance.sfxVolume = PlayerPrefs.GetFloat("SFXVolume");
            AudioManager.Instance.OnSFXVolumeChange(AudioManager.Instance.sfxVolume);
        }

    }


}