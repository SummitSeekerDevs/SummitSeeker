using UnityEngine;

public class StateAir : IMovementState
{
    private MovementStateMachine _movementSM;

    public StateAir(MovementStateMachine movementSM)
    {
        _movementSM = movementSM;
    }

    public void Initialize()
    {
        _movementSM.AddTransition(this, _movementSM.linkWalking);
        _movementSM.AddTransition(this, _movementSM.linkSprinting);
        _movementSM.AddTransition(this, _movementSM.linkJumping);
    }

    public void Enter()
    {
        Debug.Log("Air");
    }

    public void Exit()
    {
        // Nothing to do here
    }

    public void FixedUpdate(Vector3 moveDirection)
    {
        if (!_movementSM._playerMovementController._onGround)
        {
            _movementSM._playerMovementController.AddMovingForce(
                moveDirection.normalized
                    * _movementSM._playerMovementController._moveSpeed
                    * 10f
                    * _movementSM._playerMovementController._playerMovementConfig.airMultiplier
            );
        }

        // turn gravity off while on slope
        _movementSM._playerMovementController.ToggleGravity(
            !_movementSM._playerMovementController._onSlope
        );
    }

    public void Update()
    {
        // nothing
    }
}
