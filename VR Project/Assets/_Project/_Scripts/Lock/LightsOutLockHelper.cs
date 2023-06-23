using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsOutLockHelper : MonoBehaviour
{
    [SerializeField] private Vector2Int position;
    
    [SerializeField] private LightsOutLock bindLock;
    [SerializeField] private LightsOutDisplay bindGraphic;

    public void ToggleLight()
    {
        bindLock.ToggleLight(position);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (bindGraphic)
        {
            bindGraphic.gameObject.name = $"{position}";
            bindGraphic.bindLock = bindLock;
            bindGraphic.position = position;
        }
    }
#endif
}
