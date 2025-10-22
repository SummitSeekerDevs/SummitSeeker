using UnityEngine;

public class MovementContext
{
    // Rigidbody
    private Rigidbody _rigidbody;

    // Movement
    private Vector3 _moveDirection;
    private float _moveSpeed;
    private float _startYScale;

    // Ground check
    private bool _onGround;
    private bool _onSlope;
    private RaycastHit _slopeHit;
    private bool _exitingSlope;
    private bool _readyToJump = true;

    // Spawning
    private Vector3 _spawnPoint;

    // Getter
    public Vector3 MOVEDIRECTION => _moveDirection;
    public float MOVESPEED => _moveSpeed;
    public float STARTYSCALE => _startYScale;
    public bool ONGROUND => _onGround;
    public bool ONSLOPE => _onSlope;
    public RaycastHit SLOPEHIT => _slopeHit;
    public bool EXITINGSLOPE => _exitingSlope;
    public bool READYTOJUMP => _readyToJump;
    public Rigidbody AFFECTED_RIDGIDBODY => _rigidbody;
    public Vector3 SPAWNPOINT => _spawnPoint;

    public MovementContext(Rigidbody rigidbody)
    {
        _rigidbody = rigidbody;
    }

    // Setter
    #region Setter
    public void SetMoveDirection(Vector3 moveDirection)
    {
        _moveDirection = moveDirection;
    }

    public void SetMoveSpeed(float moveSpeed)
    {
        _moveSpeed = moveSpeed;
    }

    public void SetStartYScale(float startYScale)
    {
        _startYScale = startYScale;
    }

    public void SetOnGround(bool onGround)
    {
        _onGround = onGround;
    }

    public void SetOnSlope(bool onSlope, RaycastHit slopeHit)
    {
        _onSlope = onSlope;
        _slopeHit = slopeHit;
    }

    public void SetExitingSlope(bool exitingSlope)
    {
        _exitingSlope = exitingSlope;
    }

    public void SetReadyToJump(bool readyToJump)
    {
        _readyToJump = readyToJump;
    }

    public void SetSpawnPoint(Vector3 spawnPoint)
    {
        _spawnPoint = spawnPoint;
    }

    #endregion
}
