using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class PhoneImageHolder : MonoBehaviour
{
    public Photo image { get; private set; }
    private RawImage _rImage;

    public void Awake()
    {
        _rImage = GetComponent<RawImage>();
    }

    public void OnDisable()
    {
        image = null;
        _rImage.texture = null;
    }

    public void SetImage(Photo imageToSet)
    {
        image = imageToSet;
        _rImage.texture = imageToSet.texture;
    }
}