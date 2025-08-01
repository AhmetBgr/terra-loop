using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundController : Singleton<SoundController>
{
    private List<AudioSource> _audioSources = new List<AudioSource>();
    private bool _isOn = true;

    protected override void Awake()
    {
        base.Awake();
        _audioSources.Add(GetComponent<AudioSource>());

    }
    private void OnDestroy()
    {
        StopAllSounds();
    }
    private void StopAllSounds()
    {
        foreach (AudioSource source in _audioSources)
        {
            if (source.isPlaying)
            {
                source.Stop();
            }
        }
    }
    public AudioSource PlayAudio(AudioClip audio, float volume = 1f, float pitch = 1f, bool loop = false)
    {
        if (!_isOn)
            return null;

        if (!audio)
            return null;

        AudioSource audioSource = GetFreeAudioSource();
        SetAudioSource(audioSource, audio, loop, volume, pitch);

        audioSource.Play();

        return audioSource;
    }
    public void PlayAudio(SoundEffect soundEffect)
    {
        if (!_isOn)
            return;

        if (!soundEffect)
            return;

        AudioSource audioSource = GetFreeAudioSource();
        soundEffect.Play(audioSource);
    }

    public IEnumerator FadeOutSound(AudioSource source, float dur, float delay)
    {
        yield return new WaitForSeconds(delay);

        float startVolume = source.volume;
        float elapsedTime = 0f;

        while (elapsedTime < dur)
        {
            elapsedTime += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / dur);
            yield return null;
        }

        source.volume = 0f;
        source.Stop();
    }
    private AudioSource GetFreeAudioSource()
    {
        for (int i = 0; i < _audioSources.Count; i++)
        {
            if (!_audioSources[i].isPlaying)
            {
                return _audioSources[i];
            }
        }
        return GetNewAudioSource();
    }

    private void SetAudioSource(AudioSource audioSource, AudioClip audio, bool loops, float volume, float pitch)
    {
        audioSource.loop = loops;
        audioSource.clip = audio;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
    }

    private AudioSource GetNewAudioSource()
    {
        AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
        _audioSources.Add(newAudioSource);
        newAudioSource.playOnAwake = false;

        return newAudioSource;
    }

    public void SetIsOn(bool isOn)
    {
        _isOn = isOn;
    }
}
