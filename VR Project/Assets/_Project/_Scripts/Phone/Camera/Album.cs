using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Album : MonoBehaviour
{
    [Header("Directory Settings")]
    [SerializeField] private string folderName;
    [SerializeField] private string namePrefix;
    [SerializeField] private string nameSuffix;
    [Space]
    [SerializeField] private string overrideName;
        
    [Space]
    [SerializeField] private RenderTexture refTexture;
    

    private string NameFormat => 
        overrideName != "" ? overrideName 
            : namePrefix + DateTime.UtcNow.ToLocalTime().ToString("dd-MM-yyyy_HH.mm.ss") + nameSuffix;

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
    
    public List<PhoneImage> allImages;

    private void Awake()
    {
        bool success = LoadAllImageFromFolder(out allImages);
        if (!success)
        {
            Debug.Log("No image found in file");
        }
    }
    
    public void CreateImage() => CreateImage(refTexture);
    public void CreateImage(RenderTexture texture)
    {
        PhoneImage newImage = new PhoneImage(texture, DirectoryPath, NameFormat);
              
        allImages.Add(newImage);
    }

    public bool LoadAllImageFromFolder(out List<PhoneImage> images)
    {
        string[] files = Directory.GetFiles(DirectoryPath, "*.png");

        if (files.Length <= 0)
        {
            images = new List<PhoneImage>();
            return false;
        }
        
        List<PhoneImage> allImagesFromFolder = new();

        foreach (string file in files)
        {
            allImagesFromFolder.Add(new PhoneImage(file, refTexture.width, refTexture.height));
        }

        images = allImagesFromFolder;
        return true;
    }

    public void DeleteImage(PhoneImage image)
    {
        allImages.Remove(image);
        image.DeleteImage();
    }
}
