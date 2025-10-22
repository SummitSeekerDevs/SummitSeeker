public class LinkAir : ITransitionLink
{
    private readonly StateAir _linkToState;
    private readonly MovementContext _movementContext;

    public LinkAir(StateAir linkToState, MovementContext movementContext)
    {
        _linkToState = linkToState;
        _movementContext = movementContext;
    }

    public bool ConditionMatching()
    {
        return !_movementContext.ONGROUND && !_movementContext.ONSLOPE;
    }

    public IMovementState GetLinkTo()
    {
        return _linkToState;
    }
}
