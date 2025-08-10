using UnityEngine;

public class StateWalking : IMovementState
{
    private MovementStateMachine _movementSM;

    private ILink[] links = new ILink[4];

    public StateWalking(MovementStateMachine movementSM)
    {
        _movementSM = movementSM;
    }

    public void Initialize()
    {
        // Bewusst ein Array da Einsparung an RAM und schnellerer durchlauf
        links[0] = _movementSM.linkJumping;
        links[1] = _movementSM.linkSprinting;
        links[2] = _movementSM.linkAir;
        links[3] = _movementSM.linkCrouching;
    }

    public void Enter()
    {
        Debug.Log("Walking");
        _movementSM._playerMovementController.SetMovementSpeed(
            _movementSM._playerMovementController._playerMovementConfig.walkSpeed
        );
    }

    public void Exit()
    {
        // nothing to do here
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
        // Check transitions
        for (int i = 0; i < links.Length; i++)
        {
            if (links[i].ConditionMatching(_movementSM._playerMovementController))
            {
                _movementSM.TransitionTo(links[i].GetLinkTo());
            }
        }
    }
}
