using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AppManager : MonoBehaviour, IPhoneNavigation
{
    [SerializeField] private float scrollSpeed = 0.15f;
    [SerializeField] private App initialApp;
    
    private App _currentApp;
    private bool _allowScroll = true;
    private Phone _phone;
    
    public void Init(Phone phone)
    {
        if (_currentApp) _currentApp.Deselect();
        
        initialApp.Select();
        _currentApp = initialApp;
        _phone = phone;
    }
    
    public void Navigate(float x, float y)
    {
        int xInt = Mathf.RoundToInt(x);
        int yInt = Mathf.RoundToInt(y);
        
        Sequence timer = DOTween.Sequence();

        if (!_allowScroll)
            return;
        
        _allowScroll = false;
        timer.AppendInterval(scrollSpeed).OnComplete(() => _allowScroll = true);
        
        App nextApp = (xInt, yInt) switch
        {
            (0, 1) => _currentApp.FindSelectableOnUp() as App,
            (0, -1) => _currentApp.FindSelectableOnDown() as App,
            (1, 0) => _currentApp.FindSelectableOnRight() as App,
            (-1, 0) => _currentApp.FindSelectableOnLeft() as App,
            _ => _currentApp
        };

        if (!nextApp)
            return;
        
        _currentApp.Deselect();
        _currentApp = nextApp;
        _currentApp.Select();
    }

    public void StartNavigation()
    {
        
    }

    public void Confirm()
    {
        _currentApp.Confirm(_phone);
    }

    public Tween EndNavigation()
    {
        return null;
    }
}
