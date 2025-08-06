using UnityEngine;

public class PlayerPhysicsHandler
{
    private readonly Rigidbody _rb;
    private readonly PlayerMovementConfig _playerMovementConfig;
    private readonly PlayerSlopeHandler _playerSlopeHandler;
    private readonly PlayerJumpHandler _playerJumpHandler;

    public PlayerPhysicsHandler(
        Rigidbody rb,
        PlayerMovementConfig playerMovementConfig,
        PlayerSlopeHandler playerSlopeHandler,
        PlayerJumpHandler playerJumpHandler
    )
    {
        _rb = rb;
        _playerMovementConfig = playerMovementConfig;
        _playerSlopeHandler = playerSlopeHandler;
        _playerJumpHandler = playerJumpHandler;
    }

    public void MovePlayer(Vector3 moveDirection, float moveSpeed, bool isGrounded)
    {
        // on slope
        if (
            _playerSlopeHandler.OnSlope(
                _rb.transform,
                _playerMovementConfig.playerHeight,
                _playerMovementConfig.maxSlopeAngle
            ) && !_playerJumpHandler._exitingSlope
        )
        {
            _rb.AddForce(
                _playerSlopeHandler.GetSlopeMoveDirection(moveDirection) * moveSpeed * 20f,
                ForceMode.Force
            );

            if (_rb.linearVelocity.y > 0)
                _rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }
        // on ground
        else if (isGrounded)
            _rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        // in air
        else if (!isGrounded)
        {
            _rb.AddForce(
                moveDirection.normalized * moveSpeed * 10f * _playerMovementConfig.airMultiplier,
                ForceMode.Force
            );
        }

        // turn gravity off while on slope
        _rb.useGravity = !_playerSlopeHandler.OnSlope(
            _rb.transform,
            _playerMovementConfig.playerHeight,
            _playerMovementConfig.maxSlopeAngle
        );
    }

    public void SpeedControl(float moveSpeed)
    {
        // limiting speed on slope
        if (
            _playerSlopeHandler.OnSlope(
                _rb.transform,
                _playerMovementConfig.playerHeight,
                _playerMovementConfig.maxSlopeAngle
            ) && !_playerJumpHandler._exitingSlope
        )
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

    public bool IsGrounded()
    {
        return Physics.Raycast(
            _rb.transform.position,
            Vector3.down,
            _playerMovementConfig.playerHeight * 0.5f + 0.2f,
            _playerMovementConfig.whatIsGround
        );
    }

    // TODO: Überlegen ob es nicht besser geht als diese Methode öffentlich zu machen, da sie sehr sensible Eigenschaft vom RB setzt
    public void HandleDrag(bool isGrounded)
    {
        _rb.linearDamping = isGrounded ? _playerMovementConfig.groundDrag : 0;
    }

    internal void ResetUnderMap(Vector3 spawnpoint)
    {
        if (_rb.position.y <= -15f)
        {
            _rb.position = spawnpoint;
        }
    }
}
