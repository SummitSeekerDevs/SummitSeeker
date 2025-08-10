using Zenject;

public class LinkJumping : ILink
{
    private PlayerInputProvider _inputProvider;
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
        return (playerMC._onGround || playerMC._onSlope)
            && _inputProvider._jumpingIsPressed
            && playerMC._readyToJump;
    }

    public IMovementState GetLinkTo()
    {
        return _linkToState;
    }
}
