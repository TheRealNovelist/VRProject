using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class Phone_MultiPage : MonoBehaviour, IPhoneApp
{
    [Header("Settings")]
    [SerializeField] private CustomUIElement initialPage;
    [SerializeField] private GameObject panelHolder;
    [SerializeField] private float transitionDuration = 0.5f;
    [SerializeField] private Vector3 spacing;
    [SerializeField] private List<CustomUIElement> allPages;

    private int _currentIndex;
    private bool _isRunning;
    
    private void Awake()
    {
        if (!initialPage)
            initialPage = panelHolder.transform.GetChild(0).GetComponent<CustomUIElement>();
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

    public void OnJoystickMove(float x, float y)
    {
        MovePage(Mathf.RoundToInt(x));
    }

    public void StartApp()
    {
        _currentIndex = 0;
        
        foreach (var page in allPages)
        {
            page.ResetPosition();
            page.Move(page != initialPage ? new Vector3(1, 0, 0) : new Vector3(0, -1, 0), spacing);
        }
        
        initialPage.MoveY(transitionDuration, 1, spacing.y);
    }

    public void OnThumbStickDown()
    {
        
    }

    public Tween ExitApp()
    {
        CustomUIElement currentPage = allPages[_currentIndex];
        return currentPage.MoveY(transitionDuration, -1, spacing.y).OnComplete(() => gameObject.SetActive(false));
    }
}
