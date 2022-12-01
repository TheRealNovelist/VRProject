using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioTable audioTable;
    [SerializeField] private AudioSource source;
    
    // Start is called before the first frame update
    private void Awake()
    {
        if (!source)
        {
            GameObject audioHolder = new GameObject();
            source = audioHolder.AddComponent<AudioSource>();
            
            SetUpSource();
        }
    }

    // Update is called once per frame
    private void SetUpSource()
    {
        source.outputAudioMixerGroup = audioTable.mixerGroup;
    }

    private void Play(string audioName)
    {
        AudioFile file = audioTable.FindAudioByName(audioName);
        
        if (file == null)
        {
            DebugMessage(audioName);
            return;
        }

        source.volume = file.volume;
        source.pitch = file.pitch;
        source.clip = file.GetAudioClip();
        
        source.Play();
    }

    private void Unpause()
    {
        if (source.clip && !source.isPlaying)
            source.UnPause();
    }
    
    private void Pause()
    {
        if (source.isPlaying)
            source.Pause();
    }
    
    private void Stop()
    {
        if (source.isPlaying)
            source.Stop();
    }

    private void PlayOneShot(string audioName)
    {
        AudioFile file = audioTable.FindAudioByName(audioName);
        
        if (file == null)
        {
            DebugMessage(audioName);
            return;
        }

        source.pitch = file.pitch;
        source.PlayOneShot(file.GetAudioClip(), file.volume);
    }
    
#if UNITY_EDITOR
    private void DebugMessage(string message)
    {
        Debug.Log($"[{gameObject.name}] AudioController: Sound ({message}) is not found.");
      
    }
    
    private void OnValidate()
    {
        if (!source || !audioTable)
        {
            SetUpSource();
        }
    }
#endif
}
