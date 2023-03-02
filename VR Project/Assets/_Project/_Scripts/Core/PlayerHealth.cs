using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int health = 10;
    
    private void TakeDamage(InputAction.CallbackContext context)
    {
        if (health == 0)
            return;
        
        health -= 1;
    }
}
