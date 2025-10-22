using Zenject;

public class LinkCrouching : ITransitionLink
{
    internal PlayerInputProvider _inputProvider;
    private readonly StateCrouching _linkToState;
    private readonly MovementContext _movementContext;

    [Inject]
    public void Injection(PlayerInputProvider inputProvider)
    {
        _inputProvider = inputProvider;
    }

    public LinkCrouching(StateCrouching linkToState, MovementContext movementContext)
    {
        _linkToState = linkToState;
        _movementContext = movementContext;
    }

    public bool ConditionMatching()
    {
        return (_movementContext.ONGROUND || _movementContext.ONSLOPE)
            && _inputProvider._crouchingIsPressed;
    }

    public IMovementState GetLinkTo()
    {
        return _linkToState;
    }
}
