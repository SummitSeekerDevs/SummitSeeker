using UnityEngine;
using Zenject;

public class StateJumping : IMovementState
{
    private MovementStateMachine _movementSM;
    private DelayInvoker _delayInvoker;

    public StateJumping(MovementStateMachine movementSM)
    {
        _movementSM = movementSM;
    }

    [Inject]
    public void Construct(DelayInvoker delayInvoker)
    {
        _delayInvoker = delayInvoker;
    }

    public void Initialize()
    {
        _movementSM.AddTransition(_movementSM.stateJumping, _movementSM.linkAir);
    }

    public void Enter()
    {
        Debug.Log("Jumping");
        _movementSM._playerMovementController.SetReadyToJump(false);

        _movementSM._playerMovementController.SetExitingSlope(true);

        // reset y velocity
        _movementSM._playerMovementController.SetLinearVelocity(
            new Vector3(
                _movementSM._playerMovementController.GetLinearVelocity().x,
                0f,
                _movementSM._playerMovementController.GetLinearVelocity().z
            )
        );

        // statt transform möglicherweise rb erforderlich
        _movementSM._playerMovementController.AddMovingForce(
            _movementSM._playerMovementController.transform.up
                * _movementSM._playerMovementController._playerMovementConfig.jumpForce,
            ForceMode.Impulse
        );

        // jump cooldown
        _delayInvoker.InvokeDelayed(
            _movementSM._playerMovementController._playerMovementConfig.jumpCooldown,
            ResetJump
        );
    }

    public void Exit()
    {
        // Nothing to do here
    }

    public void FixedUpdate(Vector3 moveDirection)
    {
        // turn gravity off while on slope
        _movementSM._playerMovementController.ToggleGravity(
            !_movementSM._playerMovementController._onSlope
        );
    }

    public void Update()
    {
        // nothing
    }

    private void ResetJump()
    {
        _movementSM._playerMovementController.SetReadyToJump(true);
        _movementSM._playerMovementController.SetExitingSlope(false);
    }
}
