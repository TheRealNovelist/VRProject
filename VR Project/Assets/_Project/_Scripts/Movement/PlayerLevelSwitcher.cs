using System;
using System.Collections;
using System.Collections.Generic;
using BNG;
using UnityEngine;

[RequireComponent(typeof(PlayerNodeMovement))]
public class PlayerLevelSwitcher : MonoBehaviour
{
    private bool _isGrabbing;
    private InputBridge _input;

    private PlayerNodeMovement _movement => GetComponent<PlayerNodeMovement>();

    private void Awake()
    {
        _input = InputBridge.Instance;
    }

    private void OnEnable()
    {
        PlayerNodeMovement.OnTeleport += TryChangeLevel;
    }

    private void OnDisable()
    {
        PlayerNodeMovement.OnTeleport -= TryChangeLevel;
    }
    
    private void Update()
    {
        _isGrabbing = _input.LeftGripDown || _input.RightGripDown || Input.GetKey(KeyCode.Space);
        
        if (_movement.isEditorMode ? Input.GetKeyDown(KeyCode.E) : _input.XButtonDown)
        {
            Switch();    
        }
    }

    private void Switch()
    {
        MovementNode mirrorNode = _movement.currentNode.MirrorNode;
        
        if (_isGrabbing)
            return;

        if (!mirrorNode)
        {
            Debug.Log("[PlayerLevelSwitcher] Unable to switch to mirror node");
            return;
        }

        _movement.TeleportToNode(mirrorNode);
    }

    private void TryChangeLevel()
    {
        if (LevelController.CurrentLevel != _movement.currentNode.level)
            LevelController.SwitchLevel(_movement.currentNode.level, nameof(PlayerLevelSwitcher));
    }
}
