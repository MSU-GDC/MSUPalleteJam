using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Controller : MonoBehaviour
{
    //Someone might wanna clean up the Headers and SerializeFields later lmao (not it!!) - DC

    [Header("References")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private InputActionAsset _controls;

    [Header("Movement Settings")]
    [SerializeField] private float _jumpHeight = 2.0f;
    [SerializeField] private float _gravityScale = 1.0f;
    [SerializeField] private float _moveSpeed = 6.0f;
    [SerializeField] private float _acceleration = 20.0f;
    [SerializeField] private float _deceleration = 30.0f;

    [Header("Ground Check Settings")]
    [SerializeField] private Vector2 _rectExtents = Vector2.one;
    [SerializeField] private Transform groundCast;
    [SerializeField] private float rcDist = 0.1f;
    [SerializeField] private LayerMask groundMask;

    [Header("Jump Assist")] // Right now, this only includes coyote time - DC
    [SerializeField] private float coyoteTime = 0.1f; // seconds

    [Header("Debug")]
    [SerializeField] private Vector2 _movementDirection;
    [SerializeField] private bool _isJump;
    [SerializeField] private bool _isGrounded;
    [SerializeField] private float _jumpVelocity;

    private InputAction _moveAction;
    private InputAction _jumpAction;
    private bool _jumpQueued = false;
    private float coyoteTimer = 0f;

    private void OnEnable()
    {
        _controls.FindActionMap("Player").Enable();
        _moveAction = _controls.FindActionMap("Player").FindAction("Move");
        _jumpAction = _controls.FindActionMap("Player").FindAction("Jump");
        _jumpAction.started += OnJumpStarted;
    }
    private void OnDisable()
    {
        _controls.FindActionMap("Player").Disable();
        _jumpAction.started -= OnJumpStarted;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _moveAction = _controls.FindActionMap("Player").FindAction("Move");
        _jumpAction = _controls.FindActionMap("Player").FindAction("Jump");
        _movementDirection = Vector2.zero;

        _rb.gravityScale = 0f; // Disable built-in gravity
    }

    // Update is called once per frame
    void Update()
    {
        QueryPlayerInput();
        CheckGrounded();

        // Update coyote timer
        if (_isGrounded)
            coyoteTimer = coyoteTime;
        else
            coyoteTimer -= Time.deltaTime;

        // Sets Jump Velocity based on jump height and gravity scale. In update so that it can be changed in real-time for testing/mechanics. -DC

        float gravity = -9.81f * _gravityScale;
        _jumpVelocity = Mathf.Sqrt(2f * Mathf.Abs(gravity) * _jumpHeight);
    
    }

    private void FixedUpdate()
    {
        ApplyCustomGravity();
        MovePlayer();
    }

    private void QueryPlayerInput() 
    {
        //John, I tweaked with the player input a bit to add a deadzone and log warnings for nonzero inputs cause we were getting some weird behavior
        // regarding the player moving slightly on its own sometimes.
        // It's totally still happening, but at least now we can see when it happens in the logs.
        // Might wanna take a look at it if you have time. -DC

        // 

        Vector2 inputVector = _moveAction.ReadValue<Vector2>();

        // Clamp small values to zero (deadzone)
        if (Mathf.Abs(inputVector.x) < 0.2f)
            inputVector.x = 0f;
        if (Mathf.Abs(inputVector.y) < 0.2f)
            inputVector.y = 0f;

        // If the input is still not zero, log a warning for debugging
        if (inputVector.x != 0f)
            Debug.LogWarning($"Nonzero X input detected: {inputVector.x}");

        _movementDirection = new Vector2(inputVector.x, 0f);
    }

    private void OnJumpStarted(InputAction.CallbackContext ctx)
    {
        // Allow jump if grounded or within coyote time
        if (_isGrounded || coyoteTimer > 0f)
            _jumpQueued = true;
    }

    private void MovePlayer()
    {
        // Horizontal movement with acceleration/deceleration
        float targetSpeed = _movementDirection.x * _moveSpeed;
        float speedDiff = targetSpeed - _rb.linearVelocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? _acceleration : _deceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, 0.9f) * Mathf.Sign(speedDiff);

        _rb.AddForce(new Vector2(movement, 0f));

        // Jumping
        if (_jumpQueued && (coyoteTimer > 0f))
        {
            Vector2 v = _rb.linearVelocity;
            v.y = _jumpVelocity;
            _rb.linearVelocity = v;
            _jumpQueued = false;
            coyoteTimer = 0f; // Prevent double jump in coyote window
        }
    }

    private void CheckGrounded()
    {
        //Here's the BoxCast ground check, it works pretty well so far, but could totally be tweaked. -DC

        RaycastHit2D boxResult = Physics2D.BoxCast(gameObject.transform.position, new Vector2(2,1), 0f, Vector2.down, rcDist, groundMask);
        _isGrounded = boxResult.collider != null;
    }

    private void ApplyCustomGravity() //Heres the shitty custom gravity function I made, it does pretty much what it should,
                                      //but I know you wanted to use a real math function for it, so feel free to replace it. -DC
                                      //
                                      // Changed it so the player accelerates due to gravity instead of just applying a raw force - JF
    {
        if (!_isGrounded)
        {
            float gravity = -9.81f * _gravityScale;

            _rb.linearVelocityY += gravity * Time.fixedDeltaTime; 
        }
    }

    private void OnDrawGizmos()
    {
        // lets make it a wire cube so its easier to pick out against the background
        Gizmos.color = Color.green;
        if (groundCast != null)
            Gizmos.DrawWireCube(groundCast.position + Vector3.down * rcDist, new Vector2(2, 1));
    }
}
