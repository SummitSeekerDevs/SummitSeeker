using Zenject;

public class LinkCrouching : ITransitionLink
{
    internal PlayerInputProvider _inputProvider;
    private readonly StateCrouching _linkToState;

    [Inject]
    public void Injection(PlayerInputProvider inputProvider)
    {
        _inputProvider = inputProvider;
    }

    public LinkCrouching(StateCrouching linkToState)
    {
        _linkToState = linkToState;
    }

    public bool ConditionMatching(PlayerMovementController playerMC)
    {
        return (playerMC.MOVEMENTCONTEXT.ONGROUND || playerMC.MOVEMENTCONTEXT.ONSLOPE)
            && _inputProvider._crouchingIsPressed;
    }

    public IMovementState GetLinkTo()
    {
        return _linkToState;
    }
}
