using System;
using System.Collections;
using System.Collections.Generic;
using BNG;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;



public class Wristlight : MonoBehaviour
{
    [SerializeField] private Light forwardLight;
    [SerializeField] private Light surroundLight;
    
    private LightMode _mode;

    [SerializeField] private Renderer forwardMesh;
    [SerializeField] private Renderer surroundMesh;

    private enum LightMode
    {
        Off = 0,
        Forward = 1,
        Surround = 2
    }
    
    private void Start()
    {
        SetLightMode(LightMode.Off);
    }

    private void Update()
    {
         bool isDown = InputBridge.Instance.GetControllerBindingValue(ControllerBinding.XButtonDown) || Input.GetKeyDown(KeyCode.Keypad0);

         if (isDown)
         {
             CycleMode();
         }
    }

    [Button]
    private void CycleMode()
    {
        if ((int)_mode == 2)
        {
            _mode = 0;
        }
        else
        {
            _mode += 1;
        }
        
        SetLightMode(_mode);
    }
    
    private void SetLightMode(LightMode mode)
    {
        switch (mode)
        {
            case LightMode.Off:
                forwardLight.enabled = false;
                surroundLight.enabled = false;
                forwardMesh.material.color = Color.black;
                surroundMesh.material.color = Color.black;
                break;
            case LightMode.Forward:
                forwardLight.enabled = true;
                surroundLight.enabled = false;
                forwardMesh.material.color = Color.white;
                surroundMesh.material.color = Color.black;
                break;
            case LightMode.Surround:
                forwardLight.enabled = false;
                surroundLight.enabled = true;
                forwardMesh.material.color = Color.black;
                surroundMesh.material.color = Color.white;
                break;
        }
    }
}
