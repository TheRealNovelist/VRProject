using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

//Serve as a blank page as well
public abstract class App : MonoBehaviour
{
    protected UIAnimation anim;
    protected CanvasGroup canvasGrp;

    protected virtual void Awake()
    {
        if (!TryGetComponent(out anim))
            anim = gameObject.AddComponent<UIAnimation>();
        
        if (!TryGetComponent(out canvasGrp))
            canvasGrp = gameObject.AddComponent<CanvasGroup>();
    }

    public abstract void Enter(float transitionDuration = 0);

    public abstract void Exit(float transitionDuration = 0);
}
