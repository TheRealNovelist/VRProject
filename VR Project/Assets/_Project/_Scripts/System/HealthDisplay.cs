using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private TextMeshProUGUI text;
    
    private void OnGUI()
    {
        text.text = playerHealth.health + " : " + playerHealth.maxHealth;
    }
}
