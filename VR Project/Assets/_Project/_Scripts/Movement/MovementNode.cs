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
    public bool isHidden;
    [SerializeField] private bool allowTeleportToAllNode;
    [SerializeField, HideIf("allowTeleportToAllNode")] private List<MovementNode> connections;
    
    [Header("Mirror Connection")]
    [SerializeField] private MovementNode mirrorNode;
    public Level level;
    
    public MovementNode MirrorNode => mirrorNode;
    
    private IMovementNodeResponse[] SelectionResponses => GetComponents<IMovementNodeResponse>();
    private bool HasResponse => SelectionResponses.Length > 0;
    
    [Header("Setting")]
    [SerializeField] private bool disallowItemOnHand;
    [SerializeField] private bool moveOnTeleport = true;
    [SerializeField] private bool rotateOnTeleport = true;
    public bool fadeOutOnTeleport = true;
    
    [Header("Debug")] 
    [SerializeField] private bool drawReachableZone = true;
    
    [TitleGroup("Teleport Event"), SerializeField] private UnityEvent OnTeleportTo;
    [TitleGroup("Teleport Event"), SerializeField] private UnityEvent OnTeleportOut;
    
    private bool _allowTeleport = true;
    private bool _currentlyActive = false;

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
    }

    private void FindAllConnection()
    {
        connections = new List<MovementNode>(FindObjectsOfType<MovementNode>(true).Where(node => node != this && node.level == level));
    }

    private void Start()
    {
        OnDeselected();
        gameObject.SetActive(false);
        if (HasResponse)
        {
            foreach (var response in SelectionResponses)
            {
                response.SetActive(false);
            }
        }
    }
    
    public void Connect(MovementNode node)
    {
        if (connections.Contains(node) || node == this)
            return;

        connections.Add(node);
        node.AddNodeCallback(this);
    }

    public void Disconnect(MovementNode node)
    {
        if (!connections.Contains(node) || node == this)
            return;

        connections.Remove(node);
        node.RemoveNodeCallback(this);
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

    private void RemoveNodeCallback(MovementNode node)
    {
        if (!_fromNodes.Contains(node)) return;

        _fromNodes.Remove(node);
    }
    
    public bool CanTeleport() => _allowTeleport;

    #region Selection

    public void OnSelected(bool itemOnHand)
    {
        if (HasResponse)
        {
            foreach (var response in SelectionResponses)
            {
                response.Selected(!itemOnHand);
            }
        }

        if (!disallowItemOnHand) return;

        _allowTeleport = !itemOnHand;
    }

    public void OnDeselected()
    {
        if (HasResponse)
        {
            foreach (var response in SelectionResponses)
            {
                response.Deselected();
            }
        }
    }

    #endregion

    #region Teleport

    public void TeleportOut()
    {
        OnTeleportOut.Invoke();
        
        SetNodeActive(false);
    }

    public void TeleportTo(Transform player)
    {
        OnTeleportTo.Invoke();
        
        if (!moveOnTeleport) return;
        
        player.position = transform.position;
        if (rotateOnTeleport)
        {
            player.rotation = Quaternion.LookRotation(transform.forward);
        }
    }
    #endregion
    
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
        _currentlyActive = isActive;
        
        if (isHidden) return;

        if (HasResponse)
        {
            foreach (var response in SelectionResponses)
            {
                response.SetActive(isActive);
            }
        }
        
        gameObject.SetActive(isActive);
    }

    public void SetHidden(bool isObjectHidden)
    {
        isHidden = isObjectHidden;
        
        //If the object is currently active, handle activity
        if(_currentlyActive)
            gameObject.SetActive(!isObjectHidden);
    }
    
    #region Debug

    public void DrawConnection()
    {
        if (connections.Count <= 0)
            return;
        
        foreach (MovementNode connection in connections)
        {
            if (connection == null) return;
            
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
