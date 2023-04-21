using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class Phone_MultiPage : PhonePage
{
    [Header("Settings")]
    [SerializeField] protected CustomUIElement blankPage;
    [SerializeField] private float transitionDuration = 0.5f;
    [SerializeField] private Vector3 spacing;
    
    [SerializeField] private GameObject panelHolder;
    [SerializeField] protected List<CustomUIElement> allPages;

    [SerializeField] private GameObject pageToCreate;

    private int _currentIndex;
    private bool _isRunning;
    
    private void CreatePage()
    {
        GameObject newPage = Instantiate(pageToCreate, panelHolder.transform);
        CustomUIElement pageElement = newPage.GetComponent<CustomUIElement>();
        
        allPages.Add(pageElement);
    }

    private void DeletePage()
    {
        if (allPages.Count == 0)
            return;
        
        CustomUIElement currentPage = allPages[_currentIndex];
        CustomUIElement nextPage;

        int increment;

        //If current index is == 0
        if (_currentIndex == 0)
        {
            //If there are pages still exist, move to the next page on the right
            if (allPages.Count > 0)
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
            .Insert(0, currentPage.Expand(transitionDuration))
            .Insert(0, nextPage.MoveX(transitionDuration, -increment, spacing.x))
            .OnComplete(() => _isRunning = false);

        moveSequence.Play();

        _currentIndex -= 1;
    }
    
    private void MovePage(int increment)
    {
        if ((_currentIndex == 0 && increment < 0) || (_currentIndex == allPages.Count - 1 && increment > 0))
            return;

        if (_isRunning)
            return;
        
        CustomUIElement currentPage = allPages[_currentIndex];
        CustomUIElement nextPage = allPages[_currentIndex + increment];

        _isRunning = true;
        
        Sequence moveSequence = DOTween.Sequence();

        moveSequence
            .Insert(0, currentPage.MoveX(transitionDuration, -increment, spacing.x))
            .Insert(0, nextPage.MoveX(transitionDuration, -increment, spacing.x))
            .OnComplete(() => _isRunning = false);

        moveSequence.Play();
        
        _currentIndex += increment;
    }

    public override void OnJoystickMove(float x, float y)
    {
        MovePage(Mathf.RoundToInt(x));
    }

    public override void StartPage(Phone phone)
    {
        _currentIndex = 0;

        if (allPages.Count >= 0)
        {
            blankPage.MoveFromOrigin(new Vector3(-1, 0, 0), spacing);
            
            CustomUIElement firstPage = allPages[_currentIndex];
        
            foreach (var page in allPages)
            {
                page.MoveFromOrigin(page != firstPage ? new Vector3(1, 0, 0) : new Vector3(0, -1, 0), spacing);
            }
        
            firstPage.MoveY(transitionDuration, 1, spacing.y);
        }
        else
        {
            blankPage.MoveFromOrigin(new Vector3(0, -1, 0), spacing);
            blankPage.MoveY(transitionDuration, 1, spacing.y);
        }
    }
    
    public override void ExitPage(Phone phone)
    {
        CustomUIElement currentPage = allPages[_currentIndex];
        currentPage.MoveY(transitionDuration, -1, spacing.y).OnComplete(() => gameObject.SetActive(false));
    }
}
