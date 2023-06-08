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

    [Header("Warning")] 
    [SerializeField] private UIAnimation warning;
    [SerializeField] private float warningTime;
    
    [Header("Fade Settings")]
    [SerializeField] private Graphic screen;
    [SerializeField] private float fadeDuration = 0.5f;

    [Header("Camera Settings")] 
    [SerializeField] private CameraSwitcher switcher;
    
    private Level _mode;

    private bool IsCameraOn() => !Phone.IsPhoneInHolder && gameObject.activeInHierarchy;

    protected override void Awake()
    {
        base.Awake();
        
        cameraOnPhone.gameObject.SetActive(gameObject.activeInHierarchy);
        SwitchMode(Level.Present);
    }

    private void Start()
    {
        warning.MoveFromOrigin(new Vector3(0, 1, 0));
    }

    private void OnPhoneTaken()
    {
        SetBlockerActive(false);
    }
    
    private void OnPhoneReturn()
    {
        SetBlockerActive(true);
    }

    [Button]
    void SwitchMode(Level mode)
    {
        switch (mode)
        {
            case Level.Present:
                switcher.SwitchCamera(false);
                _mode = Level.Present;
                break;
            case Level.Future:
                switcher.SwitchCamera(true);
                _mode = Level.Future;
                break;
        }
    }

    
    public void CycleMode()
    {
        if (!IsCameraOn()) return;
        
        switch (_mode)
        {
            case Level.Present:
                SwitchMode(Level.Future);
                break;
            case Level.Future:
                SwitchMode(Level.Present);
                break;
        }
        
        OnPhoneAction();
    }

    public void TakePicture()
    {
        if (!IsCameraOn()) return;

        if (!album.CreatePhoto())
        {
            Sequence sequence = DOTween.Sequence();

            warning.MoveFromOrigin(new Vector3(0, 1, 0));
            
            canvasGrp.interactable = false;
            sequence.Append(warning.MoveY(fadeDuration, -1));
            sequence.AppendInterval(warningTime);
            sequence.Append(warning.MoveY(fadeDuration, 1));
            sequence.OnComplete(() => canvasGrp.interactable = true);
            
            return;
        }
        
        if (screen)
        {
            Sequence sequence = DOTween.Sequence();

            canvasGrp.interactable = false;
            sequence.Append(screen.DOFade(0, fadeDuration / 2));
            sequence.Append(screen.DOFade(1, fadeDuration / 2));
            sequence.OnComplete(() => canvasGrp.interactable = true);
            
            sequence.Play();
        }
        
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
