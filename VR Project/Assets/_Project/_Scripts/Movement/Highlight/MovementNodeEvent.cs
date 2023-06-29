using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MovementNodeEvent : MonoBehaviour, IMovementNodeResponse
{
    public UnityEvent OnSelected;
    public UnityEvent OnRejected;
    public UnityEvent OnDeselected;

    public UnityEvent OnActivated;
    public UnityEvent OnDeactivated;
    
    public void Selected(bool allow)
    {
        if (allow)
        {
            OnSelected.Invoke();
        }
        else
        {
            OnRejected.Invoke();
        }
    }

    public void Deselected()
    {
        OnDeselected.Invoke();
    }

    public void SetActive(bool active)
    {
        if (active)
            OnActivated.Invoke();
        else
            OnDeactivated.Invoke();
    }
}
