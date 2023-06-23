using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class LightsOutDisplay : MonoBehaviour
{
    public LightsOutLock bindLock;
    public Vector2Int position;

    [SerializeField] private bool updateInEditorMode;

    [SerializeField] private Graphic graphic;

    [SerializeField] private Color trueColor = Color.green;
    [SerializeField] private Color falseColor = Color.red;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!updateInEditorMode && !Application.IsPlaying(this)) return;
        
        if (!bindLock) return;
        
        if (!bindLock.IsValid(position))
        {
            Debug.Log("Lock position is not valid");
            return;
        }
        
        if (graphic)
            SetDisplayActive(bindLock.lights[position.x, position.y]);
    }

    void SetDisplayActive(bool isActive)
    {
        graphic.color = isActive ? trueColor : falseColor;
    }
}
