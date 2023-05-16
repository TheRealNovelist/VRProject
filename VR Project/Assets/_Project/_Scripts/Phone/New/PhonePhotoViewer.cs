using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PhonePhotoViewer : App
{
    [SerializeField] private RawImage image;
    [SerializeField] private Album album;

    private int _currentIndex = 0;
    private int _photoCount => album.allPhotos.Count;
    
    [Header("Buttons")] 
    [SerializeField] private GameObject upButton;
    [SerializeField] private GameObject downButton;
    [SerializeField] private GameObject trashButton;
    
    private void Start()
    {
        if (album.allPhotos.Count <= 0)
        {
            image.gameObject.SetActive(false);
        }
        else
        {
            image.gameObject.SetActive(true);
            SetTexture(album.allPhotos[_currentIndex]);
        }
    }

    private void UpdateButtons()
    {
        upButton.SetActive(_currentIndex != 0 && _photoCount > 0);
        downButton.SetActive(_currentIndex != _photoCount - 1 && _photoCount > 0);
        trashButton.SetActive(_photoCount > 0);
    }

    public void Up()
    {
        if (_currentIndex <= 0) return;
        
        _currentIndex -= 1;
        SetTexture(album.allPhotos[_currentIndex]);

        UpdateButtons();
    }
    
    public void Down()
    {
        if (_currentIndex >= album.allPhotos.Count - 1) return;
        
        _currentIndex += 1;
        SetTexture(album.allPhotos[_currentIndex]);
        
        UpdateButtons();
    }

    public void Delete()
    {
        Photo currentPhoto = album.allPhotos[_currentIndex];
        
        //If current index is == 0
        if (_currentIndex == 0)
        {
            //If there are pages still exist, move to the next page on the right
            if (_photoCount > 1)
            {
                Down();
            }
            //Else, display blank page
            else
            {
                image.gameObject.SetActive(false);
            }
        }
        else
        {
            Up();
        }
        
        album.DeletePhoto(currentPhoto);
    }

    private void SetTexture(Photo photo)
    {
        image.texture = photo.texture;
    }

    public override void Enter(float transitionDuration = 0)
    {
        gameObject.SetActive(true);
        
        UpdateButtons();
        
        if (transitionDuration != 0)
        {
            anim.MoveFromOrigin(new Vector3(0, -1, 0));
            anim.MoveY(transitionDuration, 1).OnComplete(() => canvasGrp.interactable = true);
        }
        else
        {
            canvasGrp.interactable = true;
        }
    }
    
    public override void Exit(float transitionDuration = 0)
    {
        canvasGrp.interactable = false;
        if (transitionDuration > 0)
        {
            anim.MoveY(transitionDuration, -1).OnComplete(() => gameObject.SetActive(false));
            
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
