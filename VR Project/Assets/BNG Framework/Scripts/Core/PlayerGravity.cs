﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BNG {

    /// <summary>
    /// Apply Gravity to a CharacterController or RigidBody
    /// </summary>
    public class PlayerGravity : MonoBehaviour {

        [Tooltip("If true, will apply gravity to the CharacterController component, or RigidBody if no CC is present.")]

        public bool GravityEnabled = true;

        [Tooltip("Amount of Gravity to apply to the CharacterController or Rigidbody. Default is 'Physics.gravity'.")]
        public Vector3 Gravity = Physics.gravity;

        CharacterController characterController;
        SmoothLocomotion smoothLocomotion;

        Rigidbody playerRigidbody;

        private float _movementY;
        private Vector3 _initialGravityModifier;

        // Save us a null check in FixedUpdate
        private bool _validRigidBody = false;

        private void Awake()
        {
            PlayerNodeMovement.OnBeforeTeleport += BeforeTeleport;
            PlayerNodeMovement.OnAfterTeleport += AfterTeleport;
        }

        private void OnDestroy()
        {
            PlayerNodeMovement.OnBeforeTeleport -= BeforeTeleport;
            PlayerNodeMovement.OnAfterTeleport -= AfterTeleport;
        }

        void BeforeTeleport()
        {
            enabled = false;
        }

        void AfterTeleport()
        {
            enabled = true;
        }

        void Start() {
            characterController = GetComponent<CharacterController>();
            smoothLocomotion = GetComponentInChildren<SmoothLocomotion>();
            playerRigidbody = GetComponent<Rigidbody>();

            _validRigidBody = playerRigidbody != null;

            _initialGravityModifier = Gravity;
        }

        // Apply Gravity in LateUpdate to ensure it gets applied after any character movement is applied in Update
        void LateUpdate() {

            // Apply Gravity to Character Controller
            if (GravityEnabled && characterController != null && characterController.enabled) {
                _movementY += Gravity.y * Time.deltaTime;

                // Default to smooth locomotion
                if(smoothLocomotion) {
                    smoothLocomotion.MoveCharacter(new Vector3(0, _movementY, 0) * Time.deltaTime);
                }
                // Fallback to character controller
                else if(characterController) {
                    characterController.Move(new Vector3(0, _movementY, 0) * Time.deltaTime);
                }
                
                // Reset Y movement if we are grounded
                if (characterController.isGrounded) {
                    _movementY = 0;
                }
            }
        }

        public void ToggleGravity(bool gravityOn) {

            GravityEnabled = gravityOn;

            if (gravityOn) {
                Gravity = _initialGravityModifier;
            }
            else {
                Gravity = Vector3.zero;
            }
        }
    }
}

