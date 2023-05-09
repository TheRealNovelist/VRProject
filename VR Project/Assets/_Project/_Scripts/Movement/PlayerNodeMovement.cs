using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNodeMovement : MonoBehaviour
{
    [Header("Initialization")]
    [SerializeField] private MovementNode startingNode;

    [Header("Components")] 
    [SerializeField] private Transform pointer;

    private MovementNode currentNodeSelected;
    
    private void Awake()
    {
        TeleportToNode(startingNode);
    }

    private void TeleportToNode(MovementNode node)
    {
        transform.position = node.GetPosition(transform);
        node.OnTeleportTo();
    }

    private void SearchNode()
    {
        if (Physics.Raycast(pointer.transform.position, pointer.forward, out RaycastHit hit, Mathf.Infinity))
        {
            GameObject obj = hit.collider.gameObject;
            
            if (obj.TryGetComponent(out MovementNode node))
            {
                if (node != currentNodeSelected)
                {
                    currentNodeSelected.OnDeselected();
                    currentNodeSelected = null;
                }
                
                currentNodeSelected = node;
                node.OnSelected();
            }
            else
            {
                if (currentNodeSelected == null) return;
                
                currentNodeSelected.OnDeselected();
                currentNodeSelected = null;
            }
        }
    }
}
