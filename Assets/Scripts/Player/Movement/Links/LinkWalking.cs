using UnityEngine;
using Zenject;

public class LinkWalking : ILink
{
    private PlayerInputProvider _inputProvider;
    private readonly StateWalking _linkToState;

    [Inject]
    public void Injection(PlayerInputProvider inputProvider)
    {
        _inputProvider = inputProvider;
    }

    public LinkWalking(StateWalking linkToState)
    {
        _linkToState = linkToState;
    }

    public bool ConditionMatching(PlayerMovementController playerMC)
    {
        return (playerMC._onGround || playerMC._onSlope)
            && !_inputProvider._sprintingIsPressed
            && !_inputProvider._crouchingIsPressed
            && !_inputProvider._jumpingIsPressed;
    }

    public IMovementState GetLinkTo()
    {
        return _linkToState;
    }
}
