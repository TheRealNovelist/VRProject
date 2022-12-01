using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;


[System.Serializable] 
public struct AudioFile
{
    public string name;
    
    [BoxGroup("Settings")]
    [Range(0f, 1f)] public float volume;
    [BoxGroup("Settings")]
    [Range(-3f, 3f)] public float pitch;
    
    [HideLabel] public WeightedRandomList<AudioClip> clips;

    public AudioFile(string name, WeightedRandomList<AudioClip> clips)
    {
        this.name = name;
        volume = 1f;
        pitch = 1f;
        this.clips = clips;
    }
}

[CreateAssetMenu(menuName = "System/AudioTable", fileName = "New Audio Table")]
public class AudioTable : SerializedScriptableObject
{
    [SerializeField] private AudioMixerGroup _mixerGroup;

    [SerializeField, Searchable] private List<AudioFile> _files;
}
