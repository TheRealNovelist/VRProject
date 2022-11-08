using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    [SerializeField] private PanelElement currentPanel;

    [Header("Settings")]
    [SerializeField] private bool expandOnStart = true;
    
    [SerializeField] private float duration = 1f;

    [SerializeField] private float spacing = 0.3f;


    private void Start()
    {
        foreach (Transform childPanel in transform)
        {
            childPanel.gameObject.SetActive(false);
        }
        
        if (!expandOnStart) return;
        
        currentPanel.Expand(duration);
    }

    public void ToPanel(PanelElement nextPanel)
    {
        currentPanel.MoveLeft(duration, spacing, true);
        
        //Setting the parent panel as the current panel first
        nextPanel.parentPanel = currentPanel;
        currentPanel = nextPanel;
        
        currentPanel.Expand(duration);
    }

    public void ReturnPanel()
    {
        if (!currentPanel.parentPanel)
            return;
        
        currentPanel.Minimize(duration);
        
        //Store returning panel from parent panel
        PanelElement returningPanel = currentPanel.parentPanel;
        
        //Set parent panel as null before switching
        currentPanel.parentPanel = null;
        currentPanel = returningPanel;
        
        currentPanel.MoveRight(duration, spacing, false, true);
    }
}
