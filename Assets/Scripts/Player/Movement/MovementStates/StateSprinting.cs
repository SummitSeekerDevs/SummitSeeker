using UnityEngine;

public class StateSprinting : IMovementState
{
    private MovementStateMachine _movementSM;

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
        _movementSM._playerMovementController.MOVEMENTCONTEXT.SetMoveSpeed(
            _movementSM._playerMovementController.PLAYERMOVEMENTCONFIG.sprintSpeed
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
            _movementSM._playerMovementController.MOVEMENTCONTEXT.ONSLOPE
            && !_movementSM._playerMovementController.MOVEMENTCONTEXT.EXITINGSLOPE
        )
        {
            _movementSM._playerMovementController.PLAYER_RB.AddForce(
                _movementSM._playerMovementController.MOVEMENTFUNCTIONS.GetSlopeMoveDirection(
                    moveDirection
                )
                    * 20f
                    * _movementSM._playerMovementController.MOVEMENTCONTEXT.MOVESPEED
            );

            if (_movementSM._playerMovementController.PLAYER_RB.linearVelocity.y > 0)
            {
                _movementSM._playerMovementController.PLAYER_RB.AddForce(Vector3.down * 80f);
            }
        }
        // on ground
        else if (_movementSM._playerMovementController.MOVEMENTCONTEXT.ONGROUND)
        {
            _movementSM._playerMovementController.PLAYER_RB.AddForce(
                moveDirection.normalized
                    * 10f
                    * _movementSM._playerMovementController.MOVEMENTCONTEXT.MOVESPEED
            );
        }

        // turn gravity off while on slope
        _movementSM._playerMovementController.PLAYER_RB.useGravity = !_movementSM
            ._playerMovementController
            .MOVEMENTCONTEXT
            .ONSLOPE;
    }

    public void Update()
    {
        // nothing
    }
}
