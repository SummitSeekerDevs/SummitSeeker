using UnityEngine;
using Zenject;

public class PlayerJumpHandler
{
    private bool _readyToJump = true;
    public bool _exitingSlope { get; private set; }

    // Dependencies
    private readonly Rigidbody _rb;
    private readonly PlayerMovementConfig _playerMovementConfig;
    private readonly DelayInvoker _delayInvoker;

    [Inject]
    public PlayerJumpHandler(
        Rigidbody rb,
        PlayerMovementConfig playerMovementConfig,
        DelayInvoker delayInvoker
    )
    {
        _rb = rb;
        _playerMovementConfig = playerMovementConfig;
        _delayInvoker = delayInvoker;
    }

    public void CheckJump(bool jumpIsPressed, bool isGrounded)
    {
        if (jumpIsPressed && _readyToJump && isGrounded)
        {
            Jump();
        }
    }

    private void Jump()
    {
        _readyToJump = false;

        _exitingSlope = true;

        // reset y velocity
        _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);

        _rb.AddForce(_rb.transform.up * _playerMovementConfig.jumpForce, ForceMode.Impulse);

        // jump cooldown
        _delayInvoker.InvokeDelayed(_playerMovementConfig.jumpCooldown, ResetJump);
    }

    private void ResetJump()
    {
        _readyToJump = true;

        _exitingSlope = false;
    }
}
