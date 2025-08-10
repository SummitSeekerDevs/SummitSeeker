using UnityEngine;
using Zenject;

public class StateJumping : IMovementState
{
    private MovementStateMachine _movementSM;
    private DelayInvoker _delayInvoker;

    private ILink[] links = new ILink[1];

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
        // Bewusst ein Array da Einsparung an RAM und schnellerer durchlauf
        links[0] = _movementSM.linkAir;
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
        // // on slope
        // if (
        //     _movementSM._playerMovementController._onSlope
        //     && !_movementSM._playerMovementController._exitingSlope
        // )
        // {
        //     _movementSM._playerMovementController.AddMovingForce(
        //         _movementSM._playerMovementController._playerSlopeHandler.GetSlopeMoveDirection(
        //             moveDirection
        //         )
        //             * 20f
        //             * _movementSM._playerMovementController._moveSpeed
        //     );
        //
        //     if (_movementSM._playerMovementController.GetLinearVelocity().y > 0)
        //     {
        //         _movementSM._playerMovementController.AddMovingForce(Vector3.down * 80f);
        //     }
        // }
        // // on ground
        // else if (_movementSM._playerMovementController._onGround)
        // {
        //     _movementSM._playerMovementController.AddMovingForce(
        //         moveDirection.normalized * 10f * _movementSM._playerMovementController._moveSpeed
        //     );
        // }

        // turn gravity off while on slope
        _movementSM._playerMovementController.ToggleGravity(
            !_movementSM._playerMovementController._onSlope
        );
    }

    public void Update()
    {
        // Check transitions
        for (int i = 0; i < links.Length; i++)
        {
            if (links[i].ConditionMatching(_movementSM._playerMovementController))
            {
                _movementSM.TransitionTo(links[i].GetLinkTo());
            }
        }
    }

    private void ResetJump()
    {
        _movementSM._playerMovementController.SetReadyToJump(true);
        _movementSM._playerMovementController.SetExitingSlope(false);
    }
}
