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

    }

    public void ExpandAll()
    {

    }

    public void MinimizeAll()
    {
        
    }
    
    public void ToPanel(PanelElement nextPanel)
    {

    }

    public void ReturnPanel()
    {

    }
}
