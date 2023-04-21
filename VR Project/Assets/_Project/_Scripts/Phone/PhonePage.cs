
using System.Collections;
using DG.Tweening;
using UnityEngine;

public abstract class PhonePage : MonoBehaviour
{
    public abstract void StartPage(Phone phone);
    
    public abstract void ExitPage(Phone phone);

    public virtual void OnJoystickMove(float x, float y)
    {
        
    }

    public virtual void OnThumbStickDown()
    {
        
    }
}
