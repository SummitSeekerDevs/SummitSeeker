using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

[assembly: InternalsVisibleTo("Tests")]

public class PlayerMovementVid : MonoBehaviour
{
    private PlayerInput_Actions _playerInputActions;

    public Transform spawnPoint;

    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    internal bool _sprintingIsPressed { get; private set; }

    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;
    internal bool _jumpingIsPressed { get; private set; }

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;
    internal bool _crouchingIsPressed { get; private set; }

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    public Transform orientation;

    internal Vector2 moveInput { get; private set; }

    Vector3 moveDirection;

    Rigidbody rb;

    public MovementState state;

    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        air,
    }

    private void Awake()
    {
        setPlayerInputActions();
    }

    private void setPlayerInputActions()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager.Instance is null. Ensure GameManager exists in the scene.");
            return;
        }
        _playerInputActions = GameManager.Instance.InputActions;
        if (_playerInputActions == null)
        {
            Debug.LogError("PlayerInput_Actions not initialized in GameManager");
        }
    }

    private void OnEnable()
    {
        // Movin
        _playerInputActions.Player.Move.performed += OnMove;
        _playerInputActions.Player.Move.canceled += OnMove;

        // Jumping
        _playerInputActions.Player.Jump.started += OnJump;
        _playerInputActions.Player.Jump.canceled += OnJumpCanceled;

        // Crouching
        _playerInputActions.Player.Crouch.started += OnCrouch;
        _playerInputActions.Player.Crouch.canceled += OnCrouchCanceled;

        // Sprinting
        _playerInputActions.Player.Sprint.started += OnSprint;
        _playerInputActions.Player.Sprint.canceled += OnSprintCanceled;
    }

    private void OnDisable()
    {
        // Moving
        _playerInputActions.Player.Move.performed -= OnMove;
        _playerInputActions.Player.Move.canceled -= OnMove;

        // Jumping
        _playerInputActions.Player.Jump.started -= OnJump;
        _playerInputActions.Player.Jump.canceled -= OnJumpCanceled;

        // Crouching
        _playerInputActions.Player.Crouch.started -= OnCrouch;
        _playerInputActions.Player.Crouch.canceled -= OnCrouchCanceled;

        // Sprinting
        _playerInputActions.Player.Sprint.started -= OnSprint;
        _playerInputActions.Player.Sprint.canceled -= OnSprintCanceled;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        startYScale = transform.localScale.y;
    }

    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(
            transform.position,
            Vector3.down,
            playerHeight * 0.5f + 0.2f,
            whatIsGround
        );

        MyInput();
        SpeedControl();
        StateHandler();

        // handle drag
        if (grounded)
            rb.linearDamping = groundDrag;
        else
            rb.linearDamping = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer(moveInput.x, moveInput.y);
    }

    #region Input

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    // Jumping
    private void OnJump(InputAction.CallbackContext context)
    {
        _jumpingIsPressed = true;
    }

    private void OnJumpCanceled(InputAction.CallbackContext context)
    {
        _jumpingIsPressed = false;
    }

    // Crouching
    private void OnCrouch(InputAction.CallbackContext context)
    {
        _crouchingIsPressed = true;
    }

    private void OnCrouchCanceled(InputAction.CallbackContext context)
    {
        ResetCrouch();
        _crouchingIsPressed = false;
    }

    // Sprinting
    private void OnSprint(InputAction.CallbackContext context)
    {
        _sprintingIsPressed = true;
    }

    private void OnSprintCanceled(InputAction.CallbackContext context)
    {
        _sprintingIsPressed = false;
    }

    #endregion

    private void MyInput()
    {
        // when to jump
        if (_jumpingIsPressed)
        {
            Jump();
        }
        // when to crouch
        if (_crouchingIsPressed)
        {
            Crouch();
        }
    }

    private void StateHandler()
    {
        // Mode - Crouching
        if (_crouchingIsPressed)
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }
        // Mode - Sprinting
        else if (grounded && _sprintingIsPressed)
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }
        // Mode - Walking
        else if (grounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }
        // Mode - Air
        else
        {
            state = MovementState.air;
        }
    }

    internal void MovePlayer(float horizontalInput, float verticalInput)
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            if (rb.linearVelocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }
        // on ground
        else if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        // in air
        else if (!grounded)
        {
            rb.AddForce(
                moveDirection.normalized * moveSpeed * 10f * airMultiplier,
                ForceMode.Force
            );

            ResetUnderMap();
        }

        // turn gravity off while on slope
        rb.useGravity = !OnSlope();
    }

    internal void ResetUnderMap()
    {
        if (rb.position.y <= -15f)
        {
            rb.position = spawnPoint.position;
        }
    }

    private void SpeedControl()
    {
        // limiting speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if (rb.linearVelocity.magnitude > moveSpeed)
                rb.linearVelocity = rb.linearVelocity.normalized * moveSpeed;
        }
        // limiting speed on ground or in air
        else
        {
            Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
            }
        }
    }

    internal void Jump()
    {
        if (readyToJump && grounded)
        {
            readyToJump = false;

            exitingSlope = true;

            // reset y velocity
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

            // jump cooldown
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }

    internal void Crouch()
    {
        transform.localScale = new Vector3(
            transform.localScale.x,
            crouchYScale,
            transform.localScale.z
        );
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
    }

    internal void ResetCrouch()
    {
        transform.localScale = new Vector3(
            transform.localScale.x,
            startYScale,
            transform.localScale.z
        );
    }

    private bool OnSlope()
    {
        if (
            Physics.Raycast(
                transform.position,
                Vector3.down,
                out slopeHit,
                playerHeight * 0.5f + 0.3f
            )
        )
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
}
