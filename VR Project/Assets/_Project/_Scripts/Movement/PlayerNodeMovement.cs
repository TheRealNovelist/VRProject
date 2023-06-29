using System;
using System.Collections;
using System.Collections.Generic;
using BNG;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerNodeMovement : MonoBehaviour
{
    [ReadOnly] public bool isEditorMode = false;
    
    [Header("Components")] 
    [SerializeField] private LineRenderer pointer;

    [Header("Setting")]
    [SerializeField] private MovementNode startingNode;
    [SerializeField] private float travelTime = 5f;
    [SerializeField] private LayerMask mask;

    [ReadOnly] public MovementNode currentNode;
    private MovementNode _nextNodeSelected;

    private ScreenFader _fader;
    private InputBridge _input;
    
    private bool _startedSearching;

    private bool _isGrabbing;
    private bool _isLeftThumbstick;

    public static event Action OnBeforeTeleport;
    public static event Action OnTeleport;
    public static event Action OnAfterTeleport;
    
    private void Awake()
    {
        _input = InputBridge.Instance;
        _fader = GetComponentInChildren<ScreenFader>();
        
        _fader.FadeInSpeed = travelTime;
        _fader.FadeOutSpeed = travelTime;
        
        pointer.gameObject.SetActive(false);
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
        isEditorMode = !InputBridge.Instance.HMDActive;
        _isLeftThumbstick = _input.GetInputAxisValue(InputAxis.LeftThumbStickAxis).y >= 0.75;
        
        _isGrabbing = _input.LeftGripDown || _input.RightGripDown || Input.GetKey(KeyCode.Space);
        
        if ((isEditorMode ? Input.GetKeyDown(KeyCode.W) : _isLeftThumbstick) && !_startedSearching)
        {
            _startedSearching = true;
            currentNode.SetConnectionsActive(true);
        }
        
        if (isEditorMode ? Input.GetKey(KeyCode.W) : _isLeftThumbstick)
        {
            SearchNode();
        }

        if ((isEditorMode ? Input.GetKeyUp(KeyCode.W) : !_isLeftThumbstick) && _startedSearching)
        {
            _startedSearching = false;
            currentNode.SetConnectionsActive(false);
            pointer.gameObject.SetActive(false);
            
            if (_nextNodeSelected != null)
            {
                _nextNodeSelected.OnDeselected();
                TeleportToNode(_nextNodeSelected);
            }
        }
    }
    
    public void TeleportToNode(MovementNode node, bool instant = false)
    {
        if (!node.CanTeleport())
            return;

        if (currentNode) 
            currentNode.TeleportOut();
        
        currentNode = node;
        
        OnBeforeTeleport?.Invoke();

        if (instant)
        {
            TeleportInstant();
            return;
        }
        
        StartCoroutine(TeleportRoutine());
    }

    private IEnumerator TeleportRoutine()
    {
        _fader.DoFadeIn();

        yield return new WaitForSeconds(travelTime / 2);

        TeleportInstant();
        
        if (currentNode.fadeOutOnTeleport)
        {
            _fader.DoFadeOut();
            yield return new WaitForSeconds(travelTime / 2);
        }
        
        OnAfterTeleport?.Invoke();
    }

    private void TeleportInstant()
    {
        currentNode.TeleportTo(transform);
        OnTeleport?.Invoke();
    }

    private void SearchNode()
    {
        pointer.SetPosition(0, pointer.transform.position);

        if (Physics.Raycast(pointer.transform.position,pointer.transform.forward, out RaycastHit hit,
                Mathf.Infinity, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            GameObject obj = hit.collider.gameObject;

            if (((1 << obj.layer) & mask) != 0)
            {
                pointer.gameObject.SetActive(true);
                pointer.SetPosition(1, hit.point);
            }
            else
            {
                pointer.gameObject.SetActive(false);
            }

            if (obj.TryGetComponent(SearchComponentMode.Any, out MovementNode node))
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

        if (_nextNodeSelected != null)
        {
            _nextNodeSelected.OnDeselected();
            _nextNodeSelected = null;
        }
    }
}
