using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageAlbumPage : MultiPage
{
    [Header("Image")]
    [SerializeField] private Album album;
    
    
    
    public void CreateImagePage(Photo image)
    {
        Debug.Log("Create new image page!");
        UIAnimation element = CreatePage();
        PhoneImageHolder imageHolder = element.GetComponent<PhoneImageHolder>();

        imageHolder.SetImage(image);
    }

    public void DeleteImage()
    {
        DeletePage(page => album.DeletePhoto(page.GetComponent<PhoneImageHolder>().image));
    }
}
