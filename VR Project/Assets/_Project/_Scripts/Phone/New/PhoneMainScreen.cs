using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PhoneMainScreen : App
{
    public override void Enter(float transitionDuration = 0)
    {
        gameObject.SetActive(true);
        
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(transitionDuration);
        sequence.OnComplete(() => canvasGrp.interactable = true);
        sequence.Play();
    }

    public override void Exit(float transitionDuration = 0)
    {
        canvasGrp.interactable = false;
        
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(transitionDuration);
        sequence.OnComplete(() => gameObject.SetActive(false));
        sequence.Play();
    }
}
