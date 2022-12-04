using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioCustomSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    public float GetVolume(string type)
    {
        if (mixer.GetFloat(type, out var dummy)) 
            return PlayerPrefs.GetFloat(type, 1);
        
        Debug.Log("AudioSetting: Mixer does not contain " + type);
        return default;
    }

    public void SetVolume(string type, float value)
    {
        if (!mixer.GetFloat(type, out var dummy))
        {
            Debug.Log("AudioSetting: Mixer does not contain " + type);
            return;
        }

        mixer.SetFloat(type, Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat(type, value);
    }
}
