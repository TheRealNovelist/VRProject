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
        currentPanel.Expand(duration, true);
    }

    public void ExpandAll()
    {
        currentPanel.Expand(duration, true);
    }

    public void MinimizeAll()
    {
        currentPanel.Minimize(duration, true);
    }
    
    public void ToPanel(PanelElement nextPanel)
    {
        currentPanel.MoveLeft(duration, spacing);
        
        //Setting the parent panel as the current panel first
        nextPanel.parentPanel = currentPanel;
        currentPanel = nextPanel;
        
        currentPanel.Expand(duration, false);
    }

    public void ReturnPanel()
    {
        if (!currentPanel.parentPanel)
            return;
        
        currentPanel.Minimize(duration, false);
        
        //Store returning panel from parent panel
        PanelElement returningPanel = currentPanel.parentPanel;
        
        //Set parent panel as null before switching
        currentPanel.parentPanel = null;
        currentPanel = returningPanel;
        
        currentPanel.MoveRight(duration, spacing);
    }
}
