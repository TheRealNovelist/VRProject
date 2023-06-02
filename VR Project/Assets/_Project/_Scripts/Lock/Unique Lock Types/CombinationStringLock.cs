using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class CombinationStringLock : Lock
{
    [Header("Specific Settings")]
    [SerializeField] private string combination;
    
    [Header("Display Settings")]
    [SerializeField] private TMP_Text outputText;

    private int _combinationLength;
    private Stack<string> _inputtedValues = new();
    private void Awake()
    {
        _combinationLength = combination.Length;
        outputText.text = "";
    }

    public void Input(string value)
    {
        string fullString = GetAllInputted();
        
        if (fullString.Length >= _combinationLength)
        {
            return;
        }
        
        _inputtedValues.Push(value);
        outputText.text = GetAllInputted();
    }

    public void Confirm()
    {
        if (GetAllInputted() == combination)
        {
            Invoke(true); 
        }
        else
        {
            Invoke(false);
        }
    }

    public void ClearLatest()
    {
        if (_inputtedValues.Count <= 0)
        {
            return;
        }
        
        _inputtedValues.Pop();
        outputText.text = GetAllInputted();
    }

    public void ClearAll()
    {
        if (_inputtedValues.Count <= 0)
        {
            return;
        }
        
        _inputtedValues.Clear();
        outputText.text = "";
    }

    public string GetAllInputted()
    {
        if (_inputtedValues.Count <= 0)
            return "";

        var input = _inputtedValues.Reverse();

        string fullString = string.Join("", input);
        Debug.Log(fullString);

        return fullString;
    }
}
