using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class PhoneCamera : App
{
    [Header("Components")]
    [SerializeField] private Camera cameraOnPhone;
    [SerializeField] private Album album;
    [SerializeField] private GameObject blocker;
    
    [Header("Fade Settings")]
    [SerializeField] private Graphic screen;
    [SerializeField] private float fadeDuration = 0.5f;

    [Header("Layers")]
    [SerializeField] private LayerMask normalMask; 
    [SerializeField] private LayerMask xRayMask; 
    [SerializeField] private LayerMask ghostMask;
    
    private CameraMode _mode;

    private bool IsCameraOn() => !Phone.IsPhoneInHolder && gameObject.activeInHierarchy;

    private enum CameraMode
    {
        Normal = 0,
        XRay = 1,
        Ghost = 2
    }

    protected override void Awake()
    {
        base.Awake();
        
        cameraOnPhone.gameObject.SetActive(gameObject.activeInHierarchy);
    }
    
    private void OnPhoneTaken()
    {
        SetBlockerActive(false);
    }
    
    private void OnPhoneReturn()
    {
        SetBlockerActive(true);
    }

    void SwitchMode(CameraMode mode)
    {
        switch (mode)
        {
            case CameraMode.Normal:
                cameraOnPhone.cullingMask = normalMask;
                _mode = CameraMode.Normal;
                break;
            case CameraMode.XRay:
                cameraOnPhone.cullingMask = xRayMask;
                _mode = CameraMode.XRay;
                break;
            case CameraMode.Ghost:
                cameraOnPhone.cullingMask = ghostMask;
                _mode = CameraMode.Ghost;
                break;
        }
    }

    public void CycleMode()
    {
        if (!IsCameraOn()) return;
        
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
        
        OnPhoneAction();
    }

    public void TakePicture()
    {
        if (!IsCameraOn()) return;

        if (screen)
        {
            Sequence sequence = DOTween.Sequence();

            canvasGrp.interactable = false;
            sequence.Append(screen.DOFade(0, fadeDuration / 2));
            sequence.Append(screen.DOFade(1, fadeDuration / 2));
            sequence.OnComplete(() => canvasGrp.interactable = true);
        }
        
        album.CreatePhoto();
        OnPhoneAction();
    }
    
    public void SetBlockerActive(bool isActive)
    {
        blocker.SetActive(isActive);
    }
    
    private void OnEnable()
    {
        Phone.OnPhoneTaken += OnPhoneTaken;
        Phone.OnPhoneReturn += OnPhoneReturn;
        
        cameraOnPhone.gameObject.SetActive(true);
        SetBlockerActive(Phone.IsPhoneInHolder);
    }

    private void OnDisable()
    {
        Phone.OnPhoneTaken -= OnPhoneTaken;
        Phone.OnPhoneReturn -= OnPhoneReturn;
        
        cameraOnPhone.gameObject.SetActive(false);
    }
    
    public override void Enter(float transitionDuration = 0)
    {
        gameObject.SetActive(true);
        if (transitionDuration != 0)
        {
            //Set initial position
            anim.MoveFromOrigin(new Vector3(0, 1, 0));
            //Slide Down
            anim.MoveY(transitionDuration, -1).OnComplete(() => canvasGrp.interactable = true);
        }
        else
        {
            canvasGrp.interactable = true;
        }
    }
    
    public override void Exit(float transitionDuration = 0)
    {
        canvasGrp.interactable = false;
        if (transitionDuration > 0)
        {
            //Slide Up
            anim.MoveY(transitionDuration, 1).OnComplete(() => gameObject.SetActive(false));
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
