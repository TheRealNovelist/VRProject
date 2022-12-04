using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class CustomUIElement : MonoBehaviour
{
    public Blocker blocker;

    private RectTransform _rectTransform => GetComponent<RectTransform>();
    
    private Vector3 _elementScale;
    private float _elementWidth;
    private float _elementHeight;

    private void SetCanvasLayer(int order)
    {
        if (!GetComponent<Canvas>()) return;
        GetComponent<Canvas>().sortingOrder = order;
    }
    
    private void Awake()
    {
        _elementScale = _rectTransform.localScale;
        _elementWidth = _rectTransform.rect.width * _elementScale.x;
        _elementHeight = _rectTransform.rect.height * _elementScale.y;
    }

    public Tween Expand(float duration)
    {
        gameObject.SetActive(true);
        if (blocker) blocker.SetBlockerAlpha(0, 0, duration);
        _rectTransform.localScale = Vector3.zero;
        return _rectTransform.DOScale(_elementScale, duration).SetUpdate(true);
    }
    
    public Tween Minimize(float duration)
    {
        if (blocker) blocker.SetBlockerAlpha(0, 0, duration);
        _rectTransform.localScale = _elementScale;
        return _rectTransform.DOScale(Vector3.zero, duration)
            .SetUpdate(true)
            .OnComplete(() => gameObject.SetActive(false));
    }

    public float GetWidth()
    {
        return _elementWidth;
    }

    public float GetHeight()
    {
        return _elementHeight;
    }
    
    public void SetLocalPosition(Vector3 position)
    {
        _rectTransform.localPosition = position;
    }

    public void Move(Vector3 increment, Vector3 spacing)
    {
        _rectTransform.localPosition = new Vector3(
            _rectTransform.localPosition.x + increment.x * (_elementWidth + spacing.x), 
            _rectTransform.localPosition.y + increment.y * (_elementHeight + spacing.y), 
            _rectTransform.localPosition.z + increment.z * spacing.z);
    }

    public Tween Move(float duration, Vector3 increment, Vector3 spacing)
    {
        Vector3 newPos = new Vector3(
            _rectTransform.localPosition.x + increment.x * (_elementWidth + spacing.x), 
            _rectTransform.localPosition.y + increment.y * (_elementHeight + spacing.y), 
            _rectTransform.localPosition.z + increment.z * spacing.z);

        return _rectTransform.DOLocalMove(newPos, duration).SetUpdate(true);
    }

    public Tween MoveX(float duration, float increment, float spacing)
    {
        return _rectTransform.DOLocalMoveX(_rectTransform.localPosition.x + increment * (_elementWidth + spacing), duration).SetUpdate(true);
    }

    public Tween MoveY(float duration, float increment, float spacing)
    {
        return _rectTransform.DOLocalMoveY(_rectTransform.localPosition.y + increment * (_elementHeight + spacing), duration).SetUpdate(true);
    }

    public Tween MoveZ(float duration, float increment, float spacing)
    {
        return _rectTransform.DOLocalMoveZ(_rectTransform.localPosition.z + increment * spacing, duration).SetUpdate(true);
    }

    public void SetCanvasLayer(int start, int end, float duration) => StartCoroutine(CanvasLayerChange(start, end, duration));

    private IEnumerator CanvasLayerChange(int start, int end, float duration)
    {
        SetCanvasLayer(start);
        yield return new WaitForSeconds(duration);
        SetCanvasLayer(end);
    }
    
    public void ResetPosition()
    {
        _rectTransform.localPosition = new Vector3(0, 0, 0);
    }
}
