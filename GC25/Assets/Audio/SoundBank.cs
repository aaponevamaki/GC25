using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundBank", menuName = "Audio/Sound Bank")]
public class SoundBank : ScriptableObject
{
    public List<AudioObject> audioObjects = new();

    public AudioObject GetAudioObject(string id)
    {
        return audioObjects.Find(audio => audio.id == id);
    }
}
