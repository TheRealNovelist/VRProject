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
    [Header("Components")] 
    [SerializeField] private GameObject arrow;
    
    [Header("Connections")]
    [SerializeField] private bool allowTeleportToAllNode;
    [SerializeField, HideIf("allowTeleportToAllNode")] private List<MovementNode> connections;
    
    [Header("Mirror Connection")]
    [SerializeField] private MovementNode mirrorNode;
    public Level level;
    
    public MovementNode MirrorNode => mirrorNode;
    
    [Header("Setting")]
    [SerializeField] private GameObject highlight;
    [SerializeField] private bool disallowItemOnHand;
    [SerializeField] private bool moveOnTeleport = true;
    [SerializeField] private bool rotateOnTeleport = true;
    public bool fadeOutOnTeleport = true;
    
    [Header("Teleport Event")]
    [SerializeField] private bool useTeleportEvent;
    [SerializeField, ShowIf("useTeleportEvent")] private UnityEvent OnTeleportTo;
    [SerializeField, ShowIf("useTeleportEvent")] private UnityEvent OnTeleportOut;

    [Header("Debug")] 
    [SerializeField] private bool drawReachableZone = true;
    
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
            node.AddNodeCallback(this);
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
    
    public void Connect(MovementNode node)
    {
        if (connections.Contains(node) || node == this)
            return;

        connections.Add(node);
        node.AddNodeCallback(this);
    }

    private void ConnectMirror(MovementNode node)
    {
        node.mirrorNode = this;
    }
    

    //Callback to all nodes that this node can be teleported to
    private void AddNodeCallback(MovementNode node)
    {
        if (_fromNodes.Contains(node))
            return;
        
        _fromNodes.Add(node);
    }
    
    public bool CanTeleport() => _allowTeleport;

    #region Selection

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

    public void OnDeselected()
    {
        if (highlight) highlight.SetActive(false);
    }

    #endregion

    #region Teleport

    public void TeleportOut()
    {
        if (useTeleportEvent) OnTeleportOut.Invoke();
        
        SetNodeActive(false);
    }

    public void TeleportTo(Transform player)
    {
        if (useTeleportEvent) OnTeleportTo.Invoke();
        
        if (!moveOnTeleport) return;
        
        player.position = GetPosition();
        if (rotateOnTeleport)
        {
            player.rotation = Quaternion.LookRotation(transform.forward);
        }
    }
    #endregion

    private Vector3 GetPosition()
    {
        return transform.position;
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
    
    #region Debug

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

    private void OnDrawGizmos()
    {
        if (!drawReachableZone) return;
        
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), new Vector3(2f, 2, 2f));
    }

    private void OnDrawGizmosSelected()
    {
        DrawConnection();
    }

    #endregion

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (arrow)
            arrow.SetActive(rotateOnTeleport);

        if (mirrorNode)
        {
            if (mirrorNode == this)
            {
                mirrorNode = null;    
                return;
            }
            
            ConnectMirror(mirrorNode);
        }
    }
#endif
}
