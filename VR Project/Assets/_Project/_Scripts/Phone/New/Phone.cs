using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Phone : MonoBehaviour
{
    public static event Action OnPhoneTaken;
    public static event Action OnPhoneReturn;
    public static bool IsPhoneInHolder { get; private set; } = true;
    
    [SerializeField] private App startingApp;
    [SerializeField] private PhoneLockScreen lockScreen;
    [SerializeField] private float transitionDuration = 1f;

    private Stack<App> activePages = new();
    
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

    [Button]
    public void TakePhone()
    {
        IsPhoneInHolder = false;
        OnPhoneTaken?.Invoke();
    }

    [Button]
    public void ReturnPhone()
    {
        IsPhoneInHolder = true;
        OnPhoneReturn?.Invoke();
    }

    public void TurnOn()
    {
        if (activePages.TryPeek(out App topPage))
        {
            topPage.Enter();
        }
    }

    public void TurnOff()
    {
        if (activePages.TryPeek(out App topPage))
        {
            topPage.Exit();
        }
    }
    
    public void EnterApp(App page)
    {
        page.Enter(transitionDuration);

        if (activePages.TryPeek(out App topPage))
        {
            topPage.Exit(transitionDuration);
        }
        
        activePages.Push(page);
    }

    [Button]
    public void ExitApp()
    {
        if (activePages.Count > 1)
        {
            App topPage = activePages.Pop();
            topPage.Exit(transitionDuration);
            
            App nextTopPage = activePages.Peek();
            nextTopPage.Enter(transitionDuration);
        }
    }
    
}
