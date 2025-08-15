using UnityEngine;

public class StateCrouching : IMovementState
{
    private MovementStateMachine _movementSM;

    public StateCrouching(MovementStateMachine movementSM)
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
        Debug.Log("Crouching");
        _movementSM._playerMovementController.SetMovementSpeed(
            _movementSM._playerMovementController._playerMovementConfig.crouchSpeed
        );

        // set crouch
        _movementSM._playerMovementController.SetLocalYScale(
            _movementSM._playerMovementController._playerMovementConfig.crouchYScale
        );

        _movementSM._playerMovementController.AddMovingForce(Vector3.down * 5f, ForceMode.Impulse);
    }

    public void Exit()
    {
        // reset crouch
        _movementSM._playerMovementController.SetLocalYScale(
            _movementSM._playerMovementController._startYScale
        );
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
