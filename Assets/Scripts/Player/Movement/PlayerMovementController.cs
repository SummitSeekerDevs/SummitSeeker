using UnityEngine;
using Zenject;

public class PlayerMovementController : MonoBehaviour
{
    #region Vars
    // References
    [SerializeField]
    private Transform _spawnPoint;
    public Transform SPAWNPOINT => _spawnPoint;

    [SerializeField]
    private Transform _orientation;
    public PlayerMovementConfig _playerMovementConfig { get; private set; }

    private Rigidbody _rb;
    public Rigidbody PLAYER_RB => _rb;

    private PlayerInputProvider _playerInputProvider;
    private MovementStateMachine _movementStateMachine;
    private MovementFunctions _movementFunctions;
    public MovementFunctions MOVEMENTFUNCTIONS => _movementFunctions;

    // Movement
    private Vector3 _moveDirection;
    public float _moveSpeed { get; private set; }
    public float _startYScale { get; private set; }

    // Ground check
    public virtual bool _onGround { get; private set; }
    public virtual bool _onSlope { get; private set; }
    public bool _exitingSlope { get; private set; }
    public virtual bool _readyToJump { get; private set; } = true;

    #endregion

    [Inject]
    public void Construct(
        PlayerInputProvider playerInputProvider,
        PlayerMovementConfig playerMovementConfig,
        DiContainer diContainer
    )
    {
        // Zenject injections
        _playerInputProvider = playerInputProvider;
        _playerMovementConfig = playerMovementConfig;

        _movementStateMachine = new MovementStateMachine();
        diContainer.BindInstance(this);
        diContainer.QueueForInject(_movementStateMachine);
    }

    private void Start()
    {
        // Rigidbody
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;

        // Set default scale
        _startYScale = _rb.transform.localScale.y;

        // Movementfunctions
        _movementFunctions = new MovementFunctions();
    }

    private void Update()
    {
        // set isGrounded for this frame
        _onGround = _movementFunctions.IsGrounded(
            _rb,
            _playerMovementConfig.playerHeight,
            _playerMovementConfig.whatIsGround
        );

        _movementFunctions.SpeedControl(_rb, _moveSpeed, _onSlope, _exitingSlope);

        _movementStateMachine.Update();

        _movementFunctions.HandleDrag(_rb, _onGround, _playerMovementConfig.groundDrag);
    }

    private void FixedUpdate()
    {
        _onSlope = _movementFunctions.OnSlope(
            transform,
            _playerMovementConfig.playerHeight,
            _playerMovementConfig.maxSlopeAngle
        );

        // calculate movement direction
        _moveDirection =
            _orientation.forward * _playerInputProvider._moveInput.y
            + _orientation.right * _playerInputProvider._moveInput.x;

        _movementStateMachine.FixedUpdate(_moveDirection);
    }

    #region Setter
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
    #endregion
}
