using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(TMP_Text))]
public class BindNameToText : MonoBehaviour
{
    [SerializeField] private GameObject objectToBind;
    private TMP_Text text => GetComponent<TMP_Text>();
    
    private void Update()
    {
        if (!Application.IsPlaying(gameObject))
        {
            if (objectToBind)
                text.text = objectToBind.name;
        }
    }
}
