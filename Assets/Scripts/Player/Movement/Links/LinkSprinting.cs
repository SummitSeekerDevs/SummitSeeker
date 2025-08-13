using Zenject;

public class LinkSprinting : ITransitionLink
{
    private PlayerInputProvider _inputProvider;
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
        return (playerMC._onGround || playerMC._onSlope) && _inputProvider._sprintingIsPressed;
    }

    public IMovementState GetLinkTo()
    {
        return _linkToState;
    }
}
