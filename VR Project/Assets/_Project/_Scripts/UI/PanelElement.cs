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

    public Tween Expand(float duration)
    {
        return _rectTransform.DOScale(_activeScale, duration)
            .SetUpdate(true)
            .OnStart(() =>
            {
                gameObject.SetActive(true);
                _rectTransform.localScale = Vector3.zero;
            });
    }
    
    public Tween Minimize(float duration)
    {
        return _rectTransform.DOScale(Vector3.zero, duration)
            .SetUpdate(true)
            .OnStart(() =>
            {
                _rectTransform.localScale = _activeScale;
            })
            .OnComplete(() => gameObject.SetActive(false));
    }

    public Tween MoveX(float duration, float direction, float spacing)
    {
        return _rectTransform.DOLocalMoveX(_rectTransform.localPosition.x + direction * (_moveFactor + spacing), duration)
            .SetUpdate(true)
            .OnStart(() =>
            {
                SetCanvasLayer(1);
            })
            .OnComplete(() =>
            {
                SetCanvasLayer(0);
            });
    }
    

    private Tween MoveZ(float duration, float direction, float spacing)
    {
        return _rectTransform.DOLocalMoveZ(_rectTransform.localPosition.z + direction * spacing, duration).SetUpdate(true);
    }
    
}
