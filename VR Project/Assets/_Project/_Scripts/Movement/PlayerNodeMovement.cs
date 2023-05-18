using System;
using System.Collections;
using System.Collections.Generic;
using BNG;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerNodeMovement : MonoBehaviour
{
    [Header("Components")] 
    [SerializeField] private Transform pointer;

    [Header("Setting")]
    [SerializeField] private MovementNode startingNode;
    [SerializeField] private int playerHeight = 2;
    [SerializeField] private float travelTime = 5f;
    [SerializeField] private LayerMask mask = Physics.DefaultRaycastLayers;

    private MovementNode _currentNode;
    private MovementNode _nextNodeSelected;

    private ScreenFader _fader;
    private InputBridge _input;
    
    private bool _startedSearching;

    private bool _isGrabbing;
    private bool _isLeftThumbstick;
    
    private void Awake()
    {
        _input = InputBridge.Instance;
        _fader = GetComponentInChildren<ScreenFader>();
        
        _fader.FadeInSpeed = travelTime;
        _fader.FadeOutSpeed = travelTime;
    }

    private void Start()
    {
        if (!startingNode)
        {
            Debug.Log("[PlayerNodeMovement] Please assign a starting node to the player");
            Debug.Break();
        }
        
        TeleportToNode(startingNode, true);
    }

    private void Update()
    { 
        _isLeftThumbstick = _input.GetInputAxisValue(InputAxis.LeftThumbStickAxis).y >= 0.75;

        _isGrabbing = _input.LeftGripDown || _input.RightGripDown || Input.GetKey(KeyCode.Space);

        if ((/*_isLeftThumbstick ||*/ Input.GetKeyDown(KeyCode.W)) && !_startedSearching)
        {
            _startedSearching = true;
            _currentNode.SetConnectionsActive(true);
        }
        
        if (/*_isLeftThumbstick ||*/ Input.GetKey(KeyCode.W))
        {
            SearchNode();
        }

        if ((/*!_isLeftThumbstick || */Input.GetKeyUp(KeyCode.W)) && _startedSearching)
        {
            _startedSearching = false;
            _currentNode.SetConnectionsActive(false);

            if (_nextNodeSelected != null)
            {
                _nextNodeSelected.OnDeselected();
                TeleportToNode(_nextNodeSelected);
            }
        }
    }
    
    private void TeleportToNode(MovementNode node, bool instant = false)
    {
        if (!node.CanTeleport())
            return;

        if (_currentNode) 
            _currentNode.TeleportOut();
        
        _currentNode = node;
        
        if (instant)
        {
            _currentNode.TeleportTo(transform, playerHeight);
            return;
        }
        
        StartCoroutine(InstantTeleport());
    }

    private IEnumerator InstantTeleport()
    {
        _fader.DoFadeIn();

        yield return new WaitForSeconds(travelTime / 2);

        _currentNode.TeleportTo(transform, out bool animate, playerHeight);
        
        if (animate)
        {
            _fader.DoFadeOut();
        }
    }

    private void SearchNode()
    {
        if (Physics.Raycast(pointer.transform.position,pointer.forward, out RaycastHit hit,
                Mathf.Infinity,mask, QueryTriggerInteraction.Ignore))
        {
            GameObject obj = hit.collider.gameObject;
            Debug.DrawLine(pointer.transform.position, hit.point, Color.green);
            
            if (obj.TryGetComponent(out MovementNode node))
            {
                if (node != _nextNodeSelected && _nextNodeSelected != null)
                {
                    _nextNodeSelected.OnDeselected();
                    _nextNodeSelected = null;
                }
                
                _nextNodeSelected = node;
                node.OnSelected(_isGrabbing);
                return;
            }
        }
        
        Debug.DrawRay(pointer.transform.position, pointer.forward, Color.red);

        if (_nextNodeSelected != null)
        {
            _nextNodeSelected.OnDeselected();
            _nextNodeSelected = null;
        }
    }
}
