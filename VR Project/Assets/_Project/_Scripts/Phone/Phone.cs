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
    public PhonePage startingPage;
    
    [Space]
    [SerializeField] private List<MonoBehaviour> _componentsToDisable;
    
    private ControllerHand _handSide;
    private bool _allowNavigation = false;
    
    private PhonePage _currentPage;

    private Stack<PhonePage> activePages;

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
                xAxis = input.GetInputAxisValue(InputAxis.LeftThumbStickAxis).x;
                yAxis = input.GetInputAxisValue(InputAxis.LeftThumbStickAxis).y;
                isThumbStickDown = input.GetControllerBindingValue(ControllerBinding.LeftThumbstickDown);
                isReturnButtonDown = input.GetControllerBindingValue(ControllerBinding.YButton);
                break;
            case ControllerHand.Right:
                xAxis = input.GetInputAxisValue(InputAxis.RightThumbStickAxis).x;
                yAxis = input.GetInputAxisValue(InputAxis.RightThumbStickAxis).y;
                isThumbStickDown = input.GetControllerBindingValue(ControllerBinding.RightThumbstickDown);
                isReturnButtonDown = input.GetControllerBindingValue(ControllerBinding.BButton);
                break;
            case ControllerHand.None:
            default:
                Debug.Log("Phone: Hand not valid!");
                break;
        }
            
        _currentPage.OnJoystickMove(xAxis, yAxis);
            
        if (isThumbStickDown)
            _currentPage.OnThumbStickDown();

        if (isReturnButtonDown)
        {

        }
    }

    
    
    public override void OnGrab(Grabber grabber)
    {
        _handSide = grabber.HandSide;
        
        verticalPhone.SetActive(true);
    }

    public override void OnRelease()
    {
        
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
