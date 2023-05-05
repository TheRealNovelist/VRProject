using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Lock : MonoBehaviour
{
    [SerializeField] protected UnityEvent OnFailed;
    [SerializeField] protected UnityEvent OnUnlocked;
}
