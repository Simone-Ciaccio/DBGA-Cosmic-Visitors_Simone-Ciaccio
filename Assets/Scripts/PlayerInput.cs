using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public float MoveSpeed = 5;
    public InputAction MoveAction;

    private Camera cam;
    private float boundRight;
    private float boundLeft;

    private Vector2 halfSpriteSize;

    private float input;

    private void Awake()
    {
        cam = Camera.main;

        boundRight = Helper.GetScreenBoundRight(cam);
        boundLeft = Helper.GetScreenBoundLeft(cam);

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Sprite sprite = spriteRenderer.sprite;
        halfSpriteSize = new Vector2(spriteRenderer.bounds.size.x / 2, spriteRenderer.bounds.size.y / 2);

        MoveAction.performed += OnMovePerformed;
        MoveAction.canceled += OnMoveCancelled;
    }

    private void Update()
    {
        transform.position += input * MoveSpeed * Time.deltaTime * Vector3.right;

        if (transform.position.x > boundRight - halfSpriteSize.x)
        {
            transform.position = new Vector3 (boundRight - halfSpriteSize.x, transform.position.y, transform.position.z) ;
        }
        else if (transform.position.x < boundLeft + halfSpriteSize.x)
        {
            transform.position = new Vector3 (boundLeft + halfSpriteSize.x, transform.position.y, transform.position.z);
        }
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
