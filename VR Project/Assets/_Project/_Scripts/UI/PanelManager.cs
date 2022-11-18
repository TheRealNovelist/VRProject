using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    [SerializeField] private PanelElement initialPanel;
    [SerializeField] private GameObject panelHolder;
    
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
    
    private readonly List<PanelElement> _activePanels = new List<PanelElement>();
    private void Start()
    {
        foreach (Transform childPanel in panelHolder.transform)
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
        foreach (PanelElement panel in _activePanels)
        {
            panel.Minimize(transitionDuration).OnComplete(() => panel.ResetPanel());
        }
        
        _activePanels.Clear();
    }
    
    public void ToPanel(PanelElement nextPanel)
    {
        for (int i = 0; i < _activePanels.Count; i++)
        {
            if (i == _activePanels.Count - 1)
            {
                _activePanels[i].MoveX(transitionDuration, moveRight ? 1 : -1, xSpacing);
                _activePanels[i].blocker.SetBlockerAlpha(0, fadeAmount, transitionDuration, true);
                continue;
            }

            if (panelStacking)
            {
                _activePanels[i].MoveZ(transitionDuration, 1, zSpacing);
            }
            else
            {
                _activePanels[i].MoveX(transitionDuration, moveRight ? 1 : -1, xSpacing);
            }
        }
        
        _activePanels.Add(nextPanel);
        nextPanel.Expand(transitionDuration);
    }

    public void PreviousPanel()
    {
        //Avoid the only panel to be removed
        if (_activePanels.Count == 1)
        {
            return;
        }
        
        PanelElement currentPanel = _activePanels[^1];
        _activePanels.Remove(currentPanel);
        
        currentPanel.Minimize(transitionDuration);
        
        for (int i = 0; i < _activePanels.Count; i++)
        {
            if (i == _activePanels.Count - 1)
            {
                _activePanels[i].MoveX(transitionDuration, moveRight ? -1 : 1, xSpacing);
                _activePanels[i].blocker.SetBlockerAlpha(fadeAmount, 0, transitionDuration);
                continue;
            }

            if (panelStacking)
            {
                _activePanels[i].MoveZ(transitionDuration, -1, zSpacing);
            }
            else
            {
                _activePanels[i].MoveX(transitionDuration, moveRight ? -1 : 1, xSpacing);
            }
        }
    }
}
