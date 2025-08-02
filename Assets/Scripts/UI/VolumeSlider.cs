
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSlider : MonoBehaviour
{
    public Slider audioSlider;
    public AudioMixer mixer;
    public Image handleRenderer;
    public Sprite[] volumeSprites;

    public static string volumeKey = "vol";
    private float maxValue;

    public void Start()
    {
        maxValue = audioSlider.maxValue;

        float volume = 0.5f; // default

        if (PlayerPrefs.HasKey(volumeKey))
        {
            volume = PlayerPrefs.GetFloat(volumeKey);
        }

        audioSlider.value = volume * maxValue;
        mixer.SetFloat("Master", Mathf.Log10(volume) * 20);
    }

    public void SetMasterVolume(float value)
    {
        handleRenderer.sprite = volumeSprites[Mathf.Clamp((int)value, 0, volumeSprites.Length -1)];
        float volume = Mathf.Clamp(value / maxValue, 0.0001f, 1f);

        mixer.SetFloat("Master", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(volumeKey, volume);
        PlayerPrefs.Save();

        Debug.Log("Master volume level saved.");
    }
}
