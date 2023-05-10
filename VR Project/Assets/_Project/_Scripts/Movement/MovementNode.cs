using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(CapsuleCollider))]
public class MovementNode : MonoBehaviour
{
    [SerializeField] private List<MovementNode> connections;
    [SerializeField] private bool usePlayerHeight = true;

    [SerializeField] private GameObject cursor;
    [SerializeField] private GameObject highlight;

    private void Awake()
    {
        OnDeselected();
        SetNodeActive(false);
    }

    public void OnSelected()
    {
        if (highlight) highlight.SetActive(true);
        Debug.Log($"Selected {gameObject.name}");
    }

    public void OnDeselected()
    {
        if (highlight) highlight.SetActive(false);
        Debug.Log($"Deselected {gameObject.name}");
    }

    public void OnTeleportTo()
    {
        OnDeselected();
        SetNodeActive(false);
    }

    public void OnTeleportOut()
    {
        SetConnectionsActive(false);
    }

    public void SetConnectionsActive(bool isActive)
    {
        foreach (var connection in connections)
        {
            connection.SetNodeActive(isActive);
        }
    }

    private void SetNodeActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public Vector3 GetPosition(Transform player)
    {
        return new Vector3(transform.position.x, usePlayerHeight ? player.transform.position.y : transform.position.y, transform.position.z);
    }
    
    public void DrawConnection(bool focused, MovementNode from = null)
    {
        if (connections.Count <= 0)
            return;
        
        foreach (MovementNode node in connections.Where(node => from != node))
        {
            Gizmos.color = focused ? Color.green : Color.red;
            Gizmos.DrawLine(transform.position, node.transform.position);
            node.DrawConnection(false, this);
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        DrawConnection(true, this);
    }
}
