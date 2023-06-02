using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Vector3 offset;
    [SerializeField] private Camera cameraToSwitch;
    
    private Transform currentAnchor => gameObject.transform;
    private Transform linkedAnchor;
    
    
    private void Awake()
    {
        linkedAnchor = new GameObject("Future Anchor").transform;
    }

    private void Update()
    {
        linkedAnchor.position = currentAnchor.position + offset;
        linkedAnchor.rotation = currentAnchor.rotation;
    }

    public void SwitchCamera(bool toLinked)
    {
        var cameraTransform = cameraToSwitch.transform;
        
        cameraTransform.parent = !toLinked ? currentAnchor : linkedAnchor;
        cameraTransform.localPosition = Vector3.zero;
        cameraTransform.localRotation = Quaternion.identity;
    }
}
