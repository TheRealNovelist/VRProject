using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class AudioSlider : AudioCustomSettings
{
    private Slider slider;
    [SerializeField] private string type = "MasterVolume";
    
    // Start is called before the first frame update
    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.maxValue = 1;
        slider.minValue = 0.0001f;
    }

    private void Start()
    {
        slider.value = GetVolume(type);
    }

    public void SetVolume(float value)
    {
        SetVolume(type, value);
    }
}
