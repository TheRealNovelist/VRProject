using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class CutsceneManager : MonoBehaviour
{
    private PlayableDirector Director => GetComponent<PlayableDirector>();

    public void Play(PlayableAsset scene)
    {
        Director.playableAsset = scene;
        Director.Play();
    }
}
