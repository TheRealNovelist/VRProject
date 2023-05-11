using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[SelectionBase]
[RequireComponent(typeof(CapsuleCollider))]
public class MovementNode : MonoBehaviour
{
    [SerializeField] private List<MovementNode> connections;
    [SerializeField] private GameObject highlight;
    
    [Header("Setting")]
    [SerializeField] private bool usePlayerHeight = true;
    [SerializeField] private bool disallowItemOnHand;
    [SerializeField] private bool moveOnTeleport = true;
    [SerializeField] private bool fadeOutOnTeleport = true;
    
    [Header("Teleport Event")]
    [SerializeField] private bool useTeleportEvent;
    [SerializeField, ShowIf("useTeleportEvent")] private UnityEvent OnTeleportTo;
    
    private List<Renderer> highlightRenderers;
    private bool _allowTeleport = true;

    private void Awake()
    {
        OnDeselected();
        SetNodeActive(false);

        highlightRenderers = highlight.GetComponentsInChildren<Renderer>().ToList();
        
        foreach (var highlightRenderer in highlightRenderers)
        {
            highlightRenderer.material.color = Color.green;
        }
    }

    private void Start()
    {
        
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

    public void Teleport(Transform player)
    {
        player.position = GetPosition(player);
    }
    
    public void Teleport(Transform player, out bool animate)
    {
        if (useTeleportEvent) OnTeleportTo.Invoke();
        if (moveOnTeleport) player.position = GetPosition(player);
        animate = fadeOutOnTeleport;
    }

    public Vector3 GetPosition(Transform moveTarget)
    {
        return new Vector3(
            transform.position.x, 
            usePlayerHeight ? moveTarget.position.y : transform.position.y, 
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

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
