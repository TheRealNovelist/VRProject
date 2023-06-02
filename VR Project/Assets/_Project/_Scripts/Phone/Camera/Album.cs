using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Album : MonoBehaviour
{
    [SerializeField] private int photoLimit;
    
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
    
    public List<Photo> allPhotos;

    private void Awake()
    {
        bool success = LoadAllPhotoFromFolder(out allPhotos);
        if (!success)
        {
            Debug.Log("No image found in file");
        }
    }

    public int GetPhotoLimit()
    {
        return photoLimit;
    }
    
    public bool HasReachedLimit()
    {
        return allPhotos.Count >= photoLimit;
    }
    
    public bool CreatePhoto() => CreatePhoto(refTexture);
    public bool CreatePhoto(RenderTexture texture)
    {
        if (HasReachedLimit())
        {
            return false;
        }
        
        Photo newImage = new (texture, DirectoryPath, NameFormat);
              
        allPhotos.Add(newImage);
        return true;
    }

    public bool LoadAllPhotoFromFolder(out List<Photo> photos)
    {
        photos = new List<Photo>();
        
        string[] files = Directory.GetFiles(DirectoryPath, "*.png");

        if (files.Length <= 0)
        {
            return false;
        }

        foreach (string file in files)
        {
            photos.Add(new Photo(file, refTexture.width, refTexture.height));
        }
        
        return true;
    }

    public void DeletePhoto(Photo photo)
    {
        allPhotos.Remove(photo);
        photo.DeleteImage();
    }
}
