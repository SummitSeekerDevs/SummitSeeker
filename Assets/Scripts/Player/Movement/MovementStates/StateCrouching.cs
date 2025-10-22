using System.Collections.Generic;
using UnityEngine;

public class StateCrouching : IMovementState
{
    private MovementContext _movementContext;
    private PlayerMovementConfig _movementConfig;

    public StateCrouching(MovementContext movementContext, PlayerMovementConfig movementConfig)
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
        Debug.Log("Crouching");
        _movementContext.SetMoveSpeed(_movementConfig.crouchSpeed);

        // set crouch
        _movementContext.AFFECTED_RIDGIDBODY.transform.localScale = new Vector3(
            _movementContext.AFFECTED_RIDGIDBODY.transform.localScale.x,
            _movementConfig.crouchYScale,
            _movementContext.AFFECTED_RIDGIDBODY.transform.localScale.z
        );

        _movementContext.AFFECTED_RIDGIDBODY.AddForce(Vector3.down * 5f, ForceMode.Impulse);
    }

    public void Exit()
    {
        // reset crouch
        _movementContext.AFFECTED_RIDGIDBODY.transform.localScale = new Vector3(
            _movementContext.AFFECTED_RIDGIDBODY.transform.localScale.x,
            _movementContext.STARTYSCALE,
            _movementContext.AFFECTED_RIDGIDBODY.transform.localScale.z
        );
    }

    public void FixedUpdate(Vector3 moveDirection)
    {
        // on slope
        if (_movementContext.ONSLOPE && !_movementContext.EXITINGSLOPE)
        {
            _movementContext.AFFECTED_RIDGIDBODY.AddForce(
                _movementSM._playerMovementController.MOVEMENTFUNCTIONS.GetSlopeMoveDirection(
                    moveDirection
                )
                    * 20f
                    * _movementContext.MOVESPEED
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
                moveDirection.normalized * 10f * _movementContext.MOVESPEED
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
