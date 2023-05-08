using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public float MoveSpeed = 5;
    public InputAction MoveAction;

    private float input;

    private void Awake()
    {
        MoveAction.performed += OnMovePerformed;
        MoveAction.canceled += OnMoveCancelled;
    }

    private void Update()
    {
        transform.position += Vector3.right * input * MoveSpeed * Time.deltaTime;
    }

    private void OnEnable()
    {
        MoveAction.Enable();
    }

    private void OnDisable()
    {
        MoveAction.Disable();
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        input = context.ReadValue<float>();
    }

    private void OnMoveCancelled(InputAction.CallbackContext context)
    {
        input = 0;
    }
}
