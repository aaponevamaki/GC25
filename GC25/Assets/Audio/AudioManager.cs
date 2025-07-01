using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private SoundBank _soundBank;
    [SerializeField] private AudioSource _audioObjectPrefab;

    private Dictionary<string, AudioSource> _loopedAudioObjects = new();

    private AudioMixerGroup _sfxGroup;
    private AudioMixerGroup _musicGroup;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        AudioMixerGroup[] groups = _audioMixer.FindMatchingGroups("");
        foreach (AudioMixerGroup group in groups)
        {
            switch (group.name)
            {
                case "SFX":
                    _sfxGroup = group;
                    break;
                case "Music":
                    _musicGroup = group;
                    break;
            }
        }
    }

    public void SetMasterVolume(float level) => _audioMixer.SetFloat("masterVolume", Mathf.Log10(level) * 20f);
    public void SetMusicVolume(float level) => _audioMixer.SetFloat("musicVolume", Mathf.Log10(level) * 20f);
    public void SetSFXVolume(float level) => _audioMixer.SetFloat("sfxVolume", Mathf.Log10(level) * 20f);

    public void PlaySFXClip(string id)
    {
        AudioObject audioObject = _soundBank.GetAudioObject(id);
        AudioSource audioSource= Instantiate(_audioObjectPrefab, Vector3.zero, Quaternion.identity);

        audioSource.clip = audioObject.audioClip;
        audioSource.outputAudioMixerGroup = _sfxGroup;
        audioSource.volume = audioObject.volume;
        audioSource.spatialBlend = 0;
        audioSource.loop = false;

        audioSource.Play();
        Destroy(audioSource.gameObject, audioSource.clip.length);
    }

    public void StartLoop(string id, string group = "SFX", bool useFadeIn = true)
    {
        if (_loopedAudioObjects.ContainsKey(id))
        {
            AudioSource existingAudioSource = _loopedAudioObjects[id];
            if (existingAudioSource.isPlaying) return;

            existingAudioSource.Play();
            return;
        }

        AudioObject audioObject = _soundBank.GetAudioObject(id);
        AudioSource audioSource = Instantiate(_audioObjectPrefab, Vector3.zero, Quaternion.identity);

        audioSource.clip = audioObject.audioClip;
        switch (group)
        {
            case "Music":
                audioSource.outputAudioMixerGroup = _musicGroup;
                break;
            case "SFX":
                audioSource.outputAudioMixerGroup = _sfxGroup;
                break;
        }
        audioSource.volume = audioObject.volume;
        audioSource.spatialBlend = 0;
        audioSource.loop = true;

        if (useFadeIn)
        {
            audioSource.volume = 0;
            audioSource.Play();
            StartCoroutine(FadeAudio(audioSource, audioObject.volume, 0.5f));
        }
        else audioSource.Play();

        _loopedAudioObjects.Add(id, audioSource);
    }

    public void StopLoop(string id, bool useFadeOut = true)
    {
        if (_loopedAudioObjects.ContainsKey(id))
        {
            AudioSource audioSource = _loopedAudioObjects[id];
            if (audioSource.isPlaying)
            {
                if (useFadeOut)
                {
                    StartCoroutine(FadeAudio(audioSource, 0f, 0.5f, stopAfterFade: true));
                }
                else
                {
                    audioSource.Stop();
                    Destroy(audioSource.gameObject);
                }
                _loopedAudioObjects.Remove(id);
            }
        }
    }

    private IEnumerator FadeAudio(AudioSource source, float targetVolume, float duration, bool stopAfterFade = false)
    {
        float startVolume = source.volume;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / duration);
            yield return null;
        }

        source.volume = targetVolume;

        if (stopAfterFade)
        {
            source.Stop();
            Destroy(source.gameObject);
        }
    }
}
