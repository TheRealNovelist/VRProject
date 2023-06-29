using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialEmissionHighlight : MonoBehaviour, IMovementNodeResponse
{
    [SerializeField] private Renderer highlight;
    [SerializeField] private Color defaultColor = Color.cyan;
    [SerializeField] private Color selectedColor = Color.green;
    [SerializeField] private Color rejectedColor = Color.red;
    
    public void Selected(bool allow = true)
    {
        Color color = allow ? selectedColor : rejectedColor;
        color.a = 0.3f;
        highlight.material.SetColor("_EmissionColor", color);
    }

    public void Deselected()
    {
        Color color = defaultColor;
        color.a = 0.3f;
        highlight.material.SetColor("_EmissionColor", color);
    }

    public void SetActive(bool active)
    {
        Color color = highlight.material.GetColor("_EmissionColor");
        color.a = active ? 0.3f : 0;
        highlight.material.SetColor("_EmissionColor", color);
    }
}