using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceAudioManager : AudioManager
{
    [SerializeField] private List<string> sequence;

    private int currentIndex = 0;

    public override void Play(string audioName)
    {
        if (!sequence.Contains(audioName)) return;
        var index = sequence.IndexOf(audioName);
        if (currentIndex != index) return;
        
        Debug.Log("Playing " + audioName);
        if (source.isPlaying)
            Stop();

        currentIndex++;
        base.Play(audioName);
    }
}
