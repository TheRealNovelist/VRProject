using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class OutlineHighlight : MonoBehaviour, IMovementNodeResponse
{
    [SerializeField] private Outline outline;

    [System.Serializable]
    public class OutlineSetting
    {
        public Outline.Mode outlineMode;
        public Color outlineColor = Color.white;
        [Range(0, 10f)]public float outlineWidth = 2f;
    }

    [SerializeField] private bool useOutlineDefaultSetting = true;
    [HideIf("useOutlineDefaultSetting"), SerializeField] private OutlineSetting defaultSetting;
    [SerializeField] private OutlineSetting selectedSetting;
    [SerializeField] private OutlineSetting rejectedSetting;

    private void Awake()
    {
        if (!outline)
        {
            Debug.LogWarning("[OutlineHighlight] No outline found to highlight");
            return;
        }
        
        if (useOutlineDefaultSetting)
        {
            defaultSetting = new OutlineSetting
            {
                outlineMode = outline.OutlineMode,
                outlineColor =  outline.OutlineColor,
                outlineWidth = outline.OutlineWidth
            };
        }
        else
        {
            SetOutline(defaultSetting);
        }
    }

    public void Selected(bool allow)
    {
        SetOutline(allow ? selectedSetting : rejectedSetting);
    }

    public void Deselected()
    {
        SetOutline(defaultSetting);
    }

    public void SetActive(bool active)
    {
        outline.enabled = active;
    }

    private void SetOutline(OutlineSetting setting)
    {
        if (!outline) return;
        
        outline.OutlineMode = setting.outlineMode;
        outline.OutlineColor = setting.outlineColor;
        outline.OutlineWidth = setting.outlineWidth;
    }
}
