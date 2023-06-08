using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Level
{
    None,
    Present, 
    Future
}

public class LevelController : MonoBehaviour
{
    public static Vector3 Offset {get; private set;}
    public static Level CurrentLevel { get; private set; } = Level.None;

    [SerializeField] private Transform presentRoot;
    [SerializeField] private Transform futureRoot;

    public static event Action<Level> OnLevelChanged;

    private void OnEnable()
    {
        OnLevelChanged += SetOffset;
    }

    private void OnDisable()
    {
        OnLevelChanged -= SetOffset;
    }

    public void SetOffset(Level level)
    {
        Offset = level switch
        {
            Level.Present => futureRoot.position - presentRoot.position,
            Level.Future => presentRoot.position - futureRoot.position,
            _ => Offset
        };
    }

    public static void SwitchLevel(Level level)
    {
        Debug.Log($"[LevelController] Switching to {level} level");
        
        switch (level)
        {
            case Level.Present:
                CurrentLevel = Level.Present;
                break;
            case Level.Future:
                CurrentLevel = Level.Future;
                break;
            default:
                Debug.Log("[LevelController] Invalid level to switch");
                break;
        }
        
        OnLevelChanged?.Invoke(level);
    }
}
