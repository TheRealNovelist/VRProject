using System;
using System.Collections;
using System.Collections.Generic;
using BNG;
using DG.Tweening;
using UnityEngine;

[SelectionBase]
public class Phone : GrabbableEvents
{
    public GameObject verticalPhone;
    public Phone_AppIconManager app;
    
    [Space]
    [SerializeField] private List<MonoBehaviour> _componentsToDisable;
    
    private ControllerHand _handSide;
    private bool _allowNavigation = false;
    
    private IPhoneApp _currentNav;
    
    private void Update()
    {
        if (!_allowNavigation) 
            return;
        
        var xAxis = 0f;
        var yAxis = 0f;
        var isThumbStickDown = false;
        var isReturnButtonDown = false;

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
            case ControllerHand.None:
            default:
                Debug.Log("Phone: Hand not valid!");
                break;
        }
            
        _currentNav?.OnJoystickMove(xAxis, yAxis);
            
        if (isThumbStickDown)
            _currentNav?.OnThumbStickDown();

        if (isReturnButtonDown)
        {
            Tween tween = _currentNav?.ExitApp();
            SetNav(null);
            if (tween != null)
                tween.OnComplete(() => SetNav(app));
            else
                SetNav(app);
        }
    }

    public void SetNav(IPhoneApp newNav)
    {
        _currentNav = newNav;
    }
    
    public override void OnGrab(Grabber grabber)
    {
        _handSide = grabber.HandSide;
        
        SetNav(app);
        verticalPhone.SetActive(true);
        app.Init(this);
    }

    public override void OnRelease()
    {
        _currentNav?.ExitApp();
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
