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

    private PlayerStateMachine _playerStateMachine;
    private PlayerJumpHandler _playerJumpHandler;
    private PlayerCrouchHandler _playerCrouchHandler;
    private PlayerPhysicsHandler _playerPhysicsHandler;
    public PlayerSlopeHandler _playerSlopeHandler { get; private set; }

    // Movement
    private Vector3 _moveDirection;
    private float _moveSpeed;

    // Ground check
    public bool _onGround { get; private set; }
    public bool _onSlope { get; private set; }
    public bool _exitingSlope { get; private set; }

    #endregion

    [Inject]
    public void Construct(
        PlayerInputProvider playerInputProvider,
        PlayerStateMachine playerStateMachine,
        PlayerJumpHandler playerJumpHandler,
        PlayerCrouchHandler playerCrouchHandler,
        PlayerPhysicsHandler playerPhysicsHandler,
        PlayerSlopeHandler playerSlopeHandler
    )
    {
        // Zenject injections
        _playerInputProvider = playerInputProvider;
        _playerStateMachine = playerStateMachine;
        _playerJumpHandler = playerJumpHandler;
        _playerCrouchHandler = playerCrouchHandler;
        _playerPhysicsHandler = playerPhysicsHandler;
        _playerSlopeHandler = playerSlopeHandler;

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
        _onGround = _playerPhysicsHandler.IsGrounded();

        _playerJumpHandler.CheckJump(_playerInputProvider._jumpingIsPressed, _onGround);
        _playerPhysicsHandler.SpeedControl(_moveSpeed);

        _playerStateMachine.UpdateMovementState(
            _onGround,
            _playerInputProvider._crouchingIsPressed,
            _playerInputProvider._sprintingIsPressed
        );

        SetMovementSpeed();

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

        _playerPhysicsHandler.MovePlayer(_moveDirection, _moveSpeed, _onGround);

        if (!_onGround)
            _playerPhysicsHandler.ResetUnderMap(_spawnPoint.position);
    }

    #region Moving

    public void AddMovingForce(Vector3 forceBeforeSpeed)
    {
        _rb.AddForce(forceBeforeSpeed * _moveSpeed, ForceMode.Force);
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
