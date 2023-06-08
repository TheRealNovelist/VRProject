using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class MovementNodeHelper : MonoBehaviour
{
    [SerializeField] private Level level;
    
    public void SetLevel(Level newLevel)
    {
        var movementNodes = GetComponentsInChildren<MovementNode>();

        foreach (var node in movementNodes)
        {
            node.level = newLevel;
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (level == Level.None)
        {
            level = Level.Present;
        }
        
        SetLevel(level);
    }
#endif
}
