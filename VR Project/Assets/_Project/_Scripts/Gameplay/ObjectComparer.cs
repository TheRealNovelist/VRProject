using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectComparer : Conditional
{
    [Header("Object Settings")]
    [SerializeField] private List<string> namesAllowed;
    
    private void OnTriggerEnter(Collider other)
    {
        if (isTrue) return;
        if (!namesAllowed.Contains(other.gameObject.name)) return;
        
        Invoke(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isTrue) return;
        if (!namesAllowed.Contains(other.gameObject.name)) return;
        
        Invoke(false);
    }
}
