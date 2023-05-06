using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public enum RadialDirection
{
    None,
    Clockwise,
    AntiClockwise
}

public class RadialLock : Lock
{
    [Header("Specific Settings")]
    [SerializeField] private int maxValue = 360;
    [SerializeField] private List<int> combination;
    
    [Header("Display Settings")]
    [SerializeField] private TextMeshProUGUI outputText;
    [SerializeField] private TextMeshProUGUI currentText;
    
    private List<int> _inputtedValues = new List<int>();

    private int _currentIndex = 0;
    private int _currentValue = 0;
    
    private RadialDirection _currentDirection;

    private void Start()
    {
        for (int i = 0; i < combination.Count; i++)
        {
            _inputtedValues.Add(0);
        }
        UpdateText();
    }

    public void Increment(int value)
    {
        int nextValue = _currentValue + value;

        if (nextValue < 0)
        {
            nextValue = maxValue - Mathf.Abs(value);
        }

        if (nextValue == maxValue)
        {
            nextValue = 0;
        }

        Input(nextValue);
    } 
    
    public void Input(float value)
    {
        int intVal = Mathf.RoundToInt(value);
        
        if (_currentDirection == RadialDirection.None)
        {
            _currentDirection = GetDirection(_inputtedValues[_currentIndex], intVal);
        }
        else if (_currentDirection != GetDirection(_currentValue, intVal))
        {
            _currentIndex += 1;
            
            if (_currentIndex < 3)
            {
                _currentDirection = GetDirection(_currentValue, intVal);
            }
            else
            {
                ResetLock();
            }
        }

        _currentValue = intVal;

        if (_currentIndex < combination.Count)
        {
            _inputtedValues[_currentIndex] = intVal;
        }
        
        UpdateText();
    }

    private void UpdateText()
    {
        if (outputText)
            outputText.text = string.Join(" ", _inputtedValues);
        
        if (currentText)
            currentText.text = _currentValue.ToString();
    }
    
    private RadialDirection GetDirection(int current, int next)
    {
        if (next == 0)
        {
            if (current == maxValue - 10)
                return RadialDirection.Clockwise;
        }

        if (current == 0)
        {
            if (next == maxValue - 10)
                return RadialDirection.AntiClockwise;
        }
        
        return (next - current) switch
        {
            > 0 => RadialDirection.Clockwise,
            < 0 => RadialDirection.AntiClockwise,
            _ => RadialDirection.None
        };
    }
    
    public void Confirm()
    {
        if (_currentIndex != combination.Count - 1)
        {
            OnFailed.Invoke();
            return;
        }

        if (combination.Where((t, i) => _inputtedValues[i] != t).Any())
        {
            OnFailed.Invoke();
            return;
        }
        
        OnUnlocked.Invoke();
    }

    public void ResetLock()
    {
        for (int i = 0; i < combination.Count; i++)
        {
            _inputtedValues[i] = 0;
        }
        
        _inputtedValues[0] = _currentValue;
        _currentDirection = RadialDirection.None;
        _currentIndex = 0;
        UpdateText();
    }
    
}
