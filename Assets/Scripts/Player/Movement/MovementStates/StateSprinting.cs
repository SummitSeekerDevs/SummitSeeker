using UnityEngine;

public class StateSprinting : IMovementState
{
    private MovementStateMachine _movementSM;

    private ILink[] links = new ILink[3];

    public StateSprinting(MovementStateMachine movementSM)
    {
        _movementSM = movementSM;
    }

    public void Initialize()
    {
        _movementSM.AddTransition(this, _movementSM.linkJumping);
        _movementSM.AddTransition(this, _movementSM.linkWalking);
        _movementSM.AddTransition(this, _movementSM.linkAir);
    }

    public void Enter()
    {
        Debug.Log("Sprinting");
        _movementSM._playerMovementController.SetMovementSpeed(
            _movementSM._playerMovementController._playerMovementConfig.sprintSpeed
        );
    }

    public void Exit()
    {
        // Nothing to do here
    }

    public void FixedUpdate(Vector3 moveDirection)
    {
        // on slope
        if (
            _movementSM._playerMovementController._onSlope
            && !_movementSM._playerMovementController._exitingSlope
        )
        {
            _movementSM._playerMovementController.AddMovingForce(
                _movementSM._playerMovementController._playerSlopeHandler.GetSlopeMoveDirection(
                    moveDirection
                )
                    * 20f
                    * _movementSM._playerMovementController._moveSpeed
            );

            if (_movementSM._playerMovementController.GetLinearVelocity().y > 0)
            {
                _movementSM._playerMovementController.AddMovingForce(Vector3.down * 80f);
            }
        }
        // on ground
        else if (_movementSM._playerMovementController._onGround)
        {
            _movementSM._playerMovementController.AddMovingForce(
                moveDirection.normalized * 10f * _movementSM._playerMovementController._moveSpeed
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
