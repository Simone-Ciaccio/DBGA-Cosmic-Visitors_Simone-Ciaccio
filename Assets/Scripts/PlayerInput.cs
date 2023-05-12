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
        Vector2 ScreenTopRightInWorld = cam.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        Vector2 ScreenBottomLeftInWorld = cam.ScreenToWorldPoint(new Vector2(0, 0));
        boundRight = ScreenTopRightInWorld.x;
        boundLeft = ScreenBottomLeftInWorld.x;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Sprite sprite = spriteRenderer.sprite;
        halfSpriteSize = new Vector2(spriteRenderer.bounds.size.x / 2, spriteRenderer.bounds.size.y / 2);

        MoveAction.performed += OnMovePerformed;
        MoveAction.canceled += OnMoveCancelled;
    }

    private void Update()
    {
        transform.position += Vector3.right * input * MoveSpeed * Time.deltaTime;

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
