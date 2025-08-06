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

    // Movement
    private Vector3 _moveDirection;
    private float _moveSpeed;

    // Ground check
    private bool _isGrounded;

    #endregion

    [Inject]
    public void Construct(
        PlayerInputProvider playerInputProvider,
        PlayerStateMachine playerStateMachine,
        PlayerJumpHandler playerJumpHandler,
        PlayerCrouchHandler playerCrouchHandler,
        PlayerPhysicsHandler playerPhysicsHandler
    )
    {
        // Zenject injections
        _playerInputProvider = playerInputProvider;
        _playerStateMachine = playerStateMachine;
        _playerJumpHandler = playerJumpHandler;
        _playerCrouchHandler = playerCrouchHandler;
        _playerPhysicsHandler = playerPhysicsHandler;

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
        _isGrounded = _playerPhysicsHandler.IsGrounded();

        _playerJumpHandler.CheckJump(_playerInputProvider._jumpingIsPressed, _isGrounded);
        _playerPhysicsHandler.SpeedControl(_moveSpeed);

        _playerStateMachine.UpdateMovementState(
            _isGrounded,
            _playerInputProvider._crouchingIsPressed,
            _playerInputProvider._sprintingIsPressed
        );

        SetMovementSpeed();

        _playerPhysicsHandler.HandleDrag(_isGrounded);
    }

    private void FixedUpdate()
    {
        // calculate movement direction
        _moveDirection =
            _orientation.forward * _playerInputProvider._moveInput.y
            + _orientation.right * _playerInputProvider._moveInput.x;

        _playerPhysicsHandler.MovePlayer(_moveDirection, _moveSpeed, _isGrounded);

        if (!_isGrounded)
            _playerPhysicsHandler.ResetUnderMap(_spawnPoint.position);
    }

    #region Moving


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
