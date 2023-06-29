using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorCodeLock : Lock
{
    [SerializeField] private bool isLocked;
    [SerializeField] private GameObject lockedLabel;
    
    [SerializeField] private Color colorToMatch = Color.red;
    [SerializeField] private Graphic graphicToDisplay;

    public bool IsLocked
    {
        get => isLocked;
        set
        {
            isLocked = value;
            lockedLabel.SetActive(value);
        } 
    }

    private Color _currentColor;

    private void Awake()
    {
        if (lockedLabel)
            lockedLabel.SetActive(isLocked);
    }

    void Start()
    {
        _currentColor.a = 1f;
    }

    private void Update()
    {
        if (IsLocked) return;
        
        graphicToDisplay.color = _currentColor;

        if (colorToMatch == _currentColor)
        {
            Invoke(true);
        }
        else
        {
            Invoke(false);
        }
    }

    public void UpdateR(float value)
    {
        _currentColor.r = value;
    }

    public void UpdateG(float value)
    {
        _currentColor.g = value;
    }

    public void UpdateB(float value)
    {
        _currentColor.b = value;
    }
}
