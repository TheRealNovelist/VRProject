using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private InputActionReference _reference;

    public int maxHealth = 10;
    public int health = 10;

    private void OnEnable()
    {
        _reference.action.performed += TakeDamage;
    }

    private void OnDisable()
    {
        _reference.action.performed -= TakeDamage;
    }

    private void TakeDamage(InputAction.CallbackContext context)
    {
        if (health == 0)
            return;
        
        health -= 1;
    }
}
