using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

[SelectionBase]
public class Phone : MonoBehaviour
{
    public static event Action OnPhoneTaken;
    public static event Action OnPhoneReturn;
    public static bool IsPhoneInHolder { get; private set; } = true;
    public static bool IsPhoneOn { get; private set; }
    
    [SerializeField] private App startingApp;
    [SerializeField] private CanvasGroup lockScreen;
    [SerializeField] private float transitionDuration = 1f;
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float maxIdleTime = 30f;
    
    private Stack<App> activePages = new();

    private float _idleTime;

    private void Awake()
    {
        //Avoid all app are on
        foreach (var app in GetComponentsInChildren<App>())
        {
            app.gameObject.SetActive(false);
        }
        
        lockScreen.gameObject.SetActive(true);
        activePages.Push(startingApp);
    }

    private void Update()
    {
        if (!IsPhoneOn) return;
        
        if (_idleTime >= maxIdleTime)
        {
            _idleTime = maxIdleTime;
            TurnOff();
            return;
        }

        _idleTime += Time.deltaTime;
    }
    
    public void TakePhone()
    {
        IsPhoneInHolder = false;
        OnPhoneTaken?.Invoke();
        
        OnPhoneAction();
    }
    
    public void ReturnPhone()
    {
        IsPhoneInHolder = true;
        OnPhoneReturn?.Invoke();
        
        OnPhoneAction();
    }

    public void TurnOn()
    {
        IsPhoneOn = true;
        
        //Reset idle time
        _idleTime = 0;
        
        if (activePages.TryPeek(out App topPage))
        {
            topPage.Enter();
        }
        
        lockScreen.DOFade(0, fadeDuration).OnComplete(() => lockScreen.gameObject.SetActive(false));
    }

    public void TurnOff()
    {
        IsPhoneOn = false;
        
        lockScreen.gameObject.SetActive(true);
        lockScreen.DOFade(1, fadeDuration).OnComplete(() =>
        {
            if (activePages.TryPeek(out App topPage))
            {
                topPage.Exit();
            }
        });
    }
    
    public void EnterApp(App app)
    {
        app.Enter(transitionDuration);

        if (activePages.TryPeek(out App topPage))
        {
            topPage.Exit(transitionDuration);
        }
        
        activePages.Push(app);
        
        OnPhoneAction();
    }
    
    public void ExitApp()
    {
        if (activePages.Count > 1)
        {
            App topPage = activePages.Pop();
            topPage.Exit(transitionDuration);
            
            App nextTopPage = activePages.Peek();
            nextTopPage.Enter(transitionDuration);
        }
        
        OnPhoneAction();
    }

    public void ExitAppWithButton()
    {
        if (IsPhoneInHolder)
            ExitApp();
    }

    public void OnPhoneAction()
    {
        //Reset idleTime to 0 as action has been called
        _idleTime = 0;
    }
}
