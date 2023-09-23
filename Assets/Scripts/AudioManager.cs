using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public const string MASTERVOLUME = "MasterVolumeParameter";
    public const string SFXVOLUME = "SFXVolumeParameter";
    public const string BGMVOLUME = "BGMVolumeParameter";

    public float masterVolume = 1.0f;
    public float bgmVolume = 1.0f;
    public float sfxVolume = 1.0f;

    public AudioMixer audioMixer;
    public AudioSource audioSource;
    [Header("Sounds")]
    public AudioClip sfxMenuButton;
    public AudioClip sfxMachineFire;
    public AudioClip sfxMachineHurt;
    public AudioClip sfxMachineDeath;
    public AudioClip sfxPickupGet;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Attempted to create a second audio manager");
            Destroy(this);
        }
    }

    private float ConvertToDecibel(float value)
    {
        float newVolume = value;
        if (newVolume <= 0)
        {
            newVolume = -80;
        }
        else
        {
            newVolume = Mathf.Log10(newVolume);
            newVolume = newVolume * 20;
        }

        return newVolume;
    }

    public void OnMasterVolumeChange(float value)
    {
        masterVolume = Mathf.Clamp01(value);
        float newVolume = ConvertToDecibel(value);

        audioMixer.SetFloat(MASTERVOLUME, newVolume);
    }

    public void OnSFXVolumeChange(float value)
    {
        sfxVolume = Mathf.Clamp01(value);
        float newVolume = ConvertToDecibel(value);

        audioMixer.SetFloat(SFXVOLUME, newVolume);
    }

    public void OnBGMVolumeChange(float value)
    {
        bgmVolume = Mathf.Clamp01(value);
        float newVolume = ConvertToDecibel(value);

        audioMixer.SetFloat(BGMVOLUME, newVolume);
    }


}