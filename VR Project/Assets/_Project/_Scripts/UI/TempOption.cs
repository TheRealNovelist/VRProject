using System;
using System.Collections;
using System.Collections.Generic;
using BNG;
using UnityEngine;

public class TempOption : MonoBehaviour
{
    [SerializeField] private LocomotionManager _manager;
    [SerializeField] private PlayerRotation _rotation;

    public void Start()
    {
        _manager = FindObjectOfType<LocomotionManager>();
        _rotation = FindObjectOfType<PlayerRotation>();
    }

    public void SetSmoothLocomotion(bool isSmooth)
    {
        _manager.ChangeLocomotion(isSmooth ? LocomotionType.SmoothLocomotion : LocomotionType.Teleport, _manager.LoadLocomotionFromPrefs);
    }
    
    public void SetSnapTurn(bool isSnap)
    {
        _rotation.RotationType = isSnap ? RotationMechanic.Snap : RotationMechanic.Smooth;
    }
}
