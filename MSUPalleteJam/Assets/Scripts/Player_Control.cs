using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Controller : MonoBehaviour
{
    public Rigidbody2D rb;
    public CapsuleCollider2D hitbox;
    public float speed = 2f;
    public bool airborne;
    public float gravity;
    public float jump_speed = 5f;
    InputAction moveAction;
    InputAction jumpAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("move");
        jumpAction = InputSystem.actions.FindAction("jump");

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();

        rb.AddForceX(moveValue.x);

        if (jumpAction.WasPerformedThisFrame())
        {
            airborne = false;
            rb.AddForceY(jump_speed);
        }
    }
}
