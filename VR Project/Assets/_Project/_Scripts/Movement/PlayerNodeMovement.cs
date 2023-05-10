using System;
using System.Collections;
using System.Collections.Generic;
using BNG;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerNodeMovement : MonoBehaviour
{
    [Header("Initialization")]
    [SerializeField] private MovementNode startingNode;

    [Header("Components")] 
    [SerializeField] private Transform pointer;

    [Header("Setting")] 
    [SerializeField] private bool instantTravel = true;
    [SerializeField] private float travelTime = 5f;

    private MovementNode _currentNode;
    private MovementNode _nextNodeSelected;

    private ScreenFader _fader;
    private InputBridge _input;
    
    private bool _startedSearching;
    
    private void Awake()
    {
        _input = InputBridge.Instance;
        _fader = GetComponentInChildren<ScreenFader>();
        
        _fader.FadeInSpeed = travelTime;
        _fader.FadeOutSpeed = travelTime;
        
        TeleportToNode(startingNode, true);
    }

    private void Update()
    {
        bool leftThumbstick = _input.GetInputAxisValue(InputAxis.LeftThumbStickAxis).y >= 0.75;
        
        if ((leftThumbstick || Input.GetKeyDown(KeyCode.W)) && !_startedSearching)
        {
            _startedSearching = true;
            _currentNode.SetConnectionsActive(true);
        }
        
        if (leftThumbstick || Input.GetKey(KeyCode.W))
        {
            SearchNode();
        }

        if ((!leftThumbstick || Input.GetKeyUp(KeyCode.W)) && _startedSearching)
        {
            _startedSearching = false;
            _currentNode.SetConnectionsActive(false);
            
            if (_nextNodeSelected != null)
                TeleportToNode(_nextNodeSelected);
        }
    }

    [Button]
    private void TeleportToNode(MovementNode node, bool instant = false)
    {
        if (_currentNode != null)
            _currentNode.OnTeleportOut();
        
        _currentNode = node;

        Vector3 destination = _currentNode.GetPosition(transform);

        if (instant)
        {
            transform.position = destination;
            _currentNode.OnTeleportTo();
            return;
        }
        
        if (instantTravel)
        {
            StartCoroutine(InstantTeleport(destination));
        }
        else
        {
            BlinkTeleport(destination);
        }
        
        _currentNode.OnTeleportTo();
    }

    private void BlinkTeleport(Vector3 destination)
    {
        transform.DOMove(destination, travelTime);
    }

    private IEnumerator InstantTeleport(Vector3 destination)
    {
        _fader.DoFadeIn();

        yield return new WaitForSeconds(travelTime / 2);
        
        transform.position = destination;
        _fader.DoFadeOut();
    }

    private void SearchNode()
    {
        if (Physics.Raycast(pointer.transform.position, pointer.forward, out RaycastHit hit, Mathf.Infinity))
        {
            Debug.DrawLine(pointer.transform.position, pointer.forward * 100f, Color.green);
            GameObject obj = hit.collider.gameObject;
            Debug.Log(obj.name);
            if (obj.TryGetComponent(out MovementNode node))
            {
                if (node != _nextNodeSelected && _nextNodeSelected != null)
                {
                    _nextNodeSelected.OnDeselected();
                    _nextNodeSelected = null;
                }
                
                _nextNodeSelected = node;
                node.OnSelected();
            }
            else
            {
                if (_nextNodeSelected == null) return;
                
                _nextNodeSelected.OnDeselected();
                _nextNodeSelected = null;
            }
        }
    }
}
