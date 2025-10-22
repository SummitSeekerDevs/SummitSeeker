using UnityEngine;
using Zenject;

public class PlayerMovementController : MonoBehaviour
{
    #region Vars

    // References
    [SerializeField]
    private Transform _initialSpawnPoint;

    [SerializeField]
    private Transform _orientation;

    private Rigidbody _rb;

    private PlayerInputProvider _playerInputProvider;
    private MovementStateMachine _movementStateMachine;
    private MovementContext _movementContext;
    private PlayerMovementConfig _playerMovementConfig;

    #endregion

    [Inject]
    public void Construct(
        PlayerInputProvider playerInputProvider,
        PlayerMovementConfig playerMovementConfig,
        DelayInvoker delayInvoker,
        DiContainer diContainer
    )
    {
        // Zenject injections
        _playerInputProvider = playerInputProvider;
        _playerMovementConfig = playerMovementConfig;

        // Rigidbody
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;

        // Create MovementContext
        _movementContext = new MovementContext(_rb);

        _movementStateMachine = new MovementStateMachine(
            _movementContext,
            _playerMovementConfig,
            delayInvoker,
            diContainer
        );
    }

    private void Start()
    {
        // Set default scale
        _movementContext.SetStartYScale(_rb.transform.localScale.y);

        // Set spawnpoint to default
        _movementContext.SetSpawnPoint(_initialSpawnPoint.position);
    }

    private void Update()
    {
        // set isGrounded for this frame
        _movementContext.SetOnGround(
            MovementFunctions.IsGrounded(
                _rb,
                _playerMovementConfig.playerHeight,
                _playerMovementConfig.whatIsGround
            )
        );

        MovementFunctions.SpeedControl(
            _rb,
            _movementContext.MOVESPEED,
            _movementContext.ONSLOPE,
            _movementContext.EXITINGSLOPE
        );

        _movementStateMachine.Update();

        MovementFunctions.HandleDrag(
            _rb,
            _movementContext.ONGROUND,
            _playerMovementConfig.groundDrag
        );
    }

    private void FixedUpdate()
    {
        _movementContext.SetOnSlope(
            MovementFunctions.OnSlope(
                transform,
                _playerMovementConfig.playerHeight,
                _playerMovementConfig.maxSlopeAngle,
                out RaycastHit slopeHit
            ),
            slopeHit
        );

        // calculate movement direction
        _movementContext.SetMoveDirection(
            _orientation.forward * _playerInputProvider._moveInput.y
                + _orientation.right * _playerInputProvider._moveInput.x
        );

        _movementStateMachine.FixedUpdate(_movementContext.MOVEDIRECTION);
    }
}
