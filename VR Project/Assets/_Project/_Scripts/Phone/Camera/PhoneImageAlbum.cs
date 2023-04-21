using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "New Album", menuName = "Utility/Camera Album")]
public class PhoneImageAlbum : ScriptableObject
{
    [Header("Directory Settings")]
    [SerializeField] private string folderName;
    [SerializeField] private string namePrefix;
    [SerializeField] private string nameSuffix;
    [Space]
    [SerializeField] private string overrideName;
    
    [Space]
    [SerializeField] private RenderTexture refTexture;

    private string NameFormat => overrideName == "" ? namePrefix + DateTime.UtcNow.ToLocalTime().ToString("dd-MM-yyyy_HH.mm.ss") + nameSuffix : overrideName;

    private string DirectoryPath
    {
        get
        {
            string folder = folderName != "" ? "/" + folderName : "";
            string path = Application.persistentDataPath + folder;
            Directory.CreateDirectory(path);

            return path;
        }
    }
    
    public List<PhoneImage> allImages = new();
    
    public PhoneImage CreateImage() => CreateImage(refTexture);
    public PhoneImage CreateImage(RenderTexture texture)
    {
        PhoneImage newImage = new PhoneImage(texture, DirectoryPath, NameFormat);
        
        allImages.Add(newImage);
        return newImage;
    }

    public void LoadAllImageFromFolder()
    {
        string[] files = Directory.GetFiles(DirectoryPath, "*.png");

        foreach (string file in files)
        {
            allImages.Add(new PhoneImage(file, refTexture.width, refTexture.height));
        }
    }

    public void DeleteImage(PhoneImage image)
    {
        image.DeleteImage();
        allImages.Remove(image);
    }
}
