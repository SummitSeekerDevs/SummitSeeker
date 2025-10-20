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

    private PlayerMovementConfig _playerMovementConfig;

    private Rigidbody _rb;

    private PlayerInputProvider _playerInputProvider;
    private MovementStateMachine _movementStateMachine;
    private MovementFunctions _movementFunctions;
    private MovementContext _movementContext = new MovementContext();

    // GETTER
    public Transform SPAWNPOINT => _spawnPoint;
    public virtual Rigidbody PLAYER_RB => _rb;
    public MovementFunctions MOVEMENTFUNCTIONS => _movementFunctions;
    public PlayerMovementConfig PLAYERMOVEMENTCONFIG => _playerMovementConfig;
    public MovementContext MOVEMENTCONTEXT => _movementContext;

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
        _movementContext.SetStartYScale(_rb.transform.localScale.y);

        // Movementfunctions
        _movementFunctions = new MovementFunctions();
    }

    private void Update()
    {
        // set isGrounded for this frame
        _movementContext.SetOnGround(
            _movementFunctions.IsGrounded(
                _rb,
                _playerMovementConfig.playerHeight,
                _playerMovementConfig.whatIsGround
            )
        );

        _movementFunctions.SpeedControl(
            _rb,
            _movementContext.MOVESPEED,
            _movementContext.ONSLOPE,
            _movementContext.EXITINGSLOPE
        );

        _movementStateMachine.Update();

        _movementFunctions.HandleDrag(
            _rb,
            _movementContext.ONGROUND,
            _playerMovementConfig.groundDrag
        );
    }

    private void FixedUpdate()
    {
        _movementContext.SetOnSlope(
            _movementFunctions.OnSlope(
                transform,
                _playerMovementConfig.playerHeight,
                _playerMovementConfig.maxSlopeAngle
            )
        );

        // calculate movement direction
        _movementContext.SetMoveDirection(
            _orientation.forward * _playerInputProvider._moveInput.y
                + _orientation.right * _playerInputProvider._moveInput.x
        );

        _movementStateMachine.FixedUpdate(_movementContext.MOVEDIRECTION);
    }
}
