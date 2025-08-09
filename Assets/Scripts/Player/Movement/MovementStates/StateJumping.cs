public class StateJumping : IMovementState
{
    private MovementStateMachine _movementSM;

    private ILink[] links;

    public StateJumping(MovementStateMachine movementSM)
    {
        _movementSM = movementSM;

        Initialize();
    }

    private void Initialize()
    {
        // Bewusst ein Array da Einsparung an RAM und schnellerer durchlauf
        links[0] = _movementSM.linkAir;
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
