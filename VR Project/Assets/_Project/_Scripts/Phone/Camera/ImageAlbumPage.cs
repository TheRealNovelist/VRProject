using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageAlbumPage : MultiPage
{
    [Header("Image")]
    [SerializeField] private Album album;
    
    
    
    public void CreateImagePage(PhoneImage image)
    {
        Debug.Log("Create new image page!");
        CustomUIElement element = CreatePage();
        PhoneImageHolder imageHolder = element.GetComponent<PhoneImageHolder>();

        imageHolder.SetImage(image);
    }

    public void DeleteImage()
    {
        DeletePage(page => album.DeleteImage(page.GetComponent<PhoneImageHolder>().Image));
    }
}
