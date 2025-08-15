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
            _movementSM._playerMovementController.PLAYER_RB.AddForce(
                moveDirection.normalized
                    * _movementSM._playerMovementController._moveSpeed
                    * 10f
                    * _movementSM._playerMovementController._playerMovementConfig.airMultiplier
            );
        }

        // turn gravity off while on slope
        _movementSM._playerMovementController.PLAYER_RB.useGravity = !_movementSM
            ._playerMovementController
            ._onSlope;

        // Reset to spawn if fallen in void
        ResetUnderMap(_movementSM._playerMovementController._spawnPoint.position);
    }

    public void Update()
    {
        // nothing
    }

    internal void ResetUnderMap(Vector3 spawnpoint)
    {
        if (_movementSM._playerMovementController.PLAYER_RB.position.y <= -15f)
        {
            _movementSM._playerMovementController.PLAYER_RB.position = spawnpoint;
        }
    }
}
