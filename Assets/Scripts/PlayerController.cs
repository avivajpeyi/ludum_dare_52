using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Farmer))]
public class PlayerController : MonoBehaviour
{
    private Vector2 _moveInput;
    private bool _interactInput;
    private Farmer farmer;

    private void Start()
    {
        farmer = GetComponent<Farmer>();
    }


    private void Update()
    {
        if (farmer.IsDead())
            return;

        farmer.UpdateMovement(_moveInput);
        

        if (_interactInput)
        {
            farmer.Interact();
            _interactInput = false;
        }
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _interactInput = true;
        }
    }
}