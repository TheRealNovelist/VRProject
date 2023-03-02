using System;
using System.Collections;
using System.Collections.Generic;
using BNG;
using UnityEngine;
using TMPro;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private Damageable playerHealth;
    [SerializeField] private TextMeshProUGUI text;
    
    private void OnGUI()
    {
        text.text = playerHealth.Health + " : " + playerHealth.maxHealth;
    }
}
