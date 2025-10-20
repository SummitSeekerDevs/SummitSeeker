using Zenject;

public class LinkSprinting : ITransitionLink
{
    internal PlayerInputProvider _inputProvider;
    private readonly StateSprinting _linkToState;

    [Inject]
    public void Injection(PlayerInputProvider inputProvider)
    {
        _inputProvider = inputProvider;
    }

    public LinkSprinting(StateSprinting linkToState)
    {
        _linkToState = linkToState;
    }

    public bool ConditionMatching(PlayerMovementController playerMC)
    {
        return (playerMC.MOVEMENTCONTEXT.ONGROUND || playerMC.MOVEMENTCONTEXT.ONSLOPE)
            && _inputProvider._sprintingIsPressed;
    }

    public IMovementState GetLinkTo()
    {
        return _linkToState;
    }
}
