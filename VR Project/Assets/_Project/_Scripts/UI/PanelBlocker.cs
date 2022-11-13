using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PanelBlocker : MonoBehaviour
{
    private Image _blocker => GetComponent<Image>();
    
    public Tween Fade(float from, float to, float duration)
    {
        SetBlockerAlpha(from);
        return _blocker.DOFade(to, duration).SetUpdate(true);
    }
    
    public void SetBlockerAlpha(float alpha)
    {
        Color temp = _blocker.color;
        temp.a = alpha;
        _blocker.color = temp;
    }
}
