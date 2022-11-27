
using System.Collections;
using DG.Tweening;
using UnityEngine;

public interface IPhoneNavigation
{
    public void Navigate(float x, float y);

    public void StartNavigation();
    
    public void Confirm();

    public Tween EndNavigation();
}
