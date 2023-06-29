using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PhoneLockScreenHelper : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Phone phone;
    [SerializeField] private TMP_Text text;

    private void Awake()
    {
        if (!phone) phone = GetComponentInParent<Phone>(true);
        LevelController.OnLevelChanged += ChangeText;
        ChangeText(LevelController.CurrentLevel);
    }

    private void OnDestroy()
    {
        LevelController.OnLevelChanged -= ChangeText;
    }

    private void ChangeText(Level level)
    {
        text.text = "Current Timeline: " + level;
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        phone.TurnOn();
    }
}
