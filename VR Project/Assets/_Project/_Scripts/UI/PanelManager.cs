using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    [SerializeField] protected CustomUIElement initialPanel;
    [SerializeField] protected GameObject panelHolder;
    
    [SerializeField] protected float transitionDuration = 0.5f;
    
    [Range(0, 1f)]
    [SerializeField] protected float fadeAmount = 0.7f;
    
    [Space]
    [SerializeField] protected float xSpacing = 0.3f;
    [SerializeField] protected float zSpacing = 0.05f;
    
    [Header("Settings")]
    [SerializeField] private bool expandOnStart = true;
    [SerializeField] private bool moveRight = false;
    [SerializeField] private bool panelStacking = true;

    private readonly List<CustomUIElement> _activePanels = new List<CustomUIElement>();

    private void Awake()
    {
        foreach (Transform childPanel in panelHolder.transform)
        {
            childPanel.gameObject.SetActive(false);
        }

        if (!initialPanel)
            initialPanel = panelHolder.transform.GetChild(0).GetComponent<CustomUIElement>();
    }

    private void Start()
    {
        if (!expandOnStart) return;
        ToPanel(initialPanel);
    }

    public void TurnOnPanel()
    {
        ToPanel(initialPanel);
    }
    
    public void MinimizeAll()
    {
        foreach (CustomUIElement panel in _activePanels)
        {
            panel.Minimize(transitionDuration).OnComplete(() => panel.ResetPosition());
        }
        
        _activePanels.Clear();
    }
    
    public void ToPanel(CustomUIElement nextPanel)
    {
        for (int i = 0; i < _activePanels.Count; i++)
        {
            CustomUIElement panel = _activePanels[i];
            if (i == _activePanels.Count - 1)
            {
                panel.MoveX(transitionDuration, moveRight ? 1 : -1, xSpacing);
                panel.SetCanvasLayer(1, 0, transitionDuration);
                panel.blocker.SetBlockerAlpha(0, fadeAmount, transitionDuration, true);
                continue;
            }

            if (panelStacking)
            {
                panel.MoveZ(transitionDuration, 1, zSpacing);
            }
            else
            {
                panel.MoveX(transitionDuration, moveRight ? 1 : -1, xSpacing);
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
        
        CustomUIElement currentCustomUI = _activePanels[^1];
        _activePanels.Remove(currentCustomUI);
        
        currentCustomUI.Minimize(transitionDuration);
        
        for (int i = 0; i < _activePanels.Count; i++)
        {
            CustomUIElement panel = _activePanels[i];
            if (i == _activePanels.Count - 1)
            {
                panel.MoveX(transitionDuration, moveRight ? -1 : 1, xSpacing);
                panel.SetCanvasLayer(1, 0, transitionDuration);
                panel.blocker.SetBlockerAlpha(fadeAmount, 0, transitionDuration);
                continue;
            }

            if (panelStacking)
            {
                panel.MoveZ(transitionDuration, -1, zSpacing);
            }
            else
            {
                panel.MoveX(transitionDuration, moveRight ? -1 : 1, xSpacing);
            }
        }
    }
}
