using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class PanelElement : MonoBehaviour
{
    [SerializeField] private PanelBlocker blocker;

    [HideInInspector] public PanelElement parentPanel;

    [Range(0f, 1f)][SerializeField] private float fadeAmount = 0.75f;
    private RectTransform _rectTransform;
    private Vector3 _activeScale;

    private float _moveFactor;

    private bool _inProgress;

    private void SetCanvasLayer(int order)
    {
        GetComponent<Canvas>().sortingOrder = order;
    }
    
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _activeScale = _rectTransform.localScale;
        _moveFactor = _rectTransform.rect.width * _activeScale.x;
    }

    public void Expand(float duration)
    {
        if (_inProgress)
            return;
        
        blocker.gameObject.SetActive(true);
        blocker.Fade(0f, 0f, duration).OnComplete(() => blocker.gameObject.SetActive(false));
        
        gameObject.SetActive(true);
        _rectTransform.localScale = Vector3.zero;
        _rectTransform.DOScale(_activeScale, duration)
            .OnStart(() => _inProgress = true)
            .OnComplete(() =>
            {
                _inProgress = false;
                
            });
    }

    public void Minimize(float duration)
    {
        if (_inProgress)
            return;
        
        blocker.gameObject.SetActive(true);
        blocker.Fade(0f, 0f, duration);
        
        _rectTransform.localScale = _activeScale;
        _rectTransform.DOScale(Vector3.zero, duration)
            .OnStart(() => _inProgress = true)
            .OnComplete(() =>
            {
                _inProgress = false;
                gameObject.SetActive(false);
            });
    }
    
    public void MoveLeft(float duration, float spacing)
    {
        if (_inProgress)
            return;

        if (parentPanel)
            parentPanel.MoveBack(duration);
        
        blocker.gameObject.SetActive(true);
        blocker.Fade(0f, fadeAmount, duration);
        
        SetCanvasLayer(1);
        _rectTransform.DOMoveX(_rectTransform.anchoredPosition.x - _moveFactor - spacing, duration)
            .OnStart(() => _inProgress = true)
            .OnComplete(() =>
            {
                _inProgress = false;
                SetCanvasLayer(0);
            });
    }

    public void MoveRight(float duration, float spacing)
    {
        if (_inProgress)
            return;

        if (parentPanel)
            parentPanel.MoveForward(duration);
        
        blocker.Fade(fadeAmount, 0f, duration).OnComplete(() => blocker.gameObject.SetActive(false));
        
        SetCanvasLayer(1);
        _rectTransform.DOMoveX(_rectTransform.anchoredPosition.x + _moveFactor + spacing, duration)
            .OnStart(() => _inProgress = true)
            .OnComplete(() =>
            {
                _inProgress = false;
                SetCanvasLayer(0);
            });
    }

    private void MoveBack(float duration)
    {
        if (_inProgress)
            return;
        
        if (parentPanel)
            parentPanel.MoveBack(duration);
        
        _rectTransform.DOMoveZ(_rectTransform.anchoredPosition3D.z + 0.05f, duration)
            .OnStart(() => _inProgress = true)
            .OnComplete(() => _inProgress = false);
    }
    
    private void MoveForward(float duration)
    {
        if (_inProgress)
            return;
        
        if (parentPanel)
            parentPanel.MoveForward(duration);
        
        _rectTransform.DOMoveZ(_rectTransform.anchoredPosition3D.z - 0.05f, duration)
            .OnStart(() => _inProgress = true)
            .OnComplete(() => _inProgress = false);
    }
}
