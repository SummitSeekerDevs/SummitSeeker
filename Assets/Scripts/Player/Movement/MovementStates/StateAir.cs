using System.Collections.Generic;
using UnityEngine;

public class StateAir : IMovementState
{
    private MovementContext _movementContext;
    private PlayerMovementConfig _movementConfig;

    public StateAir(MovementContext movementContext, PlayerMovementConfig movementConfig)
    {
        _movementContext = movementContext;
        _movementConfig = movementConfig;
    }

    public List<MovementStateMachine.TransitionLinkTypes> GetTransitionsList()
    {
        return new List<MovementStateMachine.TransitionLinkTypes>
        {
            MovementStateMachine.TransitionLinkTypes.LinkWalking,
            MovementStateMachine.TransitionLinkTypes.LinkSprinting,
            MovementStateMachine.TransitionLinkTypes.LinkJumping,
        };
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
        if (!_movementContext.ONGROUND)
        {
            _movementContext.AFFECTED_RIDGIDBODY.AddForce(
                moveDirection.normalized
                    * (_movementContext.MOVESPEED * 10f * _movementConfig.airMultiplier)
            );
        }

        // turn gravity off while on slope
        _movementContext.AFFECTED_RIDGIDBODY.useGravity = !_movementContext.ONSLOPE;

        // Reset to spawn if fallen in void
        ResetUnderMap(_movementContext.SPAWNPOINT);
    }

    public void Update()
    {
        // nothing
    }

    internal void ResetUnderMap(Vector3 spawnpoint)
    {
        if (_movementContext.AFFECTED_RIDGIDBODY.position.y <= -15f)
        {
            _movementContext.AFFECTED_RIDGIDBODY.position = spawnpoint;
        }
    }
}
