using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PhoneLockScreen : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Phone phone;
    [SerializeField] private CanvasGroup canvas;
    [SerializeField] private float fadeDuration;
    
    private void Awake()
    {
        if (!phone) phone = GetComponentInParent<Phone>(true);
    }

    public void TurnOn()
    {
        phone.TurnOn();
        canvas.DOFade(0, fadeDuration).OnComplete(() => gameObject.SetActive(false));
    }

    [Button]
    public void TurnOff()
    {
        gameObject.SetActive(true);
        canvas.DOFade(1, fadeDuration).OnComplete(() => phone.TurnOff());
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        TurnOn();
    }
}
