using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MultiPage : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] protected UIAnimation blankPage;
    [SerializeField] private float transitionDuration = 0.5f;
    [SerializeField] private Vector3 spacing;

    [Header("Page Settings")] 
    [SerializeField] private UIAnimation pageAndButtonHolder;
    [SerializeField] private GameObject pageHolder;
    [SerializeField] private GameObject pageToCreate;
    [Space]
    protected List<UIAnimation> allPages = new();

    private int _currentIndex;
    private bool _isRunning;

    [Header("Buttons")] 
    [SerializeField] private GameObject leftButton;
    [SerializeField] private GameObject rightButton;

    protected UIAnimation CreatePage()
    {
        GameObject newPage = Instantiate(pageToCreate, pageHolder.transform);
        UIAnimation pageElement = newPage.GetComponent<UIAnimation>();
        
        allPages.Add(pageElement);
        return pageElement;
    }

    protected void DeleteAllPage()
    {
        foreach (var page in allPages)
        {
            Destroy(page.gameObject);
        }

        allPages = new List<UIAnimation>();
    }
    
    protected bool DeletePage(Action<UIAnimation> callback = null)
    {
        if (allPages.Count == 0)
            return false;

        if (_isRunning)
            return false;
        
        UIAnimation currentPage = allPages[_currentIndex];
        UIAnimation nextPage;

        int increment;

        //If current index is == 0
        if (_currentIndex == 0)
        {
            //If there are pages still exist, move to the next page on the right
            if (allPages.Count > 1)
            {
                nextPage = allPages[_currentIndex + 1];
                increment = 1;
            }
            //Else, display blank page
            else
            {
                nextPage = blankPage;
                increment = -1;
            }
        }
        else
        {
            nextPage = allPages[_currentIndex - 1];
            increment = -1;
        }
        
        Sequence moveSequence = DOTween.Sequence();

        moveSequence
            .Insert(0, currentPage.Minimize(transitionDuration))
            .Insert(0, nextPage.MoveX(transitionDuration, -increment, spacing.x))
            .OnComplete(() =>
            {
                allPages.Remove(currentPage);
                if (nextPage != blankPage)
                {
                    _currentIndex = allPages.IndexOf(nextPage);
    
                }
                else
                {
                    _currentIndex = 0;
                    pageAndButtonHolder.gameObject.SetActive(false);    
                }
                
                callback?.Invoke(currentPage);
                Destroy(currentPage.gameObject);
                
                _isRunning = false;
            });

        moveSequence.Play();
        
        return true;
    }
    
    public void MovePage(int increment)
    {
        if (allPages.Count <= 1)
            return;
        
        if ((_currentIndex == 0 && increment < 0) || (_currentIndex == allPages.Count - 1 && increment > 0))
            return;

        if (_isRunning)
            return;
        
        UIAnimation currentPage = allPages[_currentIndex];
        UIAnimation nextPage = allPages[_currentIndex + increment];

        _isRunning = true;
        
        Sequence moveSequence = DOTween.Sequence();

        moveSequence
            .Insert(0, currentPage.MoveX(transitionDuration, -increment, spacing.x))
            .Insert(0, nextPage.MoveX(transitionDuration, -increment, spacing.x))
            .OnComplete(() =>
            {
                _currentIndex += increment;
                leftButton.SetActive(_currentIndex != 0);
                rightButton.SetActive(_currentIndex != allPages.Count - 1);
                _isRunning = false;
            });

        moveSequence.Play();
    }

    public void Enter()
    {
        _currentIndex = 0;

        gameObject.SetActive(true);
        pageAndButtonHolder.gameObject.SetActive(allPages.Count > 0);
        Debug.Log("Started Page");
        
        if (allPages.Count > 0)
        {
            leftButton.SetActive(_currentIndex != 0);
            rightButton.SetActive(_currentIndex != allPages.Count - 1);
            
            blankPage.MoveFromOrigin(new Vector3(-1, 0, 0), spacing);
            pageAndButtonHolder.MoveFromOrigin(new Vector3(0, -1, 0), spacing);
            
            UIAnimation firstPage = allPages[_currentIndex];
        
            foreach (var page in allPages)
            {
                page.MoveFromOrigin(page != firstPage ? new Vector3(1, 0, 0) : new Vector3(0, 0, 0), spacing);
            }
        
            pageAndButtonHolder.MoveY(transitionDuration, 1, spacing.y);
        }
        else
        {
            blankPage.MoveFromOrigin(new Vector3(0, -1, 0), spacing);
            blankPage.MoveY(transitionDuration, 1, spacing.y);
        }
    }
    
    public void Exit()
    {
        UIAnimation currentPage = allPages.Count > 0 ? pageAndButtonHolder : blankPage;
        currentPage.MoveY(transitionDuration, -1, spacing.y).OnComplete(() => gameObject.SetActive(false));
    }
}
