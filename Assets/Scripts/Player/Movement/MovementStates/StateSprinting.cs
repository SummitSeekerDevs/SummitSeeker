using System.Collections.Generic;
using UnityEngine;

public class StateSprinting : IMovementState
{
    private MovementContext _movementContext;
    private PlayerMovementConfig _movementConfig;

    public StateSprinting(MovementContext movementContext, PlayerMovementConfig movementConfig)
    {
        _movementContext = movementContext;
        _movementConfig = movementConfig;
    }

    public List<MovementStateMachine.TransitionLinkTypes> GetTransitionsList()
    {
        return new List<MovementStateMachine.TransitionLinkTypes>
        {
            MovementStateMachine.TransitionLinkTypes.LinkJumping,
            MovementStateMachine.TransitionLinkTypes.LinkWalking,
            MovementStateMachine.TransitionLinkTypes.LinkAir,
        };
    }

    public void Enter()
    {
        Debug.Log("Sprinting");
        _movementContext.SetMoveSpeed(_movementConfig.sprintSpeed);
    }

    public void Exit()
    {
        // Nothing to do here
    }

    public void FixedUpdate(Vector3 moveDirection)
    {
        // on slope
        if (_movementContext.ONSLOPE && !_movementContext.EXITINGSLOPE)
        {
            _movementContext.AFFECTED_RIDGIDBODY.AddForce(
                MovementFunctions.GetSlopeMoveDirection(moveDirection, _movementContext.SLOPEHIT)
                    * (20f * _movementContext.MOVESPEED)
            );

            if (_movementContext.AFFECTED_RIDGIDBODY.linearVelocity.y > 0)
            {
                _movementContext.AFFECTED_RIDGIDBODY.AddForce(Vector3.down * 80f);
            }
        }
        // on ground
        else if (_movementContext.ONGROUND)
        {
            _movementContext.AFFECTED_RIDGIDBODY.AddForce(
                moveDirection.normalized * (10f * _movementContext.MOVESPEED)
            );
        }

        // turn gravity off while on slope
        _movementContext.AFFECTED_RIDGIDBODY.useGravity = !_movementContext.ONSLOPE;
    }

    public void Update()
    {
        // nothing
    }
}
