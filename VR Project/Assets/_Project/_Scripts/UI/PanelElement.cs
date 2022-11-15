using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class PanelElement : MonoBehaviour
{
    public PanelBlocker blocker;

    private RectTransform _rectTransform;
    
    private Vector3 _canvasScale;
    private float _canvasWidth;

    private void SetCanvasLayer(int order)
    {
        GetComponent<Canvas>().sortingOrder = order;
    }
    
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasScale = _rectTransform.localScale;
        _canvasWidth = _rectTransform.rect.width * _canvasScale.x;
    }

    public Tween Expand(float duration)
    {
        blocker.SetBlockerAlpha(0, 0, duration);
        gameObject.SetActive(true);
        _rectTransform.localScale = Vector3.zero;
        return _rectTransform.DOScale(_canvasScale, duration).SetUpdate(true);
    }
    
    public Tween Minimize(float duration)
    {
        blocker.SetBlockerAlpha(0, 0, duration);
        _rectTransform.localScale = _canvasScale;
        return _rectTransform.DOScale(Vector3.zero, duration)
            .SetUpdate(true)
            .OnComplete(() => gameObject.SetActive(false));
    }

    public Tween MoveX(float duration, float direction, float spacing)
    {
        return _rectTransform.DOLocalMoveX(_rectTransform.localPosition.x + direction * (_canvasWidth + spacing), duration)
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
    

    public Tween MoveZ(float duration, float direction, float spacing)
    {
        return _rectTransform.DOLocalMoveZ(_rectTransform.localPosition.z + direction * spacing, duration).SetUpdate(true);
    }
    
    public void ResetPanel()
    {
        _rectTransform.localPosition = new Vector3(0, 0, 0);
    }
}
