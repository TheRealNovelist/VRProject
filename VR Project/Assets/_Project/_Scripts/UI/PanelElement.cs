using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class PanelElement : MonoBehaviour
{
    [SerializeField] private GameObject blocker;

    [HideInInspector] public PanelElement parentPanel;
    
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

    private void SetBlockerActive(bool isActive)
    {
        blocker.SetActive(isActive);
    }
    
    public void Expand(float duration)
    {
        if (_inProgress)
            return;
        
        gameObject.SetActive(true);
        _rectTransform.localScale = Vector3.zero;
        _rectTransform.DOScale(_activeScale, duration)
            .OnStart(() => _inProgress = true)
            .OnComplete(() =>
            {
                _inProgress = false;
                SetBlockerActive(false);
            });
    }

    public void Minimize(float duration)
    {
        if (_inProgress)
            return;
        
        SetBlockerActive(true);
        _rectTransform.localScale = _activeScale;
        _rectTransform.DOScale(Vector3.zero, duration)
            .OnStart(() => _inProgress = true)
            .OnComplete(() =>
            {
                _inProgress = false;
                gameObject.SetActive(false);
            });
    }
    
    public void MoveLeft(float duration, float spacing, bool isBlockerActive = false, bool setBlockerDelayed = false)
    {
        if (_inProgress)
            return;
        
        if (!setBlockerDelayed)
            SetBlockerActive(isBlockerActive);
        
        if (parentPanel)
            parentPanel.MoveBack();
        
        SetCanvasLayer(1);
        _rectTransform.DOMoveX(_rectTransform.anchoredPosition.x - _moveFactor - spacing, duration)
            .OnStart(() => _inProgress = true)
            .OnComplete(() =>
            {
                _inProgress = false;
                SetCanvasLayer(0);
                if (setBlockerDelayed) SetBlockerActive(isBlockerActive);
            });
    }

    public void MoveRight(float duration, float spacing, bool isBlockerActive = false, bool setBlockerDelayed = false)
    {
        if (_inProgress)
            return;
        
        if (!setBlockerDelayed)
            SetBlockerActive(isBlockerActive);
        
        if (parentPanel)
            parentPanel.MoveForward();
        
        SetCanvasLayer(1);
        _rectTransform.DOMoveX(_rectTransform.anchoredPosition.x + _moveFactor + spacing, duration)
            .OnStart(() => _inProgress = true)
            .OnComplete(() =>
            {
                _inProgress = false;
                SetCanvasLayer(0);
                if (setBlockerDelayed) SetBlockerActive(isBlockerActive);
            });
    }

    private void MoveBack()
    {
        if (_inProgress)
            return;
        
        _rectTransform.DOMoveZ(_rectTransform.anchoredPosition3D.z + 0.05f, 0.1f)
            .OnStart(() => _inProgress = true)
            .OnComplete(() => _inProgress = false);
    }
    
    private void MoveForward()
    {
        if (_inProgress)
            return;
        
        _rectTransform.DOMoveZ(_rectTransform.anchoredPosition3D.z - 0.05f, 0.1f)
            .OnStart(() => _inProgress = true)
            .OnComplete(() => _inProgress = false);
    }
}
