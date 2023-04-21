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
    [SerializeField] private PhonePage startingPage;
    
    private PhonePage _currentPage;

    private Stack<PhonePage> activePages = new();

    private void Awake()
    {
        if (!startingPage)
        {
            Debug.Log("Please add in a starting page");
            return;
        }
        
        PushPage(startingPage);
    }

    public void PushPage(PhonePage page)
    {
        Debug.Log("Pushed page " + page.name);
        page.StartPage();

        if (activePages.Count > 0)
        {
            PhonePage topPage = activePages.Peek();
            
            topPage.ExitPage();
        }
        
        activePages.Push(page);
    }

    public void PopPage()
    {
        if (activePages.Count > 1)
        {
            PhonePage topPage = activePages.Pop();
            topPage.ExitPage();
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
