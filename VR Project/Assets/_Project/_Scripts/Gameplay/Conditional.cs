using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Conditional : SerializedMonoBehaviour
{
    public bool isTrue;
    
    [Header("Events")]
    [FormerlySerializedAs("OnTrue")] public UnityEvent OnSuccess;
    [FormerlySerializedAs("OnFalse")] public UnityEvent OnFailed;

    public void Bind(ConditionManager manager)
    {
        OnSuccess.AddListener(manager.CheckCondition);   
        OnFailed.AddListener(manager.CheckCondition); 
    }

    public void Invoke(bool toTrue)
    {
        isTrue = toTrue;
        if (toTrue)
        {
            OnSuccess.Invoke();
        }
        else
        {
            OnFailed.Invoke();
        }
    }
}