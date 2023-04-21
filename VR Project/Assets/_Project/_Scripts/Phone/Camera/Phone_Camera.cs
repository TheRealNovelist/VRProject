using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Phone_Camera : PhonePage
{
    [SerializeField] private CustomUIElement cameraCanvas;
    [SerializeField] private float transitionDuration = 0.5f;
    [SerializeField] private Vector3 spacing;

    [Header("Image Settings")]
    [SerializeField] private PhoneImageAlbum album;

    [Header("Camera Settings")]
    [SerializeField] private Camera cameraOnPhone;
    [SerializeField] private TextMeshProUGUI displayText;
    [Space]
    [SerializeField] private LayerMask normalMask; 
    [SerializeField] private LayerMask xRayMask; 
    [SerializeField] private LayerMask ghostMask;

    [SerializeField] private CameraMode initialMode = CameraMode.Normal;
    private CameraMode _mode;
    
    private enum CameraMode
    {
        Normal = 0,
        XRay = 1,
        Ghost = 2
    }
    
    private void Start()
    {
        SwitchMode(initialMode);
    }

    void SwitchMode(CameraMode mode)
    {
        switch (mode)
        {
            case CameraMode.Normal:
                cameraOnPhone.cullingMask = normalMask;
                _mode = CameraMode.Normal;
                displayText.text = "Normal";
                break;
            case CameraMode.XRay:
                cameraOnPhone.cullingMask = xRayMask;
                _mode = CameraMode.XRay;
                displayText.text = "X-Ray";
                break;
            case CameraMode.Ghost:
                cameraOnPhone.cullingMask = ghostMask;
                _mode = CameraMode.Ghost;
                displayText.text = "Ghost";
                break;
        }
    }
    
    
    public override void StartPage(Phone phone)
    {
        cameraCanvas.ResetPosition();
        cameraCanvas.Move(new Vector3(0, -1, 0), spacing);
        cameraCanvas.MoveY(transitionDuration, 1, spacing.y);
    }

    public override void OnThumbStickDown()
    {
        album.CreateImage();
    }

    public override void ExitPage(Phone phone)
    {
        cameraCanvas.MoveY(transitionDuration, -1, spacing.y).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
