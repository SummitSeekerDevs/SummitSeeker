using UnityEngine;
using Zenject;

public class PlayerMovementController : MonoBehaviour
{
    #region Vars
    // References
    [SerializeField]
    private Transform _spawnPoint;

    [SerializeField]
    private Transform _orientation;

    [SerializeField]
    private PlayerMovementConfig _playerMovementConfig;

    private Rigidbody _rb;

    private PlayerInputProvider _playerInputProvider;

    private PlayerSlopeHandler _playerSlopeHandler;

    private PlayerStateMachine _playerStateMachine;
    private PlayerJumpHandler _playerJumpHandler;
    private PlayerCrouchHandler _playerCrouchHandler;

    // Movement
    private Vector3 _moveDirection;
    private float _moveSpeed;

    // Ground check
    private bool _isGrounded;

    // Slope
    private bool _exitingSlope;

    #endregion

    [Inject]
    public void Construct(
        PlayerInputProvider playerInputProvider,
        PlayerSlopeHandler playerSlopeHandler,
        PlayerStateMachine playerStateMachine,
        PlayerJumpHandler playerJumpHandler,
        PlayerCrouchHandler playerCrouchHandler
    )
    {
        // Zenject injections
        _playerInputProvider = playerInputProvider;
        _playerSlopeHandler = playerSlopeHandler;
        _playerStateMachine = playerStateMachine;
        _playerJumpHandler = playerJumpHandler;
        _playerCrouchHandler = playerCrouchHandler;

        // Rigidbody
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;
    }

    private void OnEnable()
    {
        _playerCrouchHandler.OnGameObjectEnabled();
    }

    private void OnDisable()
    {
        _playerCrouchHandler.OnGameObjectDisabled();
    }

    private void Update()
    {
        // set isGrounded for this frame
        _isGrounded = IsGrounded();

        _playerJumpHandler.CheckJump(_playerInputProvider._jumpingIsPressed, _isGrounded);
        SpeedControl();
        _playerStateMachine.UpdateMovementState(
            _isGrounded,
            _playerInputProvider._crouchingIsPressed,
            _playerInputProvider._sprintingIsPressed
        );
        SetMovementSpeed();

        HandleDrag();
    }

    private void FixedUpdate()
    {
        MovePlayer(_playerInputProvider._moveInput.x, _playerInputProvider._moveInput.y);
    }

    #region Moving
    private void MovePlayer(float horizontalInput, float verticalInput)
    {
        // calculate movement direction
        _moveDirection =
            _orientation.forward * verticalInput + _orientation.right * horizontalInput;

        // on slope
        if (
            _playerSlopeHandler.OnSlope(
                transform,
                _playerMovementConfig.playerHeight,
                _playerMovementConfig.maxSlopeAngle
            ) && !_exitingSlope
        )
        {
            _rb.AddForce(
                _playerSlopeHandler.GetSlopeMoveDirection(_moveDirection) * _moveSpeed * 20f,
                ForceMode.Force
            );

            if (_rb.linearVelocity.y > 0)
                _rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }
        // on ground
        else if (_isGrounded)
            _rb.AddForce(_moveDirection.normalized * _moveSpeed * 10f, ForceMode.Force);
        // in air
        else if (!_isGrounded)
        {
            _rb.AddForce(
                _moveDirection.normalized * _moveSpeed * 10f * _playerMovementConfig.airMultiplier,
                ForceMode.Force
            );

            ResetUnderMap();
        }

        // turn gravity off while on slope
        _rb.useGravity = !_playerSlopeHandler.OnSlope(
            transform,
            _playerMovementConfig.playerHeight,
            _playerMovementConfig.maxSlopeAngle
        );
    }

    private void SetMovementSpeed()
    {
        switch (_playerStateMachine._currentState)
        {
            case PlayerStateMachine.MovementState.Walking:
                _moveSpeed = _playerMovementConfig.walkSpeed;
                break;
            case PlayerStateMachine.MovementState.Sprinting:
                _moveSpeed = _playerMovementConfig.sprintSpeed;
                break;
            case PlayerStateMachine.MovementState.Crouching:
                _moveSpeed = _playerMovementConfig.crouchSpeed;
                break;
            case PlayerStateMachine.MovementState.Air:
                break;
        }
    }
    #endregion

    #region Checks
    internal void ResetUnderMap()
    {
        if (_rb.position.y <= -15f)
        {
            _rb.position = _spawnPoint.position;
        }
    }

    private void SpeedControl()
    {
        // limiting speed on slope
        if (
            _playerSlopeHandler.OnSlope(
                transform,
                _playerMovementConfig.playerHeight,
                _playerMovementConfig.maxSlopeAngle
            ) && !_exitingSlope
        )
        {
            if (_rb.linearVelocity.magnitude > _moveSpeed)
                _rb.linearVelocity = _rb.linearVelocity.normalized * _moveSpeed;
        }
        // limiting speed on ground or in air
        else
        {
            Vector3 flatVel = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > _moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * _moveSpeed;
                _rb.linearVelocity = new Vector3(limitedVel.x, _rb.linearVelocity.y, limitedVel.z);
            }
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(
            transform.position,
            Vector3.down,
            _playerMovementConfig.playerHeight * 0.5f + 0.2f,
            _playerMovementConfig.whatIsGround
        );
    }

    private void HandleDrag()
    {
        if (IsGrounded())
            _rb.linearDamping = _playerMovementConfig.groundDrag;
        else
            _rb.linearDamping = 0;
    }
    #endregion
}

[CreateAssetMenu(menuName = "Configs/Player Movement Config")]
public class PlayerMovementConfig : ScriptableObject
{
    [Header("Movement")]
    public float walkSpeed;
    public float sprintSpeed;
    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;

    [Header("Ground Check")]
    public LayerMask whatIsGround;
    public float playerHeight;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
}

public class CrouchSignal
{
    public readonly bool _crouchKeyPressed;

    public CrouchSignal(bool crouchKeyPressed)
    {
        _crouchKeyPressed = crouchKeyPressed;
    }
}
