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

    // Movement
    private Vector3 _moveDirection;
    public float _moveSpeed { get; private set; }
    public float _startYScale { get; private set; }

    // Ground check
    public bool _onGround { get; private set; }
    public bool _onSlope { get; private set; }
    public bool _exitingSlope { get; private set; }
    public bool _readyToJump { get; private set; } = true;
    private RaycastHit _slopeHit;

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

        // Rigidbody
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;

        // Set default scale
        _startYScale = _rb.transform.localScale.y;

        _movementStateMachine = new MovementStateMachine();
        diContainer.BindInstance(this);
        diContainer.QueueForInject(_movementStateMachine);
    }

    private void Update()
    {
        // set isGrounded for this frame
        _onGround = IsGrounded();

        SpeedControl(_moveSpeed);

        _movementStateMachine.Update();

        HandleDrag(_onGround);
    }

    private void FixedUpdate()
    {
        _onSlope = OnSlope(
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

    #region Moving
    private void SpeedControl(float moveSpeed)
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

    public void HandleDrag(bool isGrounded)
    {
        _rb.linearDamping = isGrounded ? _playerMovementConfig.groundDrag : 0;
    }

    public bool IsGrounded()
    {
        return Physics.Raycast(
            _rb.transform.position,
            Vector3.down,
            _playerMovementConfig.playerHeight * 0.5f + 0.2f,
            _playerMovementConfig.whatIsGround
        );
    }
    #endregion

    #region Slope
    public bool OnSlope(Transform transform, float playerHeight, float maxSlopeAngle)
    {
        if (
            Physics.Raycast(
                transform.position,
                Vector3.down,
                out _slopeHit,
                playerHeight * 0.5f + 0.3f
            )
        )
        {
            float angle = Vector3.Angle(Vector3.up, _slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 moveDirection)
    {
        return Vector3.ProjectOnPlane(moveDirection, _slopeHit.normal).normalized;
    }
    #endregion
}
