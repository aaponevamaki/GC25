using UnityEngine;

[System.Serializable]
public class AudioObject
{
    public string id;
    public AudioClip audioClip;
    [Range(0f, 1f)] public float volume = 1f;
}
