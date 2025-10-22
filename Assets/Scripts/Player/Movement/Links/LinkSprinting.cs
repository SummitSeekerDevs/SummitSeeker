using Zenject;

public class LinkSprinting : ITransitionLink
{
    internal PlayerInputProvider _inputProvider;
    private readonly StateSprinting _linkToState;
    private readonly MovementContext _movementContext;

    [Inject]
    public void Injection(PlayerInputProvider inputProvider)
    {
        _inputProvider = inputProvider;
    }

    public LinkSprinting(StateSprinting linkToState, MovementContext movementContext)
    {
        _linkToState = linkToState;
        _movementContext = movementContext;
    }

    public bool ConditionMatching()
    {
        return (_movementContext.ONGROUND || _movementContext.ONSLOPE)
            && _inputProvider._sprintingIsPressed;
    }

    public IMovementState GetLinkTo()
    {
        return _linkToState;
    }
}
