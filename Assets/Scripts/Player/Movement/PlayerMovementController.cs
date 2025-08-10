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
    public PlayerMovementConfig _playerMovementConfig { get; private set; }

    private Rigidbody _rb;

    private PlayerInputProvider _playerInputProvider;

    private PlayerStateMachine _playerStateMachine;
    private PlayerJumpHandler _playerJumpHandler;
    private PlayerCrouchHandler _playerCrouchHandler;
    private PlayerPhysicsHandler _playerPhysicsHandler;
    public PlayerSlopeHandler _playerSlopeHandler { get; private set; }

    // Movement
    private Vector3 _moveDirection;
    public float _moveSpeed { get; private set; }

    // Ground check
    public bool _onGround { get; private set; }
    public bool _onSlope { get; private set; }
    public bool _exitingSlope { get; private set; }
    public bool _readyToJump { get; private set; } = true;

    private MovementStateMachine _movementStateMachine;

    #endregion

    [Inject]
    public void Construct(
        PlayerInputProvider playerInputProvider,
        PlayerStateMachine playerStateMachine,
        PlayerJumpHandler playerJumpHandler,
        PlayerCrouchHandler playerCrouchHandler,
        PlayerPhysicsHandler playerPhysicsHandler,
        PlayerSlopeHandler playerSlopeHandler,
        PlayerMovementConfig playerMovementConfig,
        DiContainer diContainer
    )
    {
        // Zenject injections
        _playerInputProvider = playerInputProvider;
        _playerStateMachine = playerStateMachine;
        _playerJumpHandler = playerJumpHandler;
        _playerCrouchHandler = playerCrouchHandler;
        _playerPhysicsHandler = playerPhysicsHandler;
        _playerSlopeHandler = playerSlopeHandler;
        _playerMovementConfig = playerMovementConfig;

        // Rigidbody
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;

        _movementStateMachine = new MovementStateMachine();
        diContainer.BindInstance(this);
        diContainer.QueueForInject(_movementStateMachine);
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
        _onGround = _playerPhysicsHandler.IsGrounded();

        // _playerJumpHandler.CheckJump(_playerInputProvider._jumpingIsPressed, _onGround);
        SpeedControl(_moveSpeed);

        // _playerStateMachine.UpdateMovementState(
        //     _onGround,
        //     _playerInputProvider._crouchingIsPressed,
        //     _playerInputProvider._sprintingIsPressed
        // );

        // SetMovementSpeed();



        _movementStateMachine.Update();

        _playerPhysicsHandler.HandleDrag(_onGround);
    }

    private void FixedUpdate()
    {
        _onSlope = _playerSlopeHandler.OnSlope(
            transform,
            _playerMovementConfig.playerHeight,
            _playerMovementConfig.maxSlopeAngle
        );

        // calculate movement direction
        _moveDirection =
            _orientation.forward * _playerInputProvider._moveInput.y
            + _orientation.right * _playerInputProvider._moveInput.x;

        // _playerPhysicsHandler.MovePlayer(_moveDirection, _moveSpeed, _onGround);
        _movementStateMachine.FixedUpdate(_moveDirection);

        if (!_onGround)
            _playerPhysicsHandler.ResetUnderMap(_spawnPoint.position);
    }

    public void SpeedControl(float moveSpeed)
    {
        // limiting speed on slope
        if (_onSlope && !_exitingSlope)
        {
            if (_rb.linearVelocity.magnitude > moveSpeed)
                _rb.linearVelocity = _rb.linearVelocity.normalized * moveSpeed;
        }
        // limiting speed on ground or in air
        else
        {
            Vector3 flatVel = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                _rb.linearVelocity = new Vector3(limitedVel.x, _rb.linearVelocity.y, limitedVel.z);
            }
        }
    }

    public void ToggleGravity(bool on)
    {
        _rb.useGravity = on;
    }

    #region Moving

    public void SetMovementSpeed(float newSpeed)
    {
        _moveSpeed = newSpeed;
    }

    public void SetReadyToJump(bool readyToJump)
    {
        _readyToJump = readyToJump;
    }

    public void SetExitingSlope(bool exitingSlope)
    {
        _exitingSlope = exitingSlope;
    }

    public void SetLinearVelocity(Vector3 newVelocity)
    {
        _rb.linearVelocity = newVelocity;
    }

    public void AddMovingForce(Vector3 force, ForceMode mode = ForceMode.Force)
    {
        _rb.AddForce(force, mode);
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

    public Vector3 GetLinearVelocity()
    {
        return _rb.linearVelocity;
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
