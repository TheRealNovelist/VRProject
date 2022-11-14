using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PanelBlocker : MonoBehaviour
{
    private Image _blocker => GetComponent<Image>();
    
    public Tween SetBlockerAlpha(float from, float to, float duration, bool active = false)
    {
        gameObject.SetActive(true);
        SetBlockerAlpha(from);
        return _blocker.DOFade(to, duration).SetUpdate(true)
            .OnComplete(() =>
            {
                if (!active)
                    gameObject.SetActive(false);
            });
    }
    
    public void SetBlockerAlpha(float alpha)
    {
        Color temp = _blocker.color;
        temp.a = alpha;
        _blocker.color = temp;
    }
}
