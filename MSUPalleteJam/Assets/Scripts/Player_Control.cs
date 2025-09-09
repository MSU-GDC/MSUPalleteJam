using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Controller : MonoBehaviour
{
    private enum MovementState_e
    {
        IDLE,
        GROUNDED,
        AIRBORNE
    }


    [Header("References")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private InputActionAsset _controls;


    [Header("Movement Settings")]
    [SerializeField] private float _jumpHeight = 2.0f;
    [SerializeField] private float _gravityScale = 1.0f;
    [SerializeField] private float _moveSpeed = 2.0f;
    [SerializeField] private LayerMask _ignore;


    [Header("Ground Check Settings")]
    [SerializeField] private Vector2 _rectExtents = Vector2.one; 

    [Header("Debug")]
    [SerializeField] private Vector2 _movementDirection;
    [SerializeField] private bool _isJump;
    [SerializeField] private bool _isAirborne;
    [SerializeField] private float _jumpVelocity;


    private InputAction _moveAction;
    private InputAction _jumpAction;

    private void OnEnable()
    {
        _controls.FindActionMap("Player").Enable();
        _moveAction = _controls.FindActionMap("Player").FindAction("Move");
        _jumpAction = _controls.FindActionMap("Player").FindAction("Jump");
    }
    private void OnDisable()
    {
        _controls.FindActionMap("Player").Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _moveAction = _controls.FindActionMap("Player").FindAction("Move");
        _jumpAction = _controls.FindActionMap("Player").FindAction("Jump");
        _movementDirection = Vector2.zero;
        _jumpVelocity = Mathf.Sqrt(2f * -Physics.gravity.y * _gravityScale * _jumpHeight);

    }

    // Update is called once per frame
    void Update()
    {
        QueryPlayerInput();
        CheckIsAirborne();
    }


    private void FixedUpdate()
    {

    }

    private void QueryPlayerInput()
    {
        // Get the input vector from the player, clamp the value so it can only be -1, 0, or 1, then feed the value back into the global direction vector
        Vector2 inputVector = _moveAction.ReadValue<Vector2>();

        float vAxis = inputVector.y;
        float hAxis = inputVector.x;

        vAxis = Mathf.Ceil(Mathf.Abs(vAxis)) * Mathf.Sign(vAxis);
        hAxis = Mathf.Ceil(Mathf.Abs(hAxis)) * Mathf.Sign(hAxis);



        _movementDirection = new Vector2(hAxis, vAxis);

        _isJump = _jumpAction.IsPressed(); 
        
    }


    private void MovePlayer()
    {

    }

    // move the player using instantaneous velocity changes
    private void MovePlayerGrounded()
    {
        Vector2 velocityVector = new Vector2(_movementDirection.x,0f);
        
        velocityVector = Vector3.Normalize(velocityVector) * _moveSpeed;


        float yComponent = 0f;

        if (_isJump && !_isAirborne) yComponent = _jumpVelocity;


        velocityVector = velocityVector + ((Vector2)transform.up * yComponent);

        _rb.linearVelocity = velocityVector;
    }


    private bool IsInput()
    {
        return _movementDirection.x != 0.0f || _isJump; 
    }

    private void CheckIsAirborne()
    {

    }

    private void OnDrawGizmos()
    {
        
    }
}
