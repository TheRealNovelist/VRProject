using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField, TabGroup("Setting"), PropertySpace(SpaceBefore = 0, SpaceAfter = 5), Title("Components")] 
    protected AudioSource source;
    [SerializeField, InlineEditor, TabGroup("Setting"), Title("Audio Table"), HideLabel] 
    protected AudioTable audioTable;
    
    // Start is called before the first frame update
    private void Awake()
    {
        if (!source)
        {
            GameObject audioHolder = new GameObject("Audio Source");
            source = audioHolder.AddComponent<AudioSource>();
            
            SetUpSource();
        }
    }

    // Update is called once per frame
    private void SetUpSource()
    {
        source.outputAudioMixerGroup = audioTable.mixerGroup;
    }
    
    [TabGroup("Testing"), Button(ButtonStyle.CompactBox, Expanded = true)]
    public virtual void Play(string audioName)
    {
        AudioFile file = audioTable.FindAudioByName(audioName);
        
        if (file == null)
        {
#if UNITY_EDITOR
            DebugMessage(audioName);
#endif 
            return;
        }

        source.volume = file.volume;
        source.pitch = file.pitch;
        source.clip = file.GetAudioClip();
        
        source.Play();
    }

    [TabGroup("Testing"), Button(ButtonStyle.CompactBox, Expanded = true)]
    public virtual void PlayOneShot(string audioName)
    {
        AudioFile file = audioTable.FindAudioByName(audioName);
        
        if (file == null)
        {
#if UNITY_EDITOR
            DebugMessage(audioName);
#endif 
            return;
        }

        source.pitch = file.pitch;
        source.PlayOneShot(file.GetAudioClip(), file.volume);
    }
    
    [TabGroup("Testing"), Button]
    public virtual void Unpause()
    {
        if (source.clip && !source.isPlaying)
            source.UnPause();
    }
    
    [TabGroup("Testing"), Button]
    public virtual void Pause()
    {
        if (source.isPlaying)
            source.Pause();
    }
    
    [TabGroup("Testing"), Button]
    public virtual void Stop()
    {
        source.Stop();
    }
    
#if UNITY_EDITOR
    private void DebugMessage(string message)
    {
        Debug.Log($"[{gameObject.name}] AudioController: Sound ({message}) is not found.");
      
    }
    
    private void OnValidate()
    {
        if (!source || !audioTable)
            return;
        
        SetUpSource();
    }
#endif
}
