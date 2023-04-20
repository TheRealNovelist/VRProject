using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Phone_AppIconManager : MonoBehaviour, IPhoneApp
{
    [SerializeField] private float scrollSpeed = 0.15f;
    [SerializeField] private AppIcon initialApp;
    
    private AppIcon _currentApp;
    private bool _allowScroll = true;
    private Phone _phone;
    
    public void Init(Phone phone)
    {
        if (_currentApp) _currentApp.Deselect();
        
        initialApp.Select();
        _currentApp = initialApp;
        _phone = phone;
    }
    
    public void OnJoystickMove(float x, float y)
    {
        int xInt = Mathf.RoundToInt(x);
        int yInt = Mathf.RoundToInt(y);
        
        Sequence timer = DOTween.Sequence();

        if (!_allowScroll)
            return;
        
        _allowScroll = false;
        timer.AppendInterval(scrollSpeed).OnComplete(() => _allowScroll = true);
        
        AppIcon nextApp = (xInt, yInt) switch
        {
            (0, 1) => _currentApp.FindSelectableOnUp() as AppIcon,
            (0, -1) => _currentApp.FindSelectableOnDown() as AppIcon,
            (1, 0) => _currentApp.FindSelectableOnRight() as AppIcon,
            (-1, 0) => _currentApp.FindSelectableOnLeft() as AppIcon,
            _ => _currentApp
        };

        if (!nextApp)
            return;
        
        _currentApp.Deselect();
        _currentApp = nextApp;
        _currentApp.Select();
    }

    public void StartApp()
    {
        
    }

    public void OnThumbStickDown()
    {
        _currentApp.Confirm(_phone);
    }

    public Tween ExitApp()
    {
        return null;
    }
}
