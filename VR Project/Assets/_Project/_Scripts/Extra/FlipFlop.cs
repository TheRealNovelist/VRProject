using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FlipFlop : MonoBehaviour
{
    public UnityEvent eventOn;
    public UnityEvent eventOff;

    
    
    private bool isOn;
    
    public void TriggerEvent()
    {
        if (isOn)
        {
            eventOn.Invoke();
            isOn = false;
        }
        else
        {
            eventOff.Invoke();
            isOn = true;
        }
    }
}
