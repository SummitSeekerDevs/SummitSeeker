using Zenject;

public class LinkCrouching : ILink
{
    private PlayerInputProvider _inputProvider;
    private StateCrouching _linkToState;

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
        return (playerMC._onGround || playerMC._onSlope) && _inputProvider._crouchingIsPressed;
    }

    public IMovementState GetLinkTo()
    {
        return _linkToState;
    }
}
