using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Album", menuName = "Utility/Camera Album")]
public class ImageAlbum : ScriptableObject
{
    [SerializeField] private string namePrefix;
    [SerializeField] private string tempPath;

    [SerializeField] private RenderTexture texture;
    
    public string[] files;
    public List<Texture2D> allImages = new();
    
    public void CreateImage()
    {
        Texture2D texture2D = new Texture2D(texture.width, texture.height, TextureFormat.ARGB32, false);
        RenderTexture.active = texture;
        texture2D.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
        texture2D.Apply();
        
        byte[] bytes = texture2D.EncodeToPNG();

        string captureTime = DateTime.UtcNow.ToLocalTime().ToString("dd-MM-yyyy_HH.mm.ss");
        string imageName = captureTime;
        string path = tempPath + "/" + imageName + ".png";
        
        Debug.Log("Created new image: " + imageName);
        
        File.WriteAllBytes(path, bytes);
        
        allImages.Add(texture2D);
    }

    public void GetAllImageFromFolder()
    {
        string path = @"D:\Unity Project\VRProject\TempImages\";

        files = Directory.GetFiles(path, "*.png");
        
        foreach (string file in files)
        {
            Texture2D texture2D = new Texture2D(texture.width, texture.height);

            string filePath = file;
            
            byte[] bytes = File.ReadAllBytes(filePath);
            
            texture2D.LoadImage(bytes);
            texture2D.Apply();
            
            allImages.Add(texture2D);
        }
    }

    // public void SetImage(RenderTexture texture, RawImage imageToSet)
    // {
    //     Texture2D texture2D = new Texture2D(texture.width, texture.height);
    //
    //     byte[] bytes = File.ReadAllBytes(path);
    //
    //     texture2D.LoadImage(bytes);
    //     texture2D.Apply();
    //
    //     imageToSet.texture = texture2D;
    // }
}
