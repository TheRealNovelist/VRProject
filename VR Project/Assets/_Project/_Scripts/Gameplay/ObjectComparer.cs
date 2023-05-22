using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectComparer : MonoBehaviour
{
    [SerializeField] private List<string> namesAllowed;

    public bool isTrue;
    
    public UnityEvent OnTrue;
    public UnityEvent OnFalse;

    public void Bind(ObjectComparerManager manager)
    {
        OnTrue.AddListener(manager.CheckCondition);   
        OnFalse.AddListener(manager.CheckCondition); 
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (isTrue) return;
        if (!namesAllowed.Contains(other.gameObject.name)) return;
        
        isTrue = true;
        OnTrue.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isTrue) return;
        if (!namesAllowed.Contains(other.gameObject.name)) return;
        
        isTrue = false;
        OnFalse.Invoke();
    }
}
