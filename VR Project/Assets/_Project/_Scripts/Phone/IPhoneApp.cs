
using System.Collections;
using DG.Tweening;
using UnityEngine;

public interface IPhoneApp
{
    public void StartApp();
    
    public Tween ExitApp();
    
    public void OnJoystickMove(float x, float y);
    
    public void OnThumbStickDown();
}
