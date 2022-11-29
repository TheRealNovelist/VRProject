using System;
using System.Collections;
using System.Collections.Generic;
using BNG;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Wristlight : MonoBehaviour
{
    [SerializeField] private Light light;


    private bool isOn;

    private void Start()
    {
        isOn = true;
        light.enabled = isOn;
    }

    private void Update()
    {
         bool isDown = InputBridge.Instance.GetControllerBindingValue(ControllerBinding.XButtonDown);

         if (isDown)
         {
             isOn = !isOn;
             light.enabled = isOn;
         }
    }
}
