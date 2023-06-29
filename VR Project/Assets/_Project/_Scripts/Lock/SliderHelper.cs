using System.Collections;
using System.Collections.Generic;
using BNG;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Slider))]
public class SliderHelper : MonoBehaviour
{
    private Slider _slider => GetComponent<Slider>();

    [SerializeField] private int minValue;
    [SerializeField] private int maxValue;

    [SerializeField] private TMP_Text text;

    [SerializeField] private bool debug;

    public UnityEvent<float> OnPercentageChange;
    
    public void CalculateFloat(float value)
    {
        int calcValue = Mathf.RoundToInt(Mathf.Lerp(minValue, maxValue, value / 100));
        text.text = calcValue.ToString();
        
        OnPercentageChange.Invoke(value / 100);
    }
}
