using UnityEngine;

public class StateCrouching : IMovementState
{
    private MovementStateMachine _movementSM;

    private ILink[] links;

    public StateCrouching(MovementStateMachine movementSM)
    {
        _movementSM = movementSM;

        Initialize();
    }

    private void Initialize()
    {
        // Bewusst ein Array da Einsparung an RAM und schnellerer durchlauf
        links[0] = _movementSM.linkJumping;
        links[1] = _movementSM.linkWalking;
        links[2] = _movementSM.linkAir;
    }

    public void Enter()
    {
        throw new System.NotImplementedException();
    }

    public void Exit()
    {
        throw new System.NotImplementedException();
    }

    public void FixedUpdate()
    {
        throw new System.NotImplementedException();
    }

    public void Update()
    {
        throw new System.NotImplementedException();
    }
}
