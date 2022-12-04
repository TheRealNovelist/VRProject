using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using BNG;
using DG.Tweening;

public class PauseManager : MonoBehaviour
{
    public static bool IsGamePaused = false;

    [Header("Pause Items")]
    [SerializeField] private PanelManager pausePanel;
    [SerializeField] private List<GameObject> pauseItems;
    
    [Header("Settings")]
    [SerializeField] private InputActionReference InputAction = default;
    [SerializeField] private Transform anchor;
    
    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration;
    [Range(0f, 1f)][SerializeField] private float fadeAmount = 0.7f;

    private CustomFader fader;

    private bool inProgress;

    private void Start()
    {
        fader = new CustomFader(Color.black);
        
        foreach (GameObject item in pauseItems)
        {
            item.SetActive(IsGamePaused);
        }
    }
    
    private void OnEnable() {
        InputAction.action.performed += ToggleActive;
    }

    private void OnDisable() {
        InputAction.action.performed -= ToggleActive;
    }
    
    public void ToggleActive(InputAction.CallbackContext context)
    {
        SetPause(!IsGamePaused);
    }
    
    public void SetPause(bool isPaused)
    {
        if (inProgress)
            return;

        inProgress = true;
        
        if (!isPaused)
        {
            //Unpause the game
            fader.Fade(fadeAmount, 0f, fadeDuration).OnComplete(() => inProgress = false);
            pausePanel.MinimizeAll();

            foreach (GameObject item in pauseItems)
            {
                item.SetActive(false);
            }
            
            IsGamePaused = false;
            Time.timeScale = 1f;
        }
        else
        {
            //Pause game
            fader.Fade(0f, fadeAmount, fadeDuration).OnComplete(() => inProgress = false);
            
            //Anchor the static position of the panel according to player orientation.
            transform.position = anchor.position;
            transform.rotation = anchor.rotation;
            
            pausePanel.TurnOnPanel();

            foreach (GameObject item in pauseItems)
            {
                item.SetActive(true);
            }
            
            IsGamePaused = true;
            Time.timeScale = 0f;
        }
    }
}
