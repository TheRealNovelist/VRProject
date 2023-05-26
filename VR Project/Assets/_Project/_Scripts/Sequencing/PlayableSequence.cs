using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class PlayableSequence : MonoBehaviour
{
    [SerializeField] private List<PlayableAsset> sequence;
    [SerializeField] private int index;
    [SerializeField] private bool playOnAwake;
    
    private PlayableDirector Director => GetComponent<PlayableDirector>();

    private void Awake()
    {
        if (playOnAwake) PlayCurrent();
    }

    public void PlayCurrent()
    {
        Director.Play(sequence[index]);
    }
    
    public void PlayNext()
    {
        int nextIndex = index += 1;
        Director.Play(sequence[nextIndex]);
    }
}
