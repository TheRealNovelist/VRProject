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
    [SerializeField] private TextMeshProUGUI outputText;
    [SerializeField] private TextMeshProUGUI currentText;
    [SerializeField] private int maxValue = 100;
    [SerializeField] private List<int> combination;
    
    private List<int> _inputtedValues = new List<int>();

    private int _currentIndex = 0;
    private int _currentValue = 0;
    private string _currentInputtedText;
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
            nextValue = maxValue - value;
        }

        if (nextValue == maxValue)
        {
            nextValue = 0;
        }
        
        Input(nextValue);
    } 
    
    public void Input(int value)
    {
        //Return if all value has been inputted
        if (_currentIndex == combination.Count)
            return;

        if (_currentDirection == RadialDirection.None)
        {
            _currentDirection = GetDirection(_inputtedValues[_currentIndex], value);
        }
        else if (_currentDirection != GetDirection(_currentValue, value))
        {
            _currentDirection = GetDirection(_currentValue, value);
            _currentIndex += 1;
        }

        _currentValue = value;
        _inputtedValues[_currentIndex] = value;
        UpdateText();
    }

    private void UpdateText()
    {
        outputText.text = string.Join(" ", _inputtedValues);
        currentText.text = _currentValue.ToString();
    }
    
    private RadialDirection GetDirection(int current, int next)
    {
        if (next == 0)
        {
            if (current == maxValue - 10)
                return RadialDirection.Clockwise;
            if (current == 10)
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
