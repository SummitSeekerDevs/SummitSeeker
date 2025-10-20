public class LinkAir : ITransitionLink
{
    private readonly StateAir _linkToState;

    public LinkAir(StateAir linkToState)
    {
        _linkToState = linkToState;
    }

    public bool ConditionMatching(PlayerMovementController playerMC)
    {
        return !playerMC.MOVEMENTCONTEXT.ONGROUND && !playerMC.MOVEMENTCONTEXT.ONSLOPE;
    }

    public IMovementState GetLinkTo()
    {
        return _linkToState;
    }
}
