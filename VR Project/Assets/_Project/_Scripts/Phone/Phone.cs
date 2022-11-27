using System;
using System.Collections;
using System.Collections.Generic;
using BNG;
using DG.Tweening;
using UnityEngine;

public enum PhoneOrientation
{
    None,
    Vertical,
    Horizontal
}

[SelectionBase]
public class Phone : GrabbableEvents
{
    public GameObject verticalPhone;
    public GameObject horizontalPhone;
    public AppManager app;
    
    [Space]
    [SerializeField] private List<MonoBehaviour> _componentsToDisable;
    
    private ControllerHand _handSide;
    private IPhoneNavigation _currentNav;
    private PhoneOrientation _currentOrientation = PhoneOrientation.None;
    private bool _allowNavigation = false;
    private void Update()
    {
        if (_allowNavigation)
        {
            float xAxis = 0f;
            float yAxis = 0f;
            bool isThumbStickDown = false;
            bool isReturnButtonDown = false;

            switch (_handSide)
            {
                case ControllerHand.Left:
                    xAxis = InputBridge.Instance.GetInputAxisValue(InputAxis.LeftThumbStickAxis).x;
                    yAxis = InputBridge.Instance.GetInputAxisValue(InputAxis.LeftThumbStickAxis).y;
                    isThumbStickDown = InputBridge.Instance.GetControllerBindingValue(ControllerBinding.LeftThumbstickDown);
                    isReturnButtonDown = InputBridge.Instance.GetControllerBindingValue(ControllerBinding.YButton);
                    break;
                case ControllerHand.Right:
                    xAxis = InputBridge.Instance.GetInputAxisValue(InputAxis.RightThumbStickAxis).x;
                    yAxis = InputBridge.Instance.GetInputAxisValue(InputAxis.RightThumbStickAxis).y;
                    isThumbStickDown = InputBridge.Instance.GetControllerBindingValue(ControllerBinding.RightThumbstickDown);
                    isReturnButtonDown = InputBridge.Instance.GetControllerBindingValue(ControllerBinding.BButton);
                    break;
                default:
                    Debug.Log("Phone: Hand not valid!");
                    break;
            }
            
            _currentNav?.Navigate(xAxis, yAxis);
            
            if (isThumbStickDown)
                _currentNav?.Confirm();

            if (isReturnButtonDown)
            {
                Tween tween = _currentNav?.EndNavigation();
                if (tween != null)
                    tween.OnComplete(() => SetNav(app));
                else
                    SetNav(app);
            }
        }
    }

    public void SetNav(IPhoneNavigation newNav)
    {
        _currentNav = newNav;
    }

    public void SwitchOrientation(PhoneOrientation orientation)
    {
        if (orientation == _currentOrientation)
            return;
        
        switch (orientation)
        {
            case PhoneOrientation.Vertical:
                verticalPhone.SetActive(true);
                horizontalPhone.SetActive(false);
                _currentOrientation = PhoneOrientation.Vertical;
                break;
            case PhoneOrientation.Horizontal:
                verticalPhone.SetActive(false);
                horizontalPhone.SetActive(true);
                _currentOrientation = PhoneOrientation.Horizontal;
                break;
        }
    }
    
    public override void OnGrab(Grabber grabber)
    {
        base.OnGrab(grabber);

        _handSide = grabber.HandSide;
        
        SwitchOrientation(PhoneOrientation.Vertical);
        SetNav(app);
        app.Init(this);
    }

    public override void OnRelease()
    {
        base.OnRelease();

        _currentOrientation = PhoneOrientation.None;
        _currentNav.EndNavigation();
        _currentNav = app;
    }

    public override void OnTriggerDown()
    {
        _allowNavigation = true;
        foreach (var component in _componentsToDisable)
        {
            component.enabled = false;
        }
    }

    public override void OnTriggerUp()
    {
        _allowNavigation = false;
        foreach (var component in _componentsToDisable)
        {
            component.enabled = true;
        }
    }
}
