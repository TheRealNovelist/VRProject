using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    [SerializeField] private PanelElement initialPanel;
    
    [Header("Settings")]
    [SerializeField] private bool expandOnStart = true;
    [SerializeField] private bool moveRight = false;
    [SerializeField] private bool panelStacking = true;
    [SerializeField] private float transitionDuration = 0.5f;
    
    [Range(0, 1f)]
    [SerializeField] private float fadeAmount = 0.7f;
    
    [Space]
    [SerializeField] private float xSpacing = 0.3f;
    [SerializeField] private float zSpacing = 0.05f;
    
    private List<PanelElement> activePanels;
    private void Start()
    {
        foreach (Transform childPanel in transform)
        {
            childPanel.gameObject.SetActive(false);
        }

        if (!initialPanel)
            initialPanel = transform.GetChild(0).GetComponent<PanelElement>();
        
        if (!expandOnStart) return;
        ToPanel(initialPanel);
    }

    public void TurnOnPanel()
    {
        ToPanel(initialPanel);
    }
    
    public void MinimizeAll()
    {
        foreach (PanelElement panel in activePanels)
        {
            panel.Minimize(transitionDuration).OnComplete(() => panel.ResetPanel());
        }
        
        activePanels.Clear();
    }
    
    public void ToPanel(PanelElement nextPanel)
    {
        for (int i = 0; i < activePanels.Count; i++)
        {
            if (i == activePanels.Count - 1)
            {
                activePanels[i].MoveX(transitionDuration, moveRight ? 1 : -1, xSpacing);
                activePanels[i].blocker.SetBlockerAlpha(0, fadeAmount, transitionDuration, true);
                continue;
            }

            if (panelStacking)
            {
                activePanels[i].MoveZ(transitionDuration, 1, zSpacing);
            }
            else
            {
                activePanels[i].MoveX(transitionDuration, moveRight ? 1 : -1, xSpacing);
            }
        }
        
        activePanels.Add(nextPanel);
        nextPanel.Expand(transitionDuration);
    }

    public void PreviousPanel()
    {
        //Avoid the only panel to be removed
        if (activePanels.Count == 1)
        {
            return;
        }
        
        PanelElement currentPanel = activePanels[^1];
        activePanels.Remove(currentPanel);
        
        currentPanel.Minimize(transitionDuration);
        
        for (int i = 0; i < activePanels.Count; i++)
        {
            if (i == activePanels.Count - 1)
            {
                activePanels[i].MoveX(transitionDuration, moveRight ? -1 : 1, xSpacing);
                activePanels[i].blocker.SetBlockerAlpha(fadeAmount, 0, transitionDuration);
                continue;
            }

            if (panelStacking)
            {
                activePanels[i].MoveZ(transitionDuration, -1, zSpacing);
            }
            else
            {
                activePanels[i].MoveX(transitionDuration, moveRight ? -1 : 1, xSpacing);
            }
        }
    }
}
