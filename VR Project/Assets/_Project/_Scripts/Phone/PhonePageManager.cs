using System;
using System.Collections;
using System.Collections.Generic;
using BNG;
using DG.Tweening;
using echo17.EndlessBook;
using UnityEngine;

[SelectionBase]
public class PhonePageManager : MonoBehaviour
{
    [SerializeField] private App startingPage;
    
    private App _currentPage;

    private Stack<App> activePages = new();

    private void Awake()
    {
        if (!startingPage)
        {
            Debug.Log("Please add in a starting page");
            return;
        }
        
        PushPage(startingPage);
    }

    public void PushPage(App page)
    {
        Debug.Log("Pushed page " + page.name);
        page.Enter();

        if (activePages.Count > 0)
        {
            App topPage = activePages.Peek();
            
            topPage.Exit();
        }
        
        activePages.Push(page);
    }

    public void PopPage()
    {
        if (activePages.Count > 1)
        {
            App topPage = activePages.Pop();
            topPage.Exit();
        }
    }

    public void PopAllPage()
    {
        while (activePages.Count > 1)
        {
            PopPage();
        }
    }
}
