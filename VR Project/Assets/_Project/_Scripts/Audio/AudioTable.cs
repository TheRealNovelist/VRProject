using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public struct AudioFile
{
    public string name;
    [Range(0f, 1f)] public float volume;
    public float pitch;
    public List<AudioClip> clips;

    public AudioFile(string name)
    {
        this.name = name;
        volume = 1f;
        pitch = 1f;
        clips = null;
    }
}

public class AudioTable : ScriptableObject
{
    [SerializeField] private AudioMixerGroup _mixerGroup;
    
    
}
