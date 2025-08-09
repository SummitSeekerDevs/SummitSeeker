public class LinkAir : ILink
{
    private StateAir _linkToState;

    public LinkAir(StateAir linkToState)
    {
        _linkToState = linkToState;
    }

    public bool ConditionMatching(PlayerMovementController playerMC)
    {
        return !playerMC._onGround && !playerMC._onSlope;
    }

    public IMovementState GetLinkTo()
    {
        return _linkToState;
    }
}
