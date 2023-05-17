using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PhoneLockScreenHelper : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Phone phone;

    private void Awake()
    {
        if (!phone) phone = GetComponentInParent<Phone>(true);
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        phone.TurnOn();
    }
}
