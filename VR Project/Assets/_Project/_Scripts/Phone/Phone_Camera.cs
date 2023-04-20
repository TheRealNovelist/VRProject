using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Phone_Camera : MonoBehaviour, IPhoneApp
{
    [SerializeField] private CustomUIElement cameraCanvas;
    [SerializeField] private float transitionDuration = 0.5f;
    [SerializeField] private Vector3 spacing;

    [Header("Image Settings")]
    [SerializeField] private RenderTexture cameraTexture;
    [SerializeField] private string tempPath;
    [SerializeField] private RawImage image;
    
    [Header("Camera Settings")]
    [SerializeField] private Camera cameraOnPhone;
    [SerializeField] private TextMeshProUGUI displayText;
    [Space]
    [SerializeField] private LayerMask normalMask; 
    [SerializeField] private LayerMask xRayMask; 
    [SerializeField] private LayerMask ghostMask;

    [SerializeField] private CameraMode initialMode = CameraMode.Normal;
    private CameraMode _mode;
    
    private enum CameraMode
    {
        Normal = 0,
        XRay = 1,
        Ghost = 2
    }
    
    private void Start()
    {
        SwitchMode(initialMode);
    }

    void SwitchMode(CameraMode mode)
    {
        switch (mode)
        {
            case CameraMode.Normal:
                cameraOnPhone.cullingMask = normalMask;
                _mode = CameraMode.Normal;
                displayText.text = "Normal";
                break;
            case CameraMode.XRay:
                cameraOnPhone.cullingMask = xRayMask;
                _mode = CameraMode.XRay;
                displayText.text = "X-Ray";
                break;
            case CameraMode.Ghost:
                cameraOnPhone.cullingMask = ghostMask;
                _mode = CameraMode.Ghost;
                displayText.text = "Ghost";
                break;
        }
    }
    
    public void OnJoystickMove(float x, float y)
    {

    }

    public void StartApp()
    {
        cameraCanvas.ResetPosition();
        cameraCanvas.Move(new Vector3(0, -1, 0), spacing);
        cameraCanvas.MoveY(transitionDuration, 1, spacing.y);
    }

    public void OnThumbStickDown()
    {
        GetImage();
    }

    public Tween ExitApp()
    {
        return cameraCanvas.MoveY(transitionDuration, -1, spacing.y).OnComplete(() => gameObject.SetActive(false));
    }

    void GetImage()
    {
        Texture2D texture2D = new Texture2D(cameraTexture.width, cameraTexture.height, TextureFormat.ARGB32, false);
        RenderTexture.active = cameraTexture;
        texture2D.ReadPixels(new Rect(0, 0, cameraTexture.width, cameraTexture.height), 0, 0);
        texture2D.Apply();

        string path = tempPath + "/" + "SampleImage.png";

        byte[] bytes = texture2D.EncodeToPNG();
        
        File.WriteAllBytes(path, bytes);
        
        SetImage();
    }

    void SetImage()
    {
        Texture2D texture2D = new Texture2D(cameraTexture.width, cameraTexture.height);
        string path = tempPath + "/" + "SampleImage.png";
        byte[] bytes = File.ReadAllBytes(path);

        texture2D.LoadImage(bytes);
        texture2D.Apply();

        image.texture = texture2D;
    }
}
