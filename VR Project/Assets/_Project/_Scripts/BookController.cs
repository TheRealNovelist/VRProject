using System;
using System.Collections;
using System.Collections.Generic;
using BNG;
using echo17.EndlessBook;
using UnityEngine;

public class BookController : GrabbableEvents
{
    public EndlessBook book => GetComponent<EndlessBook>();
    private bool _allowNavigation = false;
    private ControllerHand _handSide;
    
    [SerializeField] private List<MonoBehaviour> _componentsToDisable;
    
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

        if (book.CurrentState != EndlessBook.StateEnum.OpenMiddle) return;
        if (!book.IsLastPageGroup && xAxis > 0)
        {
            book.TurnToPage(book.CurrentPageNumber + 2, EndlessBook.PageTurnTimeTypeEnum.TimePerPage, 0.5f);
        }
        else if (!book.IsFirstPageGroup && xAxis < 0)
        {
            book.TurnToPage(book.CurrentPageNumber - 2, EndlessBook.PageTurnTimeTypeEnum.TimePerPage, 0.5f);
        }
    }

    public override void OnTriggerDown()
    {
        _allowNavigation = true;
        foreach (var component in _componentsToDisable)
        {
            component.enabled = false;
        }
        
        book.SetState(EndlessBook.StateEnum.OpenMiddle);
    }

    public override void OnTriggerUp()
    {
        _allowNavigation = false;
        foreach (var component in _componentsToDisable)
        {
            component.enabled = true;
        }
        book.SetState(EndlessBook.StateEnum.ClosedFront);
    }
    
    public override void OnGrab(Grabber grabber)
    {
        _handSide = grabber.HandSide;
    }
    
    public override void OnRelease()
    {
        if (book.CurrentState == EndlessBook.StateEnum.OpenMiddle)
        {
            book.SetState(EndlessBook.StateEnum.ClosedFront);
        }
    }
}
