using Zenject;

public class LinkJumping : ITransitionLink
{
    internal PlayerInputProvider _inputProvider;
    private readonly StateJumping _linkToState;
    private readonly MovementContext _movementContext;

    [Inject]
    public void Injection(PlayerInputProvider inputProvider)
    {
        _inputProvider = inputProvider;
    }

    public LinkJumping(StateJumping linkToState, MovementContext movementContext)
    {
        _linkToState = linkToState;
        _movementContext = movementContext;
    }

    public bool ConditionMatching()
    {
        return (_movementContext.ONGROUND || _movementContext.ONSLOPE)
            && _inputProvider._jumpingIsPressed
            && _movementContext.READYTOJUMP;
    }

    public IMovementState GetLinkTo()
    {
        return _linkToState;
    }
}
