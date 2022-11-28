using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CameraPage : PageManager
{
    [SerializeField] private LayerMask normalMask; 
    [SerializeField] private LayerMask xRayMask; 
    [SerializeField] private LayerMask ghostMask;

    [SerializeField] private Camera cameraOnPhone;

    private CameraMode _mode;
    
    private enum CameraMode
    {
        Normal,
        XRay,
        Ghost
    }
    
    void SwitchMode(CameraMode mode)
    {
        switch (mode)
        {
            case CameraMode.Normal:
                cameraOnPhone.cullingMask = normalMask;
                break;
            case CameraMode.XRay:
                cameraOnPhone.cullingMask = xRayMask;
                break;
            case CameraMode.Ghost:
                cameraOnPhone.cullingMask = ghostMask;
                break;
        }
    }
    
    public override void Navigate(float x, float y)
    {
        
    }

    public override void Confirm()
    {
        switch (_mode)
        {
            case CameraMode.Normal:
                SwitchMode(CameraMode.XRay);
                break;
            case CameraMode.XRay:
                SwitchMode(CameraMode.Ghost);
                break;
            case CameraMode.Ghost:
                SwitchMode(CameraMode.Normal);
                break;
        }
    }
}
