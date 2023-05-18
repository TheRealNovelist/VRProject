using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[SelectionBase]
[RequireComponent(typeof(Collider))]
public class MovementNode : MonoBehaviour
{
    [Header("Connections")]
    [SerializeField] private bool allowTeleportToAllNode;
    [SerializeField, HideIf("allowTeleportToAllNode")] private List<MovementNode> connections;
    
    [Header("Setting")]
    [SerializeField] private GameObject highlight;
    [SerializeField] private bool disallowItemOnHand;
    [SerializeField] private bool moveOnTeleport = true;
    [SerializeField] private bool rotateOnTeleport = true;
    [SerializeField] private bool fadeOutOnTeleport = true;
    
    [Header("Teleport Event")]
    [SerializeField] private bool useTeleportEvent;
    [SerializeField, ShowIf("useTeleportEvent")] private UnityEvent OnTeleportTo;
    [SerializeField, ShowIf("useTeleportEvent")] private UnityEvent OnTeleportOut;
    
    private List<Renderer> highlightRenderers;
    private bool _allowTeleport = true;

    private List<MovementNode> _fromNodes = new();

    private void Awake()
    {
        //If there is no specified connection, use all connected
        if (allowTeleportToAllNode)
        {
            FindAllConnection();
        }
        
        foreach (MovementNode node in connections)
        {
            node.AddToFromNode(this);
        }
        
        highlightRenderers = highlight.GetComponentsInChildren<Renderer>().ToList();
        
        foreach (var highlightRenderer in highlightRenderers)
        {
            highlightRenderer.material.color = Color.green;
        }
    }

    private void FindAllConnection()
    {
        connections = new List<MovementNode>(FindObjectsOfType<MovementNode>(true).Where(node => node != this));
    }

    private void Start()
    {
        OnDeselected();
        SetNodeActive(false);
    }

    [Button]
    public void Connect(MovementNode node)
    {
        if (connections.Contains(node) || node == this)
            return;

        connections.Add(node);
        node.AddToFromNode(this);
    }
    
    //Callback to all nodes that this node can be teleported to
    private void AddToFromNode(MovementNode node)
    {
        if (_fromNodes.Contains(node))
            return;
        
        _fromNodes.Add(node);
    }
    
    public void OnSelected(bool itemOnHand)
    {
        if (highlight) highlight.SetActive(true);

        if (!disallowItemOnHand) return;

        _allowTeleport = !itemOnHand;
        
        foreach (var highlightRenderer in highlightRenderers)
        {
            highlightRenderer.material.color = itemOnHand ? Color.red : Color.green;
        }
    }

    public bool CanTeleport() => _allowTeleport;

    public void OnDeselected()
    {
        if (highlight) highlight.SetActive(false);
    }

    public void TeleportOut()
    {
        if (useTeleportEvent) OnTeleportOut.Invoke();
        
        SetNodeActive(false);
    }

    public void TeleportTo(Transform player, int playerHeight = 2)
    {
        if (useTeleportEvent) OnTeleportTo.Invoke();
        
        if (!moveOnTeleport) return;
        
        player.position = GetPosition(playerHeight);
        if (rotateOnTeleport)
        {
            player.rotation = Quaternion.LookRotation(transform.forward);
        }
    }
    
    public void TeleportTo(Transform player, out bool animate, int playerHeight = 0)
    {
        TeleportTo(player, playerHeight);
        
        animate = fadeOutOnTeleport;
    }

    private Vector3 GetPosition(int height = 2)
    {
        return new Vector3(
            transform.position.x, 
            transform.position.y + height, 
            transform.position.z);
    }

    public void SetConnectionsActive(bool isActive)
    {
        foreach (var connection in connections)
        {
            if (connection.enabled == false)
                continue;

            connection.SetNodeActive(isActive);
        }
    }

    public void SetNodeActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public void DrawConnection()
    {
        if (connections.Count <= 0)
            return;
        
        foreach (MovementNode connection in connections)
        {
            if (connection.enabled == false)
                continue;
            
            Gizmos.color = Color.green;
            
            var origin = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            var connectionPoint = new Vector3(connection.transform.position.x, connection.transform.position.y + 1, connection.transform.position.z);
            
            Gizmos.DrawLine(origin, connectionPoint);
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        DrawConnection();
    }
}
