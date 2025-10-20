using Zenject;

public class LinkJumping : ITransitionLink
{
    internal PlayerInputProvider _inputProvider;
    private readonly StateJumping _linkToState;

    [Inject]
    public void Injection(PlayerInputProvider inputProvider)
    {
        _inputProvider = inputProvider;
    }

    public LinkJumping(StateJumping linkToState)
    {
        _linkToState = linkToState;
    }

    public bool ConditionMatching(PlayerMovementController playerMC)
    {
        return (playerMC.MOVEMENTCONTEXT.ONGROUND || playerMC.MOVEMENTCONTEXT.ONSLOPE)
            && _inputProvider._jumpingIsPressed
            && playerMC.MOVEMENTCONTEXT.READYTOJUMP;
    }

    public IMovementState GetLinkTo()
    {
        return _linkToState;
    }
}
