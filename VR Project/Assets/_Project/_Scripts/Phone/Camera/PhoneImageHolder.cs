using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class PhoneImageHolder : MonoBehaviour
{
    public PhoneImage Image { get; private set; }
    private RawImage _rImage;

    public void Awake()
    {
        _rImage = GetComponent<RawImage>();
    }

    public void SetImage(PhoneImage imageToSet)
    {
        Image = imageToSet;
        _rImage.texture = imageToSet.image;
    }
}