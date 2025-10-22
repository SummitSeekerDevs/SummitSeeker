using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class StateJumping : IMovementState
{
    private MovementContext _movementContext;
    private PlayerMovementConfig _movementConfig;
    private DelayInvoker _delayInvoker;

    public StateJumping(
        MovementContext movementContext,
        PlayerMovementConfig movementConfig,
        DelayInvoker delayInvoker
    )
    {
        _movementContext = movementContext;
        _movementConfig = movementConfig;
        _delayInvoker = delayInvoker;
    }

    public List<MovementStateMachine.TransitionLinkTypes> GetTransitionsList()
    {
        return new List<MovementStateMachine.TransitionLinkTypes>
        {
            MovementStateMachine.TransitionLinkTypes.LinkAir,
        };
    }

    public void Enter()
    {
        Debug.Log("Jumping");
        _movementContext.SetReadyToJump(false);

        _movementContext.SetExitingSlope(true);

        // reset y velocity
        _movementContext.AFFECTED_RIDGIDBODY.linearVelocity = new Vector3(
            _movementContext.AFFECTED_RIDGIDBODY.linearVelocity.x,
            0f,
            _movementContext.AFFECTED_RIDGIDBODY.linearVelocity.z
        );

        // statt transform möglicherweise rb erforderlich
        _movementContext.AFFECTED_RIDGIDBODY.AddForce(
            _movementContext.AFFECTED_RIDGIDBODY.transform.up * _movementConfig.jumpForce,
            ForceMode.Impulse
        );

        // jump cooldown
        _delayInvoker.InvokeDelayed(_movementConfig.jumpCooldown, ResetJump);
    }

    public void Exit()
    {
        // Nothing to do here
    }

    public void FixedUpdate(Vector3 moveDirection)
    {
        // turn gravity off while on slope
        _movementContext.AFFECTED_RIDGIDBODY.useGravity = !_movementContext.ONSLOPE;
    }

    public void Update()
    {
        // nothing
    }

    private void ResetJump()
    {
        _movementContext.SetReadyToJump(true);
        _movementContext.SetExitingSlope(false);
    }
}
