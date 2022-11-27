using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class PageManager : MonoBehaviour, IPhoneNavigation
{
    [SerializeField] protected CustomUIElement initialPage;
    [SerializeField] protected GameObject panelHolder;
    
    [SerializeField] protected float transitionDuration = 0.5f;

    [SerializeField] protected Vector3 spacing;

    [SerializeField] private List<CustomUIElement> allPages;

    private int currentIndex;

    private bool isRunning;
    
    private void Awake()
    {
        if (!initialPage)
            initialPage = panelHolder.transform.GetChild(0).GetComponent<CustomUIElement>();

        initialPage.gameObject.SetActive(true);
        
        
    }

    public void MovePage(int increment)
    {
        if ((currentIndex == 0 && increment < 0) || (currentIndex == allPages.Count - 1 && increment > 0))
            return;

        if (isRunning)
            return;
        
        CustomUIElement currentPage = allPages[currentIndex];
        CustomUIElement nextPage = allPages[currentIndex + increment];

        isRunning = true;
        
        Sequence moveSequence = DOTween.Sequence();

        moveSequence
            .Insert(0, currentPage.MoveX(transitionDuration, increment, spacing.x))
            .Insert(0, nextPage.MoveX(transitionDuration, increment, spacing.x))
            .OnComplete(() => isRunning = false);

        moveSequence.Play();
        
        currentIndex += increment;
    }

    public void Navigate(float x, float y)
    {
        MovePage(Mathf.RoundToInt(x));
    }

    public void StartNavigation()
    {
        currentIndex = 0;
        
        foreach (var page in allPages)
        {
            page.ResetPosition();
            page.Move(page != initialPage ? new Vector3(-1, 0, 0) : new Vector3(0, -1, 0), spacing);
        }
        
        initialPage.MoveY(transitionDuration, 1, spacing.y);
    }

    public void Confirm()
    {
        
    }

    public Tween EndNavigation()
    {
        CustomUIElement currentPage = allPages[currentIndex];
        return currentPage.MoveY(transitionDuration, -1, spacing.y).OnComplete(() => gameObject.SetActive(false));
    }
}
