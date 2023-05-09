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

    [SerializeField] private GameObject highlight;
    
    private Collider _collider => GetComponent<Collider>();

    private void Awake()
    {
        foreach (MovementNode node in connections)
        {
            
        }
    }

    public void OnSelected()
    {
        highlight.SetActive(true);
    }

    public void OnDeselected()
    {
        highlight.SetActive(false);
    }

    public void OnTeleportTo()
    {
        _collider.enabled = false;
    }

    public void OnTeleportOut()
    {
        _collider.enabled = true;
    }

    public void SetNodeActive(bool isActive)
    {
        
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
