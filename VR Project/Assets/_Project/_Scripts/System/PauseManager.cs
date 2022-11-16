using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using BNG;

public class PauseManager : MonoBehaviour
{
    public static bool IsGamePaused = false;

    [SerializeField] private PanelManager pausePanel;
    [SerializeField] private InputActionReference InputAction = default;
    
    private void Start()
    {

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
        if (!isPaused)
        {
            //Unpause the game
            pausePanel.MinimizeAll();

            IsGamePaused = false;
            Time.timeScale = 1f;
        }
        else
        {
            //Pause game
            pausePanel.TurnOnPanel();

            IsGamePaused = true;
            //Time.timeScale = 0f;
        }
    }
}
