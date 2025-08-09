using UnityEngine;

public class StateWalking : IMovementState
{
    private MovementStateMachine _movementSM;

    private ILink[] links;

    public StateWalking(MovementStateMachine movementSM)
    {
        _movementSM = movementSM;

        Initialize();
    }

    private void Initialize()
    {
        // Bewusst ein Array da Einsparung an RAM und schnellerer durchlauf
        links[0] = _movementSM.linkJumping;
        links[1] = _movementSM.linkSprinting;
        links[2] = _movementSM.linkAir;
        links[3] = _movementSM.linkCrouching;
    }

    public void Enter()
    {
        // nothing to do here
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

            _movementSM._playerMovementController._playerSlopeHandler.GetSlopeMoveDirection(moveDirection) * 20f

            

            if (_rb.linearVelocity.y > 0)
                _rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }
        // on ground
        else if (isGrounded)
            _rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
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
