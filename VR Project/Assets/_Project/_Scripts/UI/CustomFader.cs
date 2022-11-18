using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CustomFader
{
    private readonly GameObject fadeObject;
    private readonly CanvasGroup canvasGroup;
    
    public CustomFader(Color fadeColor)
    {
        fadeObject = new GameObject();
        fadeObject.transform.parent = Camera.main.transform;
        fadeObject.transform.localPosition = new Vector3(0, 0, 0.03f);
        fadeObject.transform.localEulerAngles = Vector3.zero;
        fadeObject.name = "CustomFader";

        var fadeCanvas = fadeObject.AddComponent<Canvas>();
        fadeCanvas.renderMode = RenderMode.WorldSpace;
        fadeCanvas.sortingLayerName = "Fader";
        fadeCanvas.sortingOrder = 100; // Make sure the canvas renders on top

        canvasGroup = fadeObject.AddComponent<CanvasGroup>();
        canvasGroup.interactable = false;
        
        var fadeImage = fadeObject.AddComponent<Image>();
        fadeImage.color = fadeColor;
        fadeImage.raycastTarget = false;

        // Stretch the image
        var fadeObjectRect = fadeObject.GetComponent<RectTransform>();
        fadeObjectRect.anchorMin = new Vector2(1, 0);
        fadeObjectRect.anchorMax = new Vector2(0, 1);
        fadeObjectRect.pivot = new Vector2(0.5f, 0.5f);
        fadeObjectRect.sizeDelta = new Vector2(0.2f, 0.2f);
        fadeObjectRect.localScale = new Vector2(2f, 2f);
        
        fadeObject.SetActive(false);
    }

    public Tween Fade(float from, float to, float duration)
    {
        fadeObject.SetActive(true);
        canvasGroup.alpha = from;
        return canvasGroup.DOFade(to, duration).SetUpdate(true)
            .OnComplete(() =>
            {
                if (to == 0)
                {
                    fadeObject.SetActive(false);
                }
            });
    }
}

