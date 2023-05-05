using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class StringLock : Lock
{
    [SerializeField] private TextMeshProUGUI outputText;
    [SerializeField] private string combination;

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
            OnUnlocked.Invoke(); 
        }
        else
        {
            OnFailed.Invoke();
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
